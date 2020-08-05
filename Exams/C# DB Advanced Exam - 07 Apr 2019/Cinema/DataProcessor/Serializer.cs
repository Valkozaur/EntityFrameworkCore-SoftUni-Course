using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cinema.DataProcessor.ExportDto;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var result = String.Empty;

            var movies = context.Movies
                .ToList()
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Any()))
                .OrderByDescending(m => m.Rating)
                .ThenByDescending(m => m.Projections
                    .Sum(p =>
                        p.Tickets
                            .Sum(t => t.Price))
                    )
                .Take(10)
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("F2"),
                    TotalIncomes = m.Projections
                        .Sum(p =>
                            p.Tickets
                                .Sum(t => t.Price)).
                        ToString("F2"),
                    Customers = m.Projections.Select(p => p.Tickets
                        .Select(t => new
                        {
                            FirstName = t.Customer.FirstName,
                            LastName = t.Customer.LastName,
                            Balance = t.Customer.Balance
                        })
                        .OrderByDescending(c => c.Balance)
                        .ThenBy(c => c.FirstName)
                        .ThenBy(c => c.LastName))
                        .ToArray()
                })
                .ToArray();

            
            result = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var result = new StringBuilder();

            var customers = context.Customers
                .ToList()
                .Where(c => c.Age >= age)
                .Select(c => new
                {
                    c.FirstName,
                    c.LastName,
                    SpentMoney = c.Tickets.Sum(t => t.Price),
                    SpentTime = c.Tickets.Select(t => t.Projection.Movie.Duration).ToList()
                })
                .OrderByDescending(c => c.SpentMoney)
                .Take(10)
                .ToList();

            var exportCustomersDtOs = new List<ExportCustomerDTO>();

            foreach (var customer in customers)
            {
                var timeSpent = new TimeSpan();

                foreach (var duration in customer.SpentTime)
                {
                    timeSpent = timeSpent.Add(duration);
                }

                var customerDto = new ExportCustomerDTO()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    SpentMoney = customer.SpentMoney.ToString("F2"),
                    SpentTime = timeSpent.ToString(@"hh\:mm\:ss")
                };    

                exportCustomersDtOs.Add(customerDto);
            }

            using (TextWriter tWriter = new StringWriter(result))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<ExportCustomerDTO>), new XmlRootAttribute("Customers"));
                xmlSerializer.Serialize(tWriter, exportCustomersDtOs);
            }


            return result.ToString().TrimEnd();
        }
    }
}
