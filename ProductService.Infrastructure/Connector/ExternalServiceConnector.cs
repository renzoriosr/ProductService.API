using Newtonsoft.Json;
using ProductService.Domain.Interfaces.Infrastructure;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Entities;
using ProductService.Infrastructure.Exceptions;
using System.Net;

namespace ProductService.Infrastructure.Connector
{
    public class ExternalServiceConnector : IExternalServiceConnector
    {
        private readonly HttpClient _httpClient;

        public ExternalServiceConnector(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product> GetProductDiscountAsync(int productId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"productDiscount/{productId}");
            return await SendProviderServiceRequestAsync<Product>(requestMessage);
        }

        #region Private Methods
        private async Task<T> SendProviderServiceRequestAsync<T>(HttpRequestMessage requestMessage)
        {
            using (var response = await _httpClient.SendAsync(requestMessage))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
                var parsedErrorBody = JsonConvert.DeserializeObject<ErrorApiResponse>(apiResponse);
                //_logger.LogError($"Provider service returned status code:{response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new UnauthorizedAccessException();
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException($"Code: {response.StatusCode} Message: {parsedErrorBody.Message}");
                }
                throw new ExternalServiceApiException("Unknown error");
            }
        }
        #endregion
    }
}
