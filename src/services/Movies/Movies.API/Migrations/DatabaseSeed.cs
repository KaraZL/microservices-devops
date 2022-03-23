using Dapper;
using Movies.DataAccess.Data;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Movies.API.Migrations
{
    public class DatabaseSeed
    {
        private readonly DapperContext _context;

        public DatabaseSeed(DapperContext context)
        {
            _context = context;
        }

        public void CreateDatabase()
        {
            using (IDbConnection connection = _context.CreateServerConnection())
            {
                var sql = "SELECT * FROM sys.databases WHERE name = 'MoviesDb'";
                var check = connection.Query(sql);
                if (!check.Any())
                {
                    sql = "CREATE DATABASE MoviesDb";
                    connection.Execute(sql);
                }
            }
        }

    }
}
