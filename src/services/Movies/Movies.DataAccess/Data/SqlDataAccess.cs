using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Movies.DataAccess.Data;

namespace Movies.DataAccess.Data
{
    public class SqlDataAccess
    {
        private readonly DapperContext _context;

        public SqlDataAccess(DapperContext context)
        {
            _context = context;
        }
            
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (var connexion = _context.CreateDatabaseConnection())
            {
                //List<T> list = connexion.Query<T>(sql, parameters).ToList();
                var list = await connexion.QueryAsync<T>(sql, parameters);
                return list.ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            using (var connexion = _context.CreateDatabaseConnection())
            {
                await connexion.ExecuteAsync(sql, parameters);
            }
        }
    }
}
