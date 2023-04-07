using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using AppRpgEtect.Services.Usuarios;
using Newtonsoft.Json;

namespace AppRpgEtect.Services
{
    public class Request
    {
        public async Task<int> PostReturnIntAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage reponse = await httpClient.PostAsync(uri, content);

            string serialized = await reponse.Content.ReadAsStringAsync();

             if( reponse.StatusCode == System.Net.HttpStatusCode.OK)
                return int.Parse(serialized);
             else
                return 0;

        }

        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = data;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized));
        
                return result;

        }

        public async Task<int> PutAsync<TResult>(string uri, TResult data, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
             = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync(); 

            if (response.StatusCode == System.Net.HttpStatusCode.OK) return int.Parse(serialized);
            else
                return 0;
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await httpClient.GetAsync(uri); string serialized = await response.Content.ReadAsStringAsync(); TResult result = await Task.Run(() =>
            JsonConvert.DeserializeObject<TResult>(serialized)); return result;
        }

        public async Task DeleteAsync(string uri, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", token);
            await httpClient.DeleteAsync(uri);
        }


    }
}
