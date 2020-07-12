using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace _6._Remove_Villain
{
    public class Program
    {
        public static void Main()
        {
            var villainId = int.Parse(Console.ReadLine());

            using var sqlConnection = new SqlConnection(@"Server=.\SQLEXPRESS;Integrated security=true;Database=MinionsDB");
            sqlConnection.Open();

            using var transaction = sqlConnection.BeginTransaction();

            var villainExists = CheckIfVillainExists(sqlConnection, villainId, transaction);

            if (!villainExists)
            {
                transaction.Rollback();
            }

            var affectedRows = ReleaseMinions(sqlConnection, villainId, transaction);

            if (affectedRows < 0)
            {
                transaction.Rollback();
            }

            var isDeleted = DeleteVillain(sqlConnection, villainId, transaction);

            if (!isDeleted)
            {
                transaction.Rollback();
            }
            
            transaction.Commit();
        }

        private static bool DeleteVillain(SqlConnection sqlConnection, int villainId, SqlTransaction transaction)
        {
            var deleteVillainQuery = "DELETE Villains WHERE Id = @villainId";

            using var sqlCommand = new SqlCommand(deleteVillainQuery, sqlConnection, transaction);

            sqlCommand.Parameters.AddWithValue("@villainId", villainId);

            var affectedRows = sqlCommand.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                return true;
            }

            return false;
        }

        private static bool CheckIfVillainExists(SqlConnection sqlConnection, int villainId, SqlTransaction transaction)
        {
            var villainExists = "SELECT Id FROM Villains WHERE Id = @villainId";

            using SqlCommand sqlCommand = new SqlCommand(villainExists, sqlConnection, transaction);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);

            var idOutOfDb = (int)sqlCommand.ExecuteScalar();

            if (villainId == idOutOfDb)
            {
                return true;
            }

            return false;
        }

        private static int ReleaseMinions(SqlConnection sqlConnection, int villainId, SqlTransaction transaction)
        {
            var releaseMinionsQuery = "DELETE MinionsVillains WHERE VillainId = @villainId";
            
            using var sqlCommand = new SqlCommand(releaseMinionsQuery, sqlConnection, transaction);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);

            var affectedRows = sqlCommand.ExecuteNonQuery();

            return affectedRows;
        }
    }
}
