using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CodeFuseAI.Service.IService;
using static Microsoft.CodeAnalysis.AssemblyIdentityComparer;

namespace CodeFuseAI.Service
{
    public class CodeTranslationService : ICodeTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CodeTranslationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.openai.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiKey = configuration["APIKeys:MyChatGPTDEVKEY2"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> TranslateVBNetToCSharpAsync(string vbNetCode)
        {
            if (IsMessageTooLong(vbNetCode))
            {
                return "Please provide a shorter message. Try breaking down your code into smaller segments";
            }

            try
            {
                var prompt = $"The following is a VB.NET code snippet. Please translate it into equivalent C# code:\n{vbNetCode}";

                var messages = new List<object>
                {
                    new
                    {
                        role = "system",
                        content = "Translate the following VB.NET code into equivalent C# code:"
                    },
                    new
                    {
                        role = "user",
                        content = vbNetCode
                    }
                };

                var requestBody = new
                {
                    model = "gpt-3.5-turbo-16k",
                    messages,
                    temperature = 0.2,
                    max_tokens = 8150, // max out at 8150 tokens
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("v1/chat/completions", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                    return CreateDefaultResponse();
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonDocument.Parse(jsonResponse);
                var result = parsedResponse.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                var comparisonResult = await ExplainCodeDifferencesAsync(vbNetCode, result, "VB.NET", "C#");

                Console.WriteLine($">>>>>>>>>>>>>>>>Received translation: {result}<<<<<<<<<<<<<<<<<");
                Console.WriteLine($">>>>>>>>>>>>>>>>Comparison result: {comparisonResult}<<<<<<<<<<<<<<<<<");


                return result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API request failed with error: {ex.Message}");
                return CreateDefaultResponse();
            }
        }

        public async Task<string> TranslateCSharpToVBNetAsync(string csharpCode)
        {
            if (IsMessageTooLong(csharpCode))
            {
                return "Please provide a shorter message. Try breaking down your code into smaller segments";
            }

            try
            {
                var prompt = $"The following is a C# code snippet. Please translate it into equivalent VB.NET code:\n{csharpCode}";

                var messages = new List<object>
                 {
                    new
                    {
                        role = "system",
                        content = "Translate the following C# code into equivalent VB.NET code:"
                    },
                    new
                    {
                        role = "user",
                        content = csharpCode
                    }
                };

                var requestBody = new
                {
                    model = "gpt-3.5-turbo-16k",
                    messages,
                    temperature = 0.2,
                    max_tokens = 8150, // max out at 8150 tokens
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("v1/chat/completions", content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                    return CreateDefaultResponse();
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonDocument.Parse(jsonResponse);
                var result = parsedResponse.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                var comparisonResult = await ExplainCodeDifferencesAsync(csharpCode, result, "VB.NET", "C#");

                Console.WriteLine($">>>>>>>>>>>>>>>>Received translation: {result}<<<<<<<<<<<<<<<<<");
                Console.WriteLine($">>>>>>>>>>>>>>>>Comparison result: {comparisonResult}<<<<<<<<<<<<<<<<<");


                return result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API request failed with error: {ex.Message}");
                return CreateDefaultResponse();
            }
        }

        public async Task<string> ExplainCodeDifferencesAsync(string originalCode, string translatedCode, string originalLanguage, string translatedLanguage)
        {
            var prompt = $"Given the specific {originalLanguage} code and its {translatedLanguage} translation provided below, please provide a detailed analysis and explanation of the differences, focusing on syntax, structure, and any language-specific features as it pertains to the code shared here:\nOriginal: {originalCode}\nTranslation: {translatedCode}";

            var messages = new List<object>
            {
                new
                {
                    role = "system",
                    content = $"Provide a detailed analysis of the differences between the {originalLanguage} code and its {translatedLanguage} translation, focusing on syntax, structure, and language-specific features."
                },
                new
                {
                    role = "user",
                    content = $"Original: {originalCode}\nTranslation: {translatedCode}"
                }
            };

            var requestBody = new
            {
                model = "gpt-3.5-turbo-16k",
                messages,
                temperature = 0.6,
                max_tokens = 8150,
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Comparison API request failed with status code: {response.StatusCode}");
                return "Failed to compare the codes.";
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var parsedResponse = JsonDocument.Parse(jsonResponse);
            var result = parsedResponse.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return result;
        }

        private string CreateDefaultResponse()
        {
            return "I'm sorry, I couldn't translate the code at the moment, please try again";
        }

        private bool IsMessageTooLong(string message)
        {
            const int maxLength = 15000; // Define the maximum allowed length
            return message.Length > maxLength;
        }
    }
}

