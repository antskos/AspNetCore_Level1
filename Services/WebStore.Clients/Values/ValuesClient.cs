using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Clients.Values
{
     public class ValuesClient : BaseClient, IValueService
    {
        public ValuesClient(IConfiguration configuration) : base(configuration, "api/values") {}

        public HttpStatusCode Delete(int id)
        {
            var response = _client.DeleteAsync($"{_serviceAddress}/{id}").Result;
            return response.StatusCode;
        }

        public IEnumerable<string> Get()
        {
            var response = _client.GetAsync(_serviceAddress).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<string>>().Result;
            else
                return Enumerable.Empty<string>();

        }

        public string Get(int id)
        {
            var response = _client.GetAsync($"{_serviceAddress}/{id}").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<string>().Result;
            else
                return string.Empty;
        }

        public Uri Post(string value)
        {
            var response = _client.PostAsJsonAsync(_serviceAddress, value).Result;
            return response.EnsureSuccessStatusCode().Headers.Location;
        }

        public HttpStatusCode Update(int id, string value)
        {
            var response = _client.PutAsJsonAsync($"{_serviceAddress}/{id}", value).Result;
            return response.EnsureSuccessStatusCode().StatusCode;
        }
    }
}
