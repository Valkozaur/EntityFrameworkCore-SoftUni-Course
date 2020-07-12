using System;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace _5._Change_Town_Names_Casing
{
    public class Program
    {
        public static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(@"Server=.\SQLEXPRESS;Integrated security=true;Database=MinionsDB");
            sqlConnection.Open();
            var countryName = Console.ReadLine();

            var result = ChangeCityNamesCasingToUpper(sqlConnection, countryName);

            Console.WriteLine(result);
        }

        private static string ChangeCityNamesCasingToUpper(SqlConnection sqlConnection, string countryName)
        {
            var getCountryWithTowns = "SELECT * FROM Countries c INNER JOIN Towns t ON c.Id = t.CountryCode WHERE c.Name = @countryName";

            using SqlCommand checkIfCountryExistsCommand = new SqlCommand(getCountryWithTowns, sqlConnection);
            checkIfCountryExistsCommand.Parameters.AddWithValue("@countryName", countryName);

            using var cityReader = checkIfCountryExistsCommand.ExecuteReader();

            if (!cityReader.HasRows)
            {
                return "No town names were affected.";
            }

            var citiesNamesToUpper = "UPDATE t SET t.Name = UPPER(t.Name) FROM Towns t INNER JOIN Countries c ON t.CountryCode = c.Id WHERE c.Name = @countryName";

            using SqlCommand  changeCitiesNamesToUpper = new SqlCommand(citiesNamesToUpper, sqlConnection);
            changeCitiesNamesToUpper.Parameters.AddWithValue("@countryName", countryName);

            cityReader.Close();
            var affectedRows = changeCitiesNamesToUpper.ExecuteNonQuery();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{affectedRows} town names were affected.");

            var getCitiesNames = "SELECT t.Name FROM Countries c INNER JOIN Towns t ON c.Id = t.CountryCode WHERE c.Name = @countryName";

            using SqlCommand getChangedCityNames = new SqlCommand(getCitiesNames, sqlConnection);
            getChangedCityNames.Parameters.AddWithValue("@countryName", countryName);


            getChangedCityNames.CommandText = getCitiesNames;
            using var changedNamesReader = getChangedCityNames.ExecuteReader();

            var cityNames = new List<string>();
            while (changedNamesReader.Read())
            {
                cityNames.Add(changedNamesReader.GetString(0));
            }

            sb.AppendLine($"[{string.Join(", ", cityNames)}]");

            return sb.ToString().TrimEnd();
        }
    }
}
