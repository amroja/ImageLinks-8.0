using Dapper;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace ImageLinks_.Infrastructure.Repository
{
    public sealed class GenericRepository : IGenericRepository
    {
        private readonly IConfiguration _configuration;
        public GenericRepository(IConfiguration configuration) => _configuration = configuration;

        private string GetConn(string? overrideConn = null)
            => !string.IsNullOrWhiteSpace(overrideConn)
               ? overrideConn
               : _configuration.GetConnectionString("DefaultConnection")!;

        private IDbConnection CreateConnection(string? overrideConn = null)
        {
            var connStr = ModifyConnectionString(GetConn(overrideConn));
            return GetDatabaseType(connStr) switch
            {
                DatabaseProvider.Oracle => new OracleConnection(connStr),
                _ => new SqlConnection(connStr)
            };
        }

        public DatabaseProvider GetDatabaseType(string? nameOrConn = null)
        {
            if (string.IsNullOrWhiteSpace(nameOrConn))
                nameOrConn = "DefaultConnection";


            var providerName = _configuration.GetConnectionString($"{nameOrConn}_ProviderName");
            if (!string.IsNullOrWhiteSpace(providerName))
            {
                return providerName.ToUpperInvariant() switch
                {
                    "SQL" or "SYSTEM.DATA.SQLCLIENT" or "MICROSOFT.DATA.SQLCLIENT" => DatabaseProvider.SqlServer,
                    "ORACLE" or "ORACLE.MANAGEDDATAACCESS.CLIENT" or "ORACLE.MANAGEDDATAACCESS.CORE" => DatabaseProvider.Oracle,
                    _ => throw new NotSupportedException($"Provider not supported for {nameOrConn}")
                };
            }

            var connStr = GetConn(nameOrConn);
            if (connStr.Contains("User Id=", StringComparison.OrdinalIgnoreCase) &&
                connStr.Contains("Data Source=", StringComparison.OrdinalIgnoreCase))
                return DatabaseProvider.Oracle;

            if (connStr.Contains("Server=", StringComparison.OrdinalIgnoreCase) ||
                connStr.Contains("Initial Catalog=", StringComparison.OrdinalIgnoreCase))
                return DatabaseProvider.SqlServer;

            throw new NotSupportedException($"Provider not supported for {nameOrConn}");
        }

        public async Task<string?> ExecuteScalarAsync(
            string sql,
            object? parameters = null,
            string? connectionString = null,
            CancellationToken ct = default)
        {
            IDbConnection? db = CreateConnection(connectionString);
            var result = await db.ExecuteScalarAsync<object?>(new CommandDefinition(sql, parameters, cancellationToken: ct));
            return result?.ToString();
        }

        public async Task<DataTable> GetDataTableAsync(
            string sql,
            object? parameters = null,
            string? connectionString = null,
            CancellationToken ct = default)
        {
            IDbConnection? db = CreateConnection(connectionString);
            using var reader = await db.ExecuteReaderAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));
            var dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
        public async Task<List<T?>> GetListAsync<T>(
           string sql,
           object? parameters = null,
           string? connectionString = null,
           CancellationToken ct = default)
        {
            using IDbConnection db = CreateConnection(connectionString);

            var result = await db.QueryAsync<T>(
                new CommandDefinition(sql, parameters, cancellationToken: ct));

            return result.ToList();
        }

        public async Task<int> ExecuteNonQueryAsync(
            string sql,
            object? parameters = null,
            string? connectionString = null,
            CancellationToken ct = default)
        {
             IDbConnection? db = CreateConnection(connectionString);
            return await db.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));
        }

        public async Task<int> ExecuteTransactionAsync(
            IEnumerable<string> sqlStatements,
            string? connectionString = null,
            CancellationToken ct = default)
        {
            using var db = CreateConnection(connectionString);
            using var tx = db.BeginTransaction();

            try
            {
                var affected = 0;
                foreach (var sql in sqlStatements)
                {
                    if (string.IsNullOrWhiteSpace(sql)) continue;

                    affected += await db.ExecuteAsync(new CommandDefinition(sql.Trim(), transaction: tx, cancellationToken: ct));
                }
                tx.Commit();
                return affected;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public bool IsDatabaseEmpty()
        {
            var dbType = GetDatabaseType();
            var sql = dbType == DatabaseProvider.SqlServer
                ? "SELECT COUNT(*) FROM sysobjects WHERE type='U' AND name='users'"
                : "SELECT COUNT(table_name) FROM user_tables WHERE table_name='USERS'";
            return Convert.ToInt32(ExecuteScalarAsync(sql).GetAwaiter().GetResult()) == 0;
        }

        public string GetDatabaseName()
        {
            var conn = GetConn();
            return GetDatabaseType(conn) == DatabaseProvider.Oracle
                ? conn.Split("User ID")[1].Split(';')[0].Replace("=", "").Trim()
                : conn.Split("Initial Catalog")[1].Split(';')[0].Replace("=", "").Trim();
        }

        private static string ModifyConnectionString(string conn)
        {
            return string.Join(';',
                conn.Split(';').Where(p => !p.Trim().StartsWith("provider=", StringComparison.OrdinalIgnoreCase)));
        }
    }
}
