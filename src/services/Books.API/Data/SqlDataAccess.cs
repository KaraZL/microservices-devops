using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Books.API.Data
{
    public class SqlDataAccess
    {
        private readonly DapperContext _context;

        public SqlDataAccess(DapperContext context)
        {
            _context = context;
        }

        //T return type
        public List<T> LoadData<T, U>(string sql, U parameters)
        {
            //SqlConnection : System.Data.SqlClient - permet la connexion
            //IDbConnection : System.Data 

            using (var connection = _context.CreateConnection())
            {
                //Query : Dapper
                //ToList : Linq
                List<T> rows = connection.Query<T>(sql, parameters).ToList();
                return rows;
            } 
        }

        public void SaveData<T>(string sql, T parameters)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Execute(sql, parameters);
            }
        }
    }
}
