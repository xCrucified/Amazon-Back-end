using business_logic.DTOs;
using business_logic.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly HttpClient _httpClient;
        public GoogleAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GoogleUserInfo?> ValidateGoogleTokenAsync(string token)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                const string GoogleUserInfoUrl = "";
                using var request = new HttpRequestMessage(HttpMethod.Get, GoogleUserInfoUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<GoogleUserInfoWeb>(jsonResponse);

                if (userInfo == null)
                {
                    return null;
                }

                return new GoogleUserInfo
                {
                    Email = userInfo.Email
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
