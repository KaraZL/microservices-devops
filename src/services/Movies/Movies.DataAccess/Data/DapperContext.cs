
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateDatabaseConnection() => new SqlConnection(_configuration.GetConnectionString("SqlDatabase"));

        public IDbConnection CreateServerConnection() => new SqlConnection(_configuration.GetConnectionString("SqlServer"));
    }
}
