using LaboritTextForSql.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace LaboritTextForSql.Service
{
	public class Text2SQLService : IText2SQLService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public Text2SQLService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_apiKey = configuration["HuggingFace:ApiKey"];
		}
		public async Task<string> ConvertToSQLAsync(string naturalLanguageQuery)
		{
			try
			{
				var apiUrl = "https://api-inference.huggingface.co/models/Tigran555/text2sql";

				var requestData = new
				{
					inputs = $"translate English to Query SQL:{naturalLanguageQuery}",
				};

				var jsonRequest = JsonConvert.SerializeObject(requestData);
				var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

				// Adicionar cabeçalho de autorização, se necessário
				if (!string.IsNullOrEmpty(_apiKey))
				{
					_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
				}

				// Enviar requisição HTTP POST
				HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

				// Verificar se a requisição foi bem-sucedida
				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					Debug.WriteLine($"Error: {response.StatusCode} - {errorContent}");
					throw new HttpRequestException($"Response status code does not indicate success: {response.StatusCode} ({errorContent}).");
				}

				// Ler e desserializar a resposta JSON
				var result = await response.Content.ReadAsStringAsync();
				var jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);

				// Verificar se a resposta JSON é um array e se possui pelo menos um item
				if (!(jsonResponse is JArray) || jsonResponse.Count == 0)
				{
					throw new Exception("Failed to parse response or no SQL query generated.");
				}

				// Acessar a consulta SQL gerada
				var firstItem = jsonResponse[0];
				var sqlQuery = firstItem["generated_text"].ToString(); // ou ajuste conforme a estrutura da resposta

				return sqlQuery;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception: {ex.Message}");
				throw;
			}

		}

	}
}
