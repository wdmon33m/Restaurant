using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Service.IService;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using static Restaurant.Web.Utility.SD;

namespace Restaurant.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                // token

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, ErrorMessages = new List<string>() { "Not Found" } };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, ErrorMessages = new List<string>() { "Access Denied" } };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, ErrorMessages = new List<string>() { "Unauthorized" } };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, ErrorMessages = new List<string>() { "Internal Server Error" } };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    ErrorMessages = new List<string>() { ex.Message },
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
