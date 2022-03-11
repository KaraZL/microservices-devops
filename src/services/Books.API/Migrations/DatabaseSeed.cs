using Books.API.Data;
using Dapper;
using System.Data;
using System.Linq;

namespace Books.API.Migrations
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
            //Requete pour vérifier si une base de données existe à ce nom
            var query = "SELECT * FROM sys.databases WHERE name = 'BooksDb';";

            //On doit utiliser cette connexion car la base de données n'existe pas encore. le serveur oui mais pas la bdd
            using (IDbConnection connection = _context.CreateDatabaseConnection())
            {
                var records = connection.Query(query, new { }); //Si il n'y a aucune BDD, on la créé
                if (!records.Any())
                {
                    var exec = "CREATE DATABASE BooksDb;";
                    connection.Execute(exec, new { });
                }
            }
        }

        public void StoredProcedures()
        {

            using (IDbConnection connection = _context.CreateConnection())
            {
                //get all books
                var query = @"CREATE PROCEDURE spBooks_GetAll
                          AS
                          BEGIN
                            SELECT * FROM dbo.Books
                          END";

                connection.Execute(query, new { });

                query = @"CREATE PROCEDURE spBooks_GetTitles
                            AS
                            BEGIN
                                SELECT Title FROM dbo.Books
                            END";

                connection.Execute(query, new { });

                query = @"CREATE PROCEDURE spBooks_GetById
                                @bookId varchar(5)
                            AS
                            BEGIN
                                SELECT Title FROM dbo.Books WHERE id = @bookId
                            END";

                connection.Execute(query, new { });
            }


        }
    }
}
