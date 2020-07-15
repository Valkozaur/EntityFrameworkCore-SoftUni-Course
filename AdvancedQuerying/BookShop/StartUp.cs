using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookShop.Data;
using System.Globalization;
using BookShop.Models.Enums;

namespace BookShop
{
    public class StartUp
    {
        private static StringBuilder sb = new StringBuilder();

        public static void Main()
        {
            var context = new BookShopContext();
            Console.WriteLine(GetTotalProfitByCategory(context));
        }

        //2
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (Enum.TryParse(typeof(AgeRestriction), command, true, out var result))
            {
                var books = context.Books
                    .Where(x => x.AgeRestriction == (AgeRestriction)result)
                    .Select(b => b.Title)
                    .OrderBy(x => x)
                    .ToList();

                return String.Join(Environment.NewLine, books);
            }

            return "";
        }

        //3
        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookTitles = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return String.Join(Environment.NewLine, bookTitles);
        }

        //4
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new
                {
                    x.Title,
                    x.Price
                })
                .OrderByDescending(x => x.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        //5
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        //6 Might be that books have to meet all the categories;
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(" ", 
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToList();

            var bookTitles = new List<string>();

            foreach (var category in categories)
            {
                var currentBooks = context.Books
                    .Where(b => b.BookCategories
                        .Any(bc => 
                            bc.Category.Name.ToLower() == category))
                    .Select(x => x.Title)
                    .ToList();

                bookTitles.AddRange(currentBooks);
            }

            bookTitles.OrderBy(x => x);

            return String.Join(Environment.NewLine, bookTitles);
        }

        //7
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate.Value < dateTime)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price
                });

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //8
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => x.FirstName + " " + x.LastName)
                .OrderBy(x => x);

            foreach (var name in authors)
            {
                sb.AppendLine(name);
            }

            return sb.ToString().TrimEnd();
        }

        //9
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title
                    .Contains(input, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        //10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Author.LastName.StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    x.Title,
                    x.Author.FirstName,
                    x.Author.LastName
                });

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName + " " ?? ""}{book.LastName})");
            }

            return sb.ToString().TrimEnd();
        }

        //11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .Count();
        }

        //12
        public static string CountCopiesByAuthor(BookShopContext context)  
        {
            var authorsAndCopies = context.Authors
                .Select(a => new
                {
                    AuthorName = a.FirstName + " " + a.LastName,
                    BookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(x => x.BookCopies)
                .ToList();

            foreach (var authorInfo in authorsAndCopies)
            {
                Console.WriteLine($"{authorInfo.AuthorName} - {authorInfo.BookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        //13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesProfit = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks
                        .Sum(bc
                            => bc.Book.Copies * bc.Book.Price)
                })
                .OrderByDescending(x => x.TotalProfit)
                .ToList();

            foreach (var category in categoriesProfit)
            {
                sb.AppendLine($"{category.Name} ${category.TotalProfit:F2}");
            }

            return sb.ToString().TrimEnd().Length.ToString();
        }

        //14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoriesBooks = context.Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .Select(bc => new
                        {
                            bc.Book.Title,
                            bc.Book.ReleaseDate
                        })
                        .OrderByDescending(x => x.ReleaseDate)
                        .Take(3)
                        .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList();

            foreach (var c in categoriesBooks)
            {
                sb.AppendLine($"--{c.Name}");

                foreach (var b in c.MostRecentBooks)
                {
                    sb.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //15
        public static void IncreasePrices(BookShopContext context)
        {
            var bookToUpdate = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in bookToUpdate)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //16
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToDelete = context.Books
                .Where(b => b.Copies < 4200);

            var count = 0;
            foreach (var book in booksToDelete)
            {
                context.Books.Remove(book);
                count++;
            }

            context.SaveChanges();

            return count;
        }
    }
}
