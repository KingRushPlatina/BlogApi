using System;
using System.Data;
using System.Threading.Tasks;
using BlogApi.Service.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BlogApi.Models;
using Microsoft.Extensions.Logging;

public class AuthorService : IAuthorService
{
    private readonly IConfiguration _configuration;
    private const string dbName = "authors";
    private readonly string conn;

    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IConfiguration configuration, ILogger<AuthorService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        conn = string.Format(_configuration.GetConnectionString("DefaultConnection"), dbName);

    }

    public async Task<Autor?> GetPostById(int id)
    {

        const string sp_name = "sp_author_get";

        try
        {
            Autor? autor = null;
            using (SqlConnection connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(sp_name, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@author_id", id));

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
    #region private method
    private void LogError(Exception ex)
    {
        _logger.LogError(ex, "An error occurred in the AuthorService");
    }
    #endregion
}
