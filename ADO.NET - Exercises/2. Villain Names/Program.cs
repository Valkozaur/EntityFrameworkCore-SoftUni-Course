using System;
using Microsoft.Data.SqlClient;

namespace _2._Villain_Names
{
    public class Program
    {
        public static void Main()
        {
            using var sqlConnection = new SqlConnection("Server=.\\SQLEXPRESS;Integrated security=true;Database=MinionsDB");
            sqlConnection.Open();

            var command =
                @"SELECT Name, COUNT(*) AS MinionsCount 
                  FROM MinionsVillains mv 
                  INNER JOIN Villains v ON mv.VillainId = v.Id
                  GROUP BY Name HAVING COUNT(*) > 3";

            using SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using var villainsInfo = sqlCommand.ExecuteReader();
            while(villainsInfo.Read())
            {
                var villainName = villainsInfo["Name"];
                var villainMinionCount = villainsInfo["MinionsCount"];

                Console.WriteLine($"{villainName} - {villainMinionCount}");
            }
        }
    }
}
