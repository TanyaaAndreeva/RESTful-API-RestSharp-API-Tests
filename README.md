## StorySpoil API Testing with RestSharp
This project demonstrates how to interact with a RESTful API using RestSharp within a .NET test project. The main objective of this task was to create a set of automated tests that validate key functionalities of the StorySpoil API.

The project includes tests for creating, editing, and deleting story spoilers via the API, as well as handling edge cases such as missing fields or non-existing stories.

# 🛠 Technologies Used
.NET (NUnit for testing)

RestSharp (for making API requests)

JWT Authentication (for securing the API requests)

NUnit (for writing and running the tests)

# 🧪 Testing Overview
This project includes the following tests:

1. Create a New Story Spoiler
Sends a POST request to create a new story with the required fields.

Asserts that the response status code is 201 Created and includes the StoryId in the response.

2. Edit the Created Story Spoiler
Sends a PUT request to edit the created story using the StoryId.

Asserts that the response status code is 200 OK and that the story was successfully edited.

3. Delete a Story
Sends a DELETE request to delete the created story using the StoryId.

Asserts that the response status code is 200 OK and that the story was successfully deleted.

4. Create a Story with Missing Required Fields
Sends a POST request with missing fields (Title or Description).

Asserts that the response status code is 400 Bad Request.

5. Edit a Non-existing Story
Sends a PUT request to edit a story with a non-existing StoryId.

Asserts that the response status code is 404 Not Found and the response message indicates "No spoilers...".

6. Delete a Non-existing Story
Sends a DELETE request to delete a story with a non-existing StoryId.

Asserts that the response status code is 400 Bad Request and the response message indicates "Unable to delete this story spoiler!".

