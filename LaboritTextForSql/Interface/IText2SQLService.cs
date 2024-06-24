namespace LaboritTextForSql.Interface
{
	public interface IText2SQLService
	{
		Task<string> ConvertToSQLAsync(string naturalLanguageQuery);
	}
}
