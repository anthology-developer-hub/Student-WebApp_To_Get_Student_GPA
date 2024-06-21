using GetStudentGpa.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GetStudentGpa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetStudentGPAInfo(string StudentId, string username, string password)
        {
            var studentInfo = new StudentGPAModel();
            string url = buildUrl(StudentId, username, password);
            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                studentInfo = JsonConvert.DeserializeObject<StudentGPAModel>(responseBody);
            }
            return View(studentInfo);
        }

        private string buildUrl(string StudentId, string username, string Password)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            return string.Format($"{baseUrl}/api/GetStudent/GetStudentGPAInfo?StudentId={StudentId}&username={username}&password={Password}");
        }
    }
}
