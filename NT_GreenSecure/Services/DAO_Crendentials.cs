﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NT_GreenSecure.Models;
using System.Reflection;
using Xamarin.Essentials;
using System.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Collections.ObjectModel;

namespace NT_GreenSecure.Services
{
    public class DAO_Credentials : IDao_Credentials
    {
        private readonly string baseUrl = "http://10.0.2.2:8089/credentials";
        private readonly HttpClient _client;
        public DAO_Credentials()
        {
            _client = new HttpClient();
        }

        public async Task<(ObservableCollection<Credentials> Result, string Error)> GetAllCredentialsAsync()
        {
            Uri uri = new Uri(baseUrl);
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var credentials = JsonConvert.DeserializeObject<ObservableCollection<Credentials>>(content);
                    return (credentials, null);
                }
                return (null, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCredentialsAsync: {ex.Message}");
                return (null, $"Exception: {ex.Message}");
            }
        }

        public async Task<(Credentials Result, string Error)> GetCredentialByIdAsync(int id)
        {
            Uri uri = new Uri($"{baseUrl}/{id}");
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var credential = JsonConvert.DeserializeObject<Credentials>(content);
                    return (credential, null);
                }
                return (null, $"Error: {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCredentialByIdAsync: {ex.Message}");
                return (null, $"Exception: {ex.Message}");
            }
        }

        // Post
        public async Task<string> AddCredentialAsync(Credentials credential)
        {
            var content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.PostAsync(baseUrl, content);
                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.ReasonPhrase}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }

        // Put
        public async Task<string> UpdateCredentialAsync(Credentials credential)
        {
            var content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.PutAsync($"{baseUrl}/{credential.Id}", content);
                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.ReasonPhrase}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }

        // Delete
        public async Task<string> DeleteCredentialAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"{baseUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.ReasonPhrase}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCredentialAsync: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
            return null;
        }

    }
}