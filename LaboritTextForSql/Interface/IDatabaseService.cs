namespace LaboritTextForSql.Interface
{
	public interface IDatabaseService
	{
		Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
	}
}
