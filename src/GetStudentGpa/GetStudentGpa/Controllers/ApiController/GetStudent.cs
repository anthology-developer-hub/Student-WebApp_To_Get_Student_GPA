using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Text;
using GetStudentGpa.Models;

namespace StudentInfoById.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetStudent : ControllerBase
    {
        [HttpGet("GetStudentGPAInfo")]
        public async Task<IActionResult> getStudentInfo([FromQuery] int studentId, [FromQuery] string username, [FromQuery] string password)
        {
            // URL to make the request to
            string url = $"https://sisclientweb-700031.campusnexus.cloud/api/commands/Academics/AdditionalGpa/getStudentGpa";

            // Create HttpClient instance
            using var httpClient = new HttpClient();

            // Set Basic Authentication header
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}")));
            httpClient.DefaultRequestHeaders.Authorization = authValue;
            var jobject = new
            {
                payload = new
                {
                    StudentId = studentId
                }
            };
            var json = JsonConvert.SerializeObject(jobject);
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                // Make a GET request to the URL
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject responseJson = JObject.Parse(responseBody);

                    StudentGPAModel studentGpaResult = new StudentGPAModel();

                    studentGpaResult.CumulativeGpa = (double)responseJson["payload"]["data"]["studentGpaList"][0]["cumulativeGpa"];
                    studentGpaResult.UnitsEarned = (double)responseJson["payload"]["data"]["studentGpaList"][0]["unitsEarned"];
                    studentGpaResult.UnitsAttempted = (double)responseJson["payload"]["data"]["studentGpaList"][0]["unitsAttempted"];
                    studentGpaResult.QualityPoints = (double)responseJson["payload"]["data"]["studentGpaList"][0]["qualityPoints"];
                    studentGpaResult.QualityUnits = (double)responseJson["payload"]["data"]["studentGpaList"][0]["qualityUnits"];
                    studentGpaResult.GradePoints = (double)responseJson["payload"]["data"]["studentGpaList"][0]["gradePoints"];
                    studentGpaResult.CoursesTaken = (int)responseJson["payload"]["data"]["studentGpaList"][0]["coursesTaken"];
                    studentGpaResult.CumulativeGpaText = (string)responseJson["payload"]["data"]["studentGpaList"][0]["cumulativeGpaText"];
                    
                    var studentGpaInfo = studentGpaResult;
                    // Return the response
                    return Ok(studentGpaInfo);
                }
                else
                {
                    // Return the error status code
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Return the error message
                return BadRequest(ex.Message);
            }
        }
    }
}
