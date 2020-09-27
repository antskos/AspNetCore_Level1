using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly string _serviceAddress;
        protected readonly HttpClient _client;

        protected BaseClient(IConfiguration configuration, string serviceAddress)
        {
            _serviceAddress = serviceAddress;

            _client = new HttpClient
            {
                BaseAddress = new Uri(configuration["WebApiURL"]),
                DefaultRequestHeaders =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue("application/json")}
                }
            };
        }

        // синхронная версия запроса
        protected T Get<T>(string url) => GetAsync<T>(url).Result;

        // асинхронная версия запроса
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default)
        {
            var response = await _client.GetAsync(url, cancel);
            return await response
                .EnsureSuccessStatusCode()      
                .Content.ReadAsAsync<T>();       // десериализуем содержимое контента в нужный тип объекта
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await _client.PostAsJsonAsync(url, item, cancel);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await _client.PutAsJsonAsync(url, item, cancel);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancel = default)
        {
            var response = await _client.DeleteAsync(url, cancel);

            return response;
        }

        #region IDisposable
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            //можно выполнить освобождение неуправляемых ресурсов

            if (disposing)
            {
                // уничтожение управляемых ресурсов
                _client.Dispose();
            }
        }
        #endregion
    }
}
