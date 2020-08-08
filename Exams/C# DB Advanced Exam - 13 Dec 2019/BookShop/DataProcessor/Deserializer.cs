using System.Reflection.Metadata.Ecma335;
using BookShop.Data.Models;
using BookShop.Data.Models.Enums;
using BookShop.DataProcessor.ImportDto;
using BookShop.XmlFacade;
using Microsoft.EntityFrameworkCore.Query.ResultOperators.Internal;

namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        private static StringBuilder sb;

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            sb = new StringBuilder();

            var bookDtos = XmlConverter.Deserializer<ImportBookDto>(xmlString, "Books");

            var books = new List<Book>();
            foreach (var bookDto in bookDtos)
            {
                DateTime publishedON;
                var dateIsValid = 
                    DateTime.TryParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedON);

                if (!IsValid(bookDto) || !dateIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var book = new Book()
                {
                    Name = bookDto.Name,
                    Genre = (Genre)bookDto.Genre,
                    Pages = bookDto.Pages,
                    Price = bookDto.Price,
                    PublishedOn = publishedON
                };

                books.Add(book);
                sb.AppendFormat(SuccessfullyImportedBook, book.Name, book.Price);
                sb.AppendLine();
            }

            context.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            sb = new StringBuilder();
            var authorDtos = JsonConvert.DeserializeObject<ImportAuthorDto[]>(jsonString);

            var authors = new List<Author>();
            foreach (var authorDto in authorDtos)
            {
                if (!IsValid(authorDto))    
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (authors.Any(x => x.Email == authorDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author()
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Phone = authorDto.Phone,
                    Email = authorDto.Email
                };

                var authorBooks = new List<AuthorBook>();
                foreach (var bookDto in authorDto.Books)
                {
                    if (context.Books.Find(bookDto.Id) != null)
                    {
                        var authorBook = new AuthorBook()
                        {
                            BookId = bookDto.Id,
                            AuthorId = author.Id
                        };

                        author.AuthorsBooks.Add(authorBook);
                    }
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(author);
                sb.AppendFormat(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName, author.AuthorsBooks.Count);
                sb.AppendLine();
            }

            context.Authors.AddRange(authors);
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