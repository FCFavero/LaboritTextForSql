using LaboritTextForSql.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LaboritTextForSql.Controllers
{
	public class TextForSqlController : Controller
	{
		private readonly IDatabaseService _databaseService;
		private readonly IText2SQLService _text2SQLService;

		public TextForSqlController(IDatabaseService databaseService, IText2SQLService text2SQLService)
		{
			_databaseService = databaseService;
			_text2SQLService = text2SQLService;
		}

		public class NaturalLanguageQuery
		{
			public string Query { get; set; }
		}

		[HttpPost("natural-language-query")]
		public async Task<IActionResult> ExecuteNaturalLanguageQuery([FromBody] NaturalLanguageQuery query)
		{
			try
			{
				var sql = await _text2SQLService.ConvertToSQLAsync(query.Query);
				var result = await _databaseService.QueryAsync<dynamic>(sql);
				return Ok(result);
			}
			catch (HttpRequestException ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
