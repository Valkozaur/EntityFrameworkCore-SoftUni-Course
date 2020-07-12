using System;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace _3._Minions_Names
{
    public class StartUp
    {
        public static void Main()
        {
            var villainId = int.Parse(Console.ReadLine());

            using var sqlConnection = new SqlConnection(@"Server=.\SQLEXPRESS;Integrated security=true;Database=MinionsDB");
            sqlConnection.Open();

            var name = GetVillainName(villainId, sqlConnection);

            Console.WriteLine(name);
            if (name.StartsWith("No villain"))
            {
                return;
            }

            Console.WriteLine(GetMinionsNames(villainId, sqlConnection));
        }

        public static string GetVillainName(int villainId, SqlConnection sqlConnection)
        {

            var getVillainSqlCommand = @"SELECT Name FROM Villains 
                                         WHERE Id = @villainId";

            using SqlCommand sqlCommand = new SqlCommand(getVillainSqlCommand, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);

            var villainName = sqlCommand
                .ExecuteScalar()?
                .ToString();

            if (villainName != null)
            {
                return $"Villain: " + villainName;
            }
            else
            {
                return $"No villain with ID {villainId} exists in the database";
            }
        }

        public static string GetMinionsNames(int villainId, SqlConnection sqlConnection)
        {
            var minionsNames = new List<string>();

            var getMinionsNamesSqlCommand = @"SELECT m.Name, m.Age FROM MinionsVillains AS mv
                                              INNER JOIN Minions AS m ON MinionId = Id 
                                              WHERE VillainId = @villainId";

            using SqlCommand sqlCommand = new SqlCommand(getMinionsNamesSqlCommand, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);

            using var reader = sqlCommand.ExecuteReader();

            var sb = new StringBuilder();

            if (reader.HasRows)
            {

                var rowNum = 1;

                while (reader.Read())
                {
                    var minionName = reader["Name"]
                        ?.ToString();
                    var minionAge = reader["Age"]
                        ?.ToString();

                    sb.AppendLine($"{rowNum}. {minionName} {minionAge}");
                    rowNum++;
                }

            }
            else
            {
                Console.WriteLine("(no minions)");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
