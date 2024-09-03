using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace ChatLab
{
    public class DataBase
    {
        static string connectionString = "Host=localhost;Port=yourPort;Database=yourDatabaseName;Username=yourUsername;Password=yourPassword;";
        NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);

        public void DBConnection()
        {
            try
            {
                npgSqlConnection.Open();
                Console.WriteLine("Succesful connection!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Безвозвратная команда
        public void DBInputCommand(string command)
        {
            DBConnection();
            NpgsqlCommand  commandSQL = new NpgsqlCommand(command, npgSqlConnection);
            commandSQL.ExecuteNonQuery();
            npgSqlConnection.Close();
        }

        public void DBClose() { npgSqlConnection.Close(); }

        // Получение пользовательских данных
        public async Task<List<List<string>>> DBOutputUsersCommand()
        {
            DBConnection();
            List<List<string>> users = new List<List<string>>();
            // Создаем вложенные списки (подсписки)
            List<string> user = new List<string> { "AdminName"};
            List<string> password = new List<string> { "AdminPassword"};
            users.Add(user);
            users.Add(password);
            NpgsqlCommand commandSQL = new NpgsqlCommand("SELECT * FROM users;", npgSqlConnection);
            try
            {
                await commandSQL.ExecuteNonQueryAsync();
                var reader = await commandSQL.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users[0].Add(reader.GetString(0));
                    users[1].Add(reader.GetString(1));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}") ;
            }
            npgSqlConnection.Close();
            return users;
        }

        // Получение данных о сообщениях
        public async Task<List<List<string>>> DBOutputMessagesCommand()
        {
            DBConnection();
            List<List<string>> messages = new List<List<string>>();
            List<string> name = new List<string> { "AdminName" };
            List<string> message = new List<string> { "AdminMessage" };
            List<string> date = new List<string> { "AdminDate" };
            List<string> id = new List<string> { "AdminId" };
            messages.Add(name);
            messages.Add(message);
            messages.Add(date);
            messages.Add(id);
            NpgsqlCommand commandSQL = new NpgsqlCommand("SELECT * FROM messages;", npgSqlConnection);
            await commandSQL.ExecuteNonQueryAsync();
            try
            {
                var reader = await commandSQL.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    messages[0].Add(reader.GetString(0));
                    messages[1].Add(reader.GetString(1));
                    messages[2].Add(reader.GetString(2));
                    messages[3].Add(reader.GetInt32(3).ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            npgSqlConnection.Close();
            return messages;
        }

        // Получение ID сообщений
        public async Task<List<List<string>>> DBOutputMessagesID()
        {
            DBConnection();
            List<List<string>> messages = new List<List<string>>();
            List<string> id = new List<string> { "AdminId" };
            messages.Add(id);
            NpgsqlCommand commandSQL = new NpgsqlCommand("SELECT * FROM messageID;", npgSqlConnection);
            await commandSQL.ExecuteNonQueryAsync();
            try
            {
                var reader = await commandSQL.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    messages[0].Add(reader.GetInt32(0).ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            npgSqlConnection.Close();
            return messages;
        }
    }
}
