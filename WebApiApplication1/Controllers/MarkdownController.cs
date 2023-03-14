using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebApiApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarkdownController : ControllerBase
    {
       
        private readonly ILogger<MarkdownController> _logger;

        public MarkdownController(ILogger<MarkdownController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        private readonly HttpClient _httpClient;

        [HttpGet]
        public async Task<IActionResult> Get(string text)
        {

            var json = new
            {
               // mode = "markdown",
                text = text
            };
            string jsonString = JsonSerializer.Serialize(json);

            #region Configure Access for HttpClient (not in use here)

            //        var accessToken = "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";
            //        accessToken = "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";

            //        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            //        _httpClient.DefaultRequestHeaders.Authorization =
            //new AuthenticationHeaderValue("Token", accessToken);
            #endregion

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "WebApiApplication1");

            var requestContent = new StringContent(jsonString);
            #region Configure request Headers (not in use here)
            //requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            //requestContent.Headers.Add("X-GitHub-Api-Version", "2022-11-28"); 
            #endregion
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/markdown");
            request.Content = requestContent;

            #region Using SendAsync Method in HttpClient
            //// set the Accept header on the HttpRequestMessage
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //// send the request and wait for the response
            //var response = await _httpClient.SendAsync(request);

            //// read the response content as a string
            //var responseContent = await response.Content.ReadAsStringAsync(); 
            #endregion

            var response = await _httpClient.PostAsync("https://api.github.com/markdown", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();
                return Content(htmlContent, "text/html");
            }
            else
            {
                return BadRequest();
            }
        }


    }
}