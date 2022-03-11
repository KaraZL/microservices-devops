using Books.API.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Repository
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(string id);
        Task CreateBook(Book book);
        Task DeleteBook(Book book);
    }
}
