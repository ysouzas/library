using System.Data;
using System.Threading.Tasks;

namespace Library.Api.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}
