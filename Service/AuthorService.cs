using System;
using System.Data;
using System.Globalization;
using System.Threading;
using BlogApi.Service.Interface;
using Microsoft.Data.SqlClient;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

public class AuthorService : IAuthorService
{
    const string serverName = "DESKTOP-EIE5MAT\\SQLEXPRESS";
    const string dbName = "Post";
    const string _connectionString = "Data Source=DESKTOP-EIE5MAT\\SQLEXPRESS;Initial Catalog=Post;Integrated Security=True;Encrypt=true;TrustServerCertificate=true;";

    public void Conn()
    {
        try
        {
            // Imposta la cultura corrente su invariante
            CultureInfo culture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // La connessione è aperta, puoi eseguire operazioni sul database qui

                Console.WriteLine("Connessione al database riuscita!");

                // Importante: Chiudi la connessione quando hai finito di utilizzarla
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la connessione al database: {ex.Message}");
        }
    }

    public string GetPostById(int postId)
    {
        string s = "";
        try
        {
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Sp_Post_get", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Aggiungi i parametri necessari per la stored procedure
                    command.Parameters.Add(new SqlParameter("@Post_id", postId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Leggi i dati restituiti dalla stored procedure e fai qualcosa con essi
                            int id = reader.GetInt32(reader.GetOrdinal("ID"));
                            string title = reader.GetString(reader.GetOrdinal("Titolo"));
                             s = $"ID: {id}, Titolo: {title}";
                          
                        }
                    }
                }
                connection.Close();
               
            }
            return s;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
