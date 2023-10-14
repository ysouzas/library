using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Api.Model;

namespace Library.Api.Services;

public interface IBookService
{
    public Task<bool> CreateAsync(Book book);

    public Task<Book?> GetByISBNAsync(string isbn);

    public Task<IEnumerable<Book>> GetAllAsync();

    public Task<IEnumerable<Book>> SearchByTitleAsync(string title);

    public Task<bool> UpdateAsync(Book book);

    public Task<bool> DeleteAsync(string isbn);
}
