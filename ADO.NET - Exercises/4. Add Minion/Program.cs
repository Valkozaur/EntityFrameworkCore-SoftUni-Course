namespace _4._Add_Minion
{
    using System;
    using System.Linq;
    using Microsoft.Data.SqlClient;

    public class Program
    {
        public static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(@"Server=.\SQLEXPRESS;Integrated Security=true;Database=MinionsDB");
            sqlConnection.Open();

            var minionInfo = Console.ReadLine()
                .Split(" ")
                .ToList();

            var name = minionInfo[1];
            var age = minionInfo[2];
            var town = minionInfo[3];

            var villainInfo = Console.ReadLine()
                .Split();
            var villainName = villainInfo[1];

            bool shouldBeATransaction = EnsureTownIsInDb(sqlConnection, town);

            shouldBeATransaction = EnsureVillainIsInDb(sqlConnection, villainName);

            shouldBeATransaction =  AddMinions(sqlConnection, name, age, town, villainName);
        }

        private static bool AddMinions(SqlConnection sqlConnection, string name, string age, string town, string villainName)
        {


            var getTownId = "SELECT Id FROM Towns WHERE Name = @town";

            using SqlCommand sqlCommand = new SqlCommand(getTownId, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@town", town);

            var townId = sqlCommand.ExecuteScalar();

            var addMinionToDbQuery = "INSERT Minions(Name, Age, TownId) VALUES (@name, @age, @townId)";
            sqlCommand.CommandText = addMinionToDbQuery;

            sqlCommand.Parameters.AddWithValue("@name", name);
            sqlCommand.Parameters.AddWithValue("@age", age);
            sqlCommand.Parameters.AddWithValue("@townId", townId);

            if (sqlCommand.ExecuteNonQuery() == 0)
            {
                return false;
            }

            var takeMinionId = "SELECT Id FROM Minions WHERE Name = @name AND Age = @age AND TownId = @townId";

            sqlCommand.CommandText = takeMinionId;
            var minionId = sqlCommand.ExecuteScalar();

            var takeVillainId = "SELECT Id FROM Villains WHERE Name = @villainName";

            sqlCommand.CommandText = takeVillainId;
            sqlCommand.Parameters.AddWithValue("@villainName", villainName);
            var villainId = sqlCommand.ExecuteScalar();

            var addConnectionToMappingTable = "INSERT MinionsVillains(MinionId, VillainId) VALUES (@minionId, @villainId)";
            sqlCommand.CommandText = addConnectionToMappingTable;
            sqlCommand.Parameters.AddWithValue("@minionId", minionId);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);

            if (sqlCommand.ExecuteNonQuery() != 0)
            {
                Console.WriteLine($"Successfully added {name} to be minion of {villainName}.");
                return true;
            }

            return false;
        }

        private static bool EnsureVillainIsInDb(SqlConnection sqlConnection, string villainName)
        {
            var searchForVillainInDbQuery = $"SELECT Name From Villains WHERE Name = @villainName";
            
            using SqlCommand villainCommand = new SqlCommand(searchForVillainInDbQuery, sqlConnection);

            villainCommand.Parameters.AddWithValue("@villainName", villainName);
            var villainFromDb = villainCommand.ExecuteScalar()?.ToString();

            if (villainFromDb == villainName)
            {
                return true;
            }

            var addVillainToDbQuery = "INSERT Villains (Name, EvilnessFactorId) VALUES (@villainName, 4)";
            villainCommand.CommandText = addVillainToDbQuery;

            if (villainCommand.ExecuteNonQuery() > 0)
            {
                Console.WriteLine($"Villain {villainName} was added to the database.");
                return true;
            }

            return false;
        }

        private static bool EnsureTownIsInDb(SqlConnection sqlConnection, string town)
        {
            var searchTownNameInDatabaseQuery = $"SELECT Name FROM Towns WHERE Name = @town";
            using SqlCommand townCommand = new SqlCommand(searchTownNameInDatabaseQuery, sqlConnection);
            townCommand.Parameters.AddWithValue("@town", town);

            var townName = townCommand.ExecuteScalar()?.ToString();

            if (townName == town)
            {
                return true;
            }

            var addTownToDb = $"INSERT Towns(Name, CountryCode) VALUES(@town, 1)";
            townCommand.CommandText = addTownToDb;

            if (townCommand.ExecuteNonQuery() != 0)
            {
                Console.WriteLine($"Town {town} was added to the database.");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
