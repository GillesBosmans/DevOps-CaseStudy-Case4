using models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dal.api
{
	public class ApiLoginValidatie
	{
		private const string ApiBaseUrl = "https://gbautopartsapirailway-production.up.railway.app";
		public async Task<LoginValidatie> AuthenticateKlantAsync(string username, string password)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					// Prepare the login request body
					var requestBody = new
					{
						username = username,
						password = password
					};

					// Convert the request body to JSON
					var jsonBody = JsonConvert.SerializeObject(requestBody);

					// Create the HTTP content
					var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

					// Send the POST request to the login endpoint
					HttpResponseMessage response = await client.PostAsync($"{ApiBaseUrl}/login", content);

					// Check if the request was successful (status code 200)
					if (response.IsSuccessStatusCode)
					{
						// Read and parse the response content
						string responseContent = await response.Content.ReadAsStringAsync();
						LoginValidatie loginValidatie = JsonConvert.DeserializeObject<LoginValidatie>(responseContent);

						// Return the parsed response
						return loginValidatie;
					}
					else
					{
						// Handle authentication failure
						// You might want to throw an exception, display an error message, or handle it as needed
						return null;
					}
				}
				catch (Exception ex)
				{
					// Handle exceptions (e.g., network errors, server unreachable)
					// You might want to throw an exception, display an error message, or handle it as needed
					return null;
				}
			}
		}
	}
}
