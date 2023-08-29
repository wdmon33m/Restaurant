﻿using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Service.IService;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using static Restaurant.Web.Utility.SD;
using System.Net.Mime;

namespace Restaurant.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("RestaurantAPI");
                HttpRequestMessage message = new();
                if (requestDto.ContentType == Utility.SD.ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                }
                else
                {
                    message.Headers.Add("Accept", MediaTypeNames.Application.Json);
                }
                //token
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.ContentType == Utility.SD.ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();

                    foreach (var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (requestDto.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, MediaTypeNames.Application.Json);
                    }
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
                    case ApiType.PATCH:
                        message.Method = HttpMethod.Patch;
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
                    case HttpStatusCode.UnsupportedMediaType:
                        return new() { IsSuccess = false, ErrorMessages = new List<string>() { "Unsupported Media Type" } };
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
