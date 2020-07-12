namespace ADO.Net___Exercises
{
    using System;
    using Microsoft.Data.SqlClient;

    public class CreateDatabase
    {
        public static void Main()
        {
            string command;
            using var myConn = new SqlConnection("Server=.\\SQLEXPRESS;Integrated security=true;Database=master");

            command = "CREATE DATABASE MinionsDB";

            using SqlCommand myCommand = new SqlCommand(command, myConn);
            myConn.Open();
            var numberOfRows = myCommand.ExecuteNonQuery();

            if (numberOfRows != 0)
            {
                Console.WriteLine("DataBase is Created Successfully");
            }

            using SqlConnection sqlConnection = new SqlConnection(@"Server=.\SQLEXPRESS;Integrated security=true;Database=MinionsDB");
                    sqlConnection.Open();

                    var createTableCountries = "CREATE TABLE Countries(" +
                                               "Id int PRIMARY KEY IDENTITY," +
                                               "[Name] nvarchar(30) NOT NULL)";

            using SqlCommand sqlCommand = new SqlCommand(createTableCountries, sqlConnection);

            var isSuccessful = sqlCommand.ExecuteNonQuery();

            var createTownsString = "CREATE TABLE Towns(" +
                                    "Id int PRIMARY KEY IDENTITY," +
                                    "[Name] nvarchar(30) NOT NULL," +
                                    "CountryCode int REFERENCES Countries(Id) NOT NULL)";

            sqlCommand.CommandText = createTownsString;
            sqlCommand.ExecuteNonQuery();

            var createEvilnessFactors = "CREATE TABLE EvilnessFactors(" +
                                               "Id int PRIMARY KEY IDENTITY," +
                                               "[Name] nvarchar(30) NOT NULL)";

            sqlCommand.CommandText = createEvilnessFactors;
            sqlCommand.ExecuteNonQuery();

            var createVillainsString = "CREATE TABLE Villains(" +
                                       "Id int PRIMARY KEY IDENTITY," +
                                       "[Name] nvarchar(30) NOT NULL," +
                                       "EvilnessFactorId int REFERENCES EvilnessFactors(Id))";

            sqlCommand.CommandText = createVillainsString;
            sqlCommand.ExecuteNonQuery(); 
            var createMinionsString = "CREATE TABLE Minions(" +
                                      "Id int PRIMARY KEY IDENTITY," +
                                      "[Name] nvarchar(30) NOT NULL," +
                                      "Age int NOT NULL," +
                                      "TownId int REFERENCES Towns(Id) NOT NULL)";

            sqlCommand.CommandText = createMinionsString;
            sqlCommand.ExecuteNonQuery();

            var createMinionsVillainsMappingTableString = "CREATE TABLE MinionsVillains(" +
                                                          "MinionId int REFERENCES Minions(Id)," +
                                                          "VillainId int REFERENCES Villains(Id)" +
                                                          "PRIMARY KEY(MinionId, VillainId))";

            sqlCommand.CommandText = createMinionsVillainsMappingTableString;
            sqlCommand.ExecuteNonQuery();
        }
    }
}
