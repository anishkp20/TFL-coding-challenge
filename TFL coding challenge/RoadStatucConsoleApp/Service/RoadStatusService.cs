using Newtonsoft.Json;
using RoadStatucConsoleApp.Interface;
using RoadStatucConsoleApp.Models;
using System.Net;

namespace RoadStatucConsoleApp.Service
{
    public class RoadStatusService : IRoadStatusService
    {
        private readonly string _appId;
        private readonly string _appKey;
        private readonly string _serviceUrl = "https://api.tfl.gov.uk/Road/{0}?app_id={1}&app_key={2}";
        private readonly HttpClient _httpClient;       
        public RoadStatusService(string appId, string appKey, HttpClient httpClient)
        {
            //Injecting all dependencies from outside of the class
            _appId = appId;
            _appKey = appKey;
            _httpClient = httpClient;
        }
        public RoadStatusService(string appId, string appKey)
        {            
            _appId = appId;
            _appKey = appKey;
            _httpClient = new HttpClient();
        }
        public async Task<RoadStatus> GetRoadStatusAsync(string roadId)
        {
            try
            {
                string requestUrl = string.Format(_serviceUrl, roadId, _appId, _appKey);

                var responseMessage = await _httpClient.GetAsync(requestUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<RoadStatus>>(jsonResponse)?.FirstOrDefault();

                }
                else if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                    var statusError = JsonConvert.DeserializeObject<RoadStatusError>(errorResponse);
                    throw new NotFoundException(statusError?.Message);
                }
                else
                {
                    throw new Exception("Unexpected error while accessing API.");
                }

            }
            catch(NotFoundException ex)
            {
                throw;
            }
            catch (Exception ex) { throw; }
        }
    }
}
