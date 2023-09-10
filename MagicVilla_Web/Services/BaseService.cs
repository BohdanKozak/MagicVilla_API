using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponce responceModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }


        public BaseService(IHttpClientFactory httpClient)
        {
            responceModel = new();
            _httpClient = httpClient;

        }


        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }


                HttpResponseMessage apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponce ApiResponce = JsonConvert.DeserializeObject<APIResponce>(apiContent);
                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                        apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ApiResponce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        ApiResponce.IsSuccess = false;

                        var res = JsonConvert.SerializeObject(ApiResponce);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception e)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }
                var APIResponce = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponce;
            }
            catch (Exception ex)
            {
                var dto = new APIResponce
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var ApiResponce = JsonConvert.DeserializeObject<T>(res);
                return ApiResponce;
            }
        }
    }
}
