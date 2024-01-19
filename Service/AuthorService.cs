using System;
using System.Data;
using System.Globalization;
using System.Threading;
using BlogApi.Service.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Xml;
using BlogApi.Models;

public class AuthorService : IAuthorService
{
    private readonly IConfiguration _configuration;
    private const string dbName= "authors";
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IConfiguration configuration, ILogger<AuthorService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Autor GetPostById(int id)
    {
        const string sp_name = "sp_author_get";

        try
        {
            Autor autor = null;
            string conn = string.Format(_configuration.GetConnectionString("DefaultConnection"), dbName);
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sp_name, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@author_id", id));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            autor = new Autor();
                            autor.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            autor.Name = reader.GetString(reader.GetOrdinal("NAME"));
                            autor.Surname = reader.GetString(reader.GetOrdinal("SURNAME"));
                            autor.Mail = reader.GetString(reader.GetOrdinal("MAIL"));
                        }
                    }
                }
                connection.Close();
               
            }
            return autor;
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    private void LogError(Exception ex)
    {
        _logger.LogError(ex, "An error occurred in the AuthorService");
    }
}
