using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Books.API.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //On doit utiliser cette connexion car la base de données n'existe pas encore. le serveur oui mais pas la bdd
        public IDbConnection CreateDatabaseConnection() => new SqlConnection(_configuration.GetConnectionString("SqlServer"));
        //Une fois créé, on utilise ça
        public IDbConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("SqlDatabase"));
    }
}
