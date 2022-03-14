using Books.API.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Repository
{
    public class SqlBooksRepository : IBooksRepository
    {
        private readonly IConfiguration _configuration;

        public SqlBooksRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task CreateBook(Book book)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteBook(Book book)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAllBooks()
        {
            throw new System.NotImplementedException();
        }

        public Task<Book> GetBookById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
