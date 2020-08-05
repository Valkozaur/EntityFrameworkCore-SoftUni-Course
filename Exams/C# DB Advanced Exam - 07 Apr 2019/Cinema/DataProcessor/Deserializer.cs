using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ImportDto;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using Newtonsoft.Json;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var movies = new List<Movie>();

            var moviesDTOs = JsonConvert.DeserializeObject<ImportMoviesDTO[]>(jsonString);

            foreach (var movieDTO in moviesDTOs)
            {
                if (!IsValid(movieDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre;
                if (!Enum.TryParse(typeof(Genre), movieDTO.Genre, false, out object outObjectResult))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                genre = (Genre)outObjectResult;

                if (context.Movies.Any(m => m.Title == movieDTO.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie()
                {
                    Title = movieDTO.Title,
                    Duration = movieDTO.Duration,
                    Genre = genre,
                    Director = movieDTO.Director,
                    Rating = movieDTO.Rating
                };

                movies.Add(movie);
                sb.AppendLine(String.Format(SuccessfulImportMovie, movie.Title, movie.Genre.ToString(), movie.Rating.ToString("F2")));
            }

            context.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var halls = new List<Hall>();

            var hallsDTOs = JsonConvert.DeserializeObject<ImportHallsAndSeatsDTO[]>(jsonString);

            foreach (var hallsDTO in hallsDTOs)
            {
                if (!IsValid(hallsDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall()
                {
                    Name = hallsDTO.Name,
                    Is3D = hallsDTO.Is3D,
                    Is4Dx = hallsDTO.Is4Dx
                };

                for (int i = 0; i < hallsDTO.Seats; i++)
                { 
                    hall.Seats.Add(new Seat());
                }

                var projectionType = "Normal";

                if (hall.Is3D && hall.Is4Dx)
                {
                    projectionType = "4Dx/3D";
                }
                else if (hall.Is3D)
                {
                    projectionType = "3D";
                }
                else if (hall.Is4Dx)
                {
                    projectionType = "4Dx";
                }

                halls.Add(hall);
                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportProjectioDTO[] projectionDTOs;

            using (StringReader stream = new StringReader(xmlString))
            {
                var xml = new XmlSerializer(typeof(ImportProjectioDTO[]),new XmlRootAttribute("Projections"));
                projectionDTOs =  (ImportProjectioDTO[])xml.Deserialize(stream);
            }

            var projections = new List<Projection>();

            foreach (var projectionDTO in projectionDTOs)
            {
                if (!IsValid(projectionDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Movies.Any(m => m.Id == projectionDTO.MovieId) || !context.Halls.Any(h => h.Id == projectionDTO.HallId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime dateTime;
                var dateIsValid = DateTime.TryParse(projectionDTO.DateTime,
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out dateTime);

                if (!dateIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection()
                {
                    MovieId = projectionDTO.MovieId,
                    HallId = projectionDTO.HallId,
                    DateTime = dateTime
                };

                var movieTitle = context.Movies.Find(projection.MovieId).Title;

                projections.Add(projection);
                sb.AppendFormat(SuccessfulImportProjection,
                    movieTitle, 
                    projection.DateTime.ToString("MM/dd/yyyy"));
                sb.AppendLine();
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();
            ImportCustomerDTO[] customerDTOs;

            using (StringReader stream = new StringReader(xmlString))
            {
                var xmlSerializer = new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));
                customerDTOs = (ImportCustomerDTO[])xmlSerializer.Deserialize(stream);
            }

            var customers = new List<Customer>();

            foreach (var customerDTO in customerDTOs)
            {
                if (!IsValid(customerDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer()
                {
                    FirstName = customerDTO.FirstName,
                    LastName =  customerDTO.LastName,
                    Age = customerDTO.Age,
                    Balance = customerDTO.Balance
                };

                var tickets = new List<Ticket>();
                foreach (var ticketDTO in customerDTO.Tickets)
                {
                    if (!IsValid(ticketDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    
                    if (!context.Projections.Any(p => p.Id == ticketDTO.ProjectionId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var ticket = new Ticket()
                    {
                        Price = ticketDTO.Price,
                        ProjectionId = ticketDTO.ProjectionId
                    };

                    tickets.Add(ticket);
                }

                customer.Tickets = tickets;
                customers.Add(customer);

                sb.AppendFormat(SuccessfulImportCustomerTicket,
                    customer.FirstName,
                    customer.LastName,
                    customer.Tickets.Count);
                sb.AppendLine();
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}