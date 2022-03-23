using Movies.DataAccess.Data;
using Movies.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess.Repository
{
    public class MovieRepo : IMovieRepo
    {
        private readonly SqlDataAccess _access;

        public MovieRepo(SqlDataAccess access)
        {
            _access = access;
        }

        public async Task Add(Movie movie)
        {
            var sql = "INSERT INTO movies(name, author, year) VALUES(@Name, @Author, @Year)";
            await _access.SaveData(sql, new { Name = movie.Name, Author = movie.Author, Year = movie.Year }); //@Name, @Author, @Year
        }

        public async Task Delete(int id)
        {
            var sql = "DELETE FROM movies WHERE id = @id";
            await _access.SaveData(sql, new { Id = id });
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            var sql = "SELECT * FROM movies";
            var data = await _access.LoadData<Movie, dynamic>(sql, new {});
            return data;
        }

        public async Task<Movie> GetById(int id)
        {
            var sql = "SELECT * FROM movies WHERE id = @Id";
            var data = await _access.LoadData<Movie, dynamic>(sql, new { Id = id });
            return data.FirstOrDefault();
        }
    }
}
