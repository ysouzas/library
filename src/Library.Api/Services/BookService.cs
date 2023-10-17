using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Library.Api.Data;
using Library.Api.Model;

namespace Library.Api.Services;

public class BookService : IBookService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BookService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(Book book)
    {
        var existingBook = await GetByISBNAsync(book.ISBN);

        if (existingBook is not null)
        {
            return false;
        }

        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(@"INSERT INTO Books (ISBN, Title, Author, ShortDescription, PageCount, ReleaseDate) 
                                        VALUES(@ISBN, @Title, @Author, @ShortDescription, @PageCount, @ReleaseDate)", book);

        return result > 0;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.QueryAsync<Book>("SELECT * FROM Books");

        return result;
    }

    public async Task<Book?> GetByISBNAsync(string isbn)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.QuerySingleOrDefaultAsync<Book>("SELECT * FROM Books WHERE ISBN = @Isbn LIMIT 1", new { Isbn = isbn });

        return result;
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.QueryAsync<Book>("SELECT * FROM Books WHERE Title LIKE '%'|| @Title|| '%'", new { Title = title });

        return result;
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        var existingBook = await GetByISBNAsync(book.ISBN);

        if (existingBook is null)
        {
            return false;
        }

        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(@"UPDATE Books SET
                                                            ISBN = @ISBN,
                                                            Title = @Title,
                                                            Author = @Author,
                                                            ShortDescription = @ShortDescription,
                                                            PageCount = @PageCount,
                                                            ReleaseDate = @ReleaseDate
                                                            WHERE ISBN=@ISBN", book);

        return result > 0;
    }

    public Task<bool> DeleteAsync(string isbn)
    {
        throw new NotImplementedException();
    }
}
