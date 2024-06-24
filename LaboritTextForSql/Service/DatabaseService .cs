using Dapper;
using LaboritTextForSql.Interface;
using MySql.Data.MySqlClient;

namespace LaboritTextForSql.Service
{
	public class DatabaseService : IDatabaseService
	{
		private readonly IConfiguration _configuration;

		public DatabaseService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
		{
			using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
			await connection.OpenAsync(); 
			return await connection.QueryAsync<T>(sql, parameters);
		}
	}
}
