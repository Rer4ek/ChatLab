using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ChatLab
{
    public class DataBase
    {
        static string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345;";
        NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);

        public async void DBConnection()
        {
            npgSqlConnection.Open();
            Console.WriteLine("Succesful connection!");
        }

        public async void DBInputCommand(string command)
        {
            NpgsqlCommand commandSQL = new NpgsqlCommand(command, npgSqlConnection);
            await npgSqlConnection.OpenAsync();
            await commandSQL.ExecuteNonQueryAsync();
        }

        public async void DBOutputUsersCommand()
        {
            List<List<string>> users = new List<List<string>>();
            NpgsqlCommand commandSQL = new NpgsqlCommand("SELECT * FROM users", npgSqlConnection);
            //await npgSqlConnection.OpenAsync();
            await commandSQL.ExecuteNonQueryAsync();
            try
            {
                var reader = await commandSQL.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users[0].Add(reader.GetString(0));
                    users[1].Add(reader.GetInt32(1).ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            
        }
    }
}
