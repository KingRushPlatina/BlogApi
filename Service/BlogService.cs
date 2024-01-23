using BlogApi.Models;
using BlogApi.Service.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BlogApi.Service
{
    public class BlogService : IBlogService
    {
        private readonly IConfiguration _configuration;
        private const string dbName = "Post";
        private readonly ILogger<BlogService> _logger;
        private readonly string conn;

        public BlogService(IConfiguration configuration, ILogger<BlogService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            conn = string.Format(_configuration.GetConnectionString("DefaultConnection"), dbName);
        }

        public async Task AddPost(Post value)
        {
            const string spName = "sp_post_insert";

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@title", value.Title);
                        command.Parameters.AddWithValue("@body", value.Body);
                        command.Parameters.AddWithValue("@authorid", value.Autor.Id);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            const string spName = "sp_post_get";

            try
            {
                Post post = null;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Post_id", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                post = new Post
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Title = reader.GetString(reader.GetOrdinal("Titolo")),
                                    Body = reader.GetString(reader.GetOrdinal("Contenuto")),
                                    PublishDate = reader.GetDateTime(reader.GetOrdinal("DataPubblicazione")).ToLongDateString(),
                                    Autor = new Autor { Id = reader.GetInt32(reader.GetOrdinal("AuthorID")) },
                                    Comments = await GetComments(id)
                                };
                            }
                        }
                    }
                }

                return post;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }


        public async Task<List<Post>> GetPosts(string title, int pageNumber = 1, int pageSize = 4)
        {
            const string spName = "sp_posts_get";

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PageNumber", pageNumber);
                        command.Parameters.AddWithValue("@PageSize", pageSize);
                        command.Parameters.AddWithValue("@Title", title);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Post> posts = new List<Post>();

                            while (await reader.ReadAsync())
                            {
                                Post post = new Post
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Title = reader.GetString(reader.GetOrdinal("Titolo")),
                                    Body = reader.GetString(reader.GetOrdinal("Contenuto")),
                                    PublishDate = reader.GetDateTime(reader.GetOrdinal("DataPubblicazione")).ToLongDateString(),
                                    Autor = new Autor { Id = reader.GetInt32(reader.GetOrdinal("AuthorID")) },
                                };

                                posts.Add(post);
                            }

                            return posts;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        private async Task<List<Comment>> GetComments(int postId)
        {
            const string spName = "sp_comments_get";

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Post_id", postId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Comment> comments = new List<Comment>();

                            while (await reader.ReadAsync())
                            {
                                Comment comment = new Comment
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Text = reader.GetString(reader.GetOrdinal("Testo")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("DataCommento")).ToLongDateString(),

                                };
                                comment.Commentator = new Autor();
                                comment.Commentator.Id = reader.GetInt32(reader.GetOrdinal("IDAuthor"));

                                comments.Add(comment);
                            }

                            return comments;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        private void LogError(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the BlogService");
        }
    }
}
