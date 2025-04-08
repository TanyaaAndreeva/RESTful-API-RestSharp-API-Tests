
using RestSharp;
using RestSharp.Authenticators;
using Story_Exam.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization; 


namespace Story_Exam

{
    [TestFixture]
    public class StoryTests
    {
        private RestClient client;

       
        private string? storyId;

        [OneTimeSetUp]
        public void Setup()
        {
            string jwtToken = GetJwtToken("Tanya_Test", "123456789");

            var options = new RestClientOptions("https://d5wfqm7y6yb3q.cloudfront.net")
            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };

            this.client = new RestClient(options);
        }

        private string GetJwtToken(string userName, string password)
        {
            var tempClient = new RestClient("https://d5wfqm7y6yb3q.cloudfront.net");
            var request = new RestRequest("/api/User/Authentication", Method.Post);
            request.AddJsonBody(new
            {
                userName,
                password
            });

            var response = tempClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var token = content.GetProperty("accessToken").GetString();
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException("The JWT token is null or empty.");
                }
                return token;
            }
            else
            {
                throw new InvalidOperationException($"Authentication failed: {response.StatusCode}, {response.Content}");
            }
        }

        [Test, Order(1)]
        public void CreateANewStorySpoiler_WithTheRequiredFields_ShouldSucceed()
        {
            var storyRequest = new StoryDTO
            {
                Title = "Story Spoiler  RestSharp Test",
                Url = "",
                Description = "A detailed description of the stort."

            };

            var request = new RestRequest("/api/Story/Create", Method.Post);
            request.AddJsonBody(storyRequest);
            var response = this.client.Execute(request);
            var createResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(createResponse.Msg, Is.EqualTo("Successfully created!"));

            storyId = createResponse.StoryId;


        }
        [Test, Order(2)]
        public void EditTheCreatedStorySpoiler_WithTheRequiredFields_ShouldSucceed()
        {
            var editRequest = new StoryDTO
            {
                Title = "Edited Story Title",
                Description = "Edited description"

            };

            var request = new RestRequest($"/api/Story/Edit/{storyId}", Method.Put);
           
            request.AddJsonBody(editRequest);
            var response = this.client.Execute(request);
            var editResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(editResponse.Msg, Is.EqualTo("Successfully edited"));
        }

        [Test, Order(3)]
        public void DeleteAStoryYouCreated_ShouldSucceed()
            {
            var request = new RestRequest($"/api/Story/Delete/{storyId}", Method.Delete);
           
            var response = this.client.Execute(request);

            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(content.Msg, Is.EqualTo("Deleted successfully!"));


        }
        [Test,Order (4)]
        public void CreateANewStorySpoilerWithoutRequiredFields_ShouldReturnBadRequest()
        {
            var storyRequest = new StoryDTO
            {
              

            };

            var request = new RestRequest("/api/Story/Create", Method.Post);
            request.AddJsonBody(storyRequest);
            var response = this.client.Execute(request);
            var createResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            
        }
        [Test, Order(5)]
        public void EditANonExistingStory_ShoulReturnNotFound()
        {
            var editRequest = new StoryDTO
            {
                Title = "Edited Story Title",
                Description = "Edited description"

            };

            var request = new RestRequest($"/api/Story/Edit/NONEXISTINID", Method.Put);

            request.AddJsonBody(editRequest);
            var response = this.client.Execute(request);
            var editResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(editResponse.Msg, Is.EqualTo("No spoilers..."));

        }
        [Test, Order(6)]
        public void DeleteANonExistingOrder_ShouReturnBadRequest()
        {
            var request = new RestRequest($"/api/Story/Delete/NONEXISTINGID", Method.Delete);
            
            var response = this.client.Execute(request);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(content.Msg, Is.EqualTo("Unable to delete this story spoiler!"));

        }
    }
}