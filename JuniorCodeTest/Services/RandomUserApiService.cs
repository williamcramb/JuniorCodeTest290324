using JuniorCodeTest.Interfaces;
using JuniorCodeTest.Models;

using System.Text.Json;

namespace JuniorCodeTest.Services
{
	public class RandomUserApiService(HttpClient httpClient) : IRandomUserApiService
	{
		private const string randomUserEndPoint = "https://randomuser.me/api";

		public async Task<List<RequestedUsersModel>> GetRandomUserDataFromApi()
		{
			var requiredDataList = new List<RequestedUsersModel>();

			//Added for statement to retrieve 5 sets of randomUsers data
			for (int i = 0; i < 5; i++)
			{ 
				var response = await httpClient.GetAsync(randomUserEndPoint);

				if (response.IsSuccessStatusCode)
				{
					string responseData = await response.Content.ReadAsStringAsync();
					var randomUserModelData = JsonSerializer.Deserialize<UserModel>(responseData);
					if (randomUserModelData != null)
					{

						foreach (var randomUser in randomUserModelData.results)
						{
							var requiredData = new RequestedUsersModel()
							{
								Title = randomUser.name.title,
								First = randomUser.name.first,
								Last = randomUser.name.last,
								Age = randomUser.dob.age,
								Country = randomUser.location.country,
								Longitude = randomUser.location.coordinates.longitude,
								Latitude = randomUser.location.coordinates.latitude,
							};

							requiredDataList.Add(requiredData);

							if (requiredDataList.Count >= 5)
							{
								break;
							}

						}

					}
				}
				else
				{
					throw new Exception("Failed to fetch data from the random user API");
				}
				
			}
			return requiredDataList;

		}
	}
}
