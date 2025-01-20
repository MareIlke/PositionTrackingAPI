Position Tracking API

This is a .NET 8 WebAPI designed for managing and tracking geographical positions. It supports basic operations like adding new positions, fetching stored positions, and calculating distances between positions using the Haversine formula. The API interacts with a SQL Server database to store position data.

Features

-GET /api/positions: Fetches all positions from the database, ordered by pos_name. This returns a list of positions as a JSON array.
-POST /api/positions: Accepts a new position, validates the data (ensures the name is unique, latitude and longitude are valid), and inserts the position into the database.
-GET /api/positions/{name}/distance: Accepts a position name as a path parameter, calculates the distance between the specified position and all other positions in the database using the Haversine formula, and returns the results as a JSON array of distances in kilometers.

Setup

Before starting the application, ensure you have the following installed on your machine:

.NET 8 SDK (Download from here https://dotnet.microsoft.com/en-us/download/dotnet).
SQL Server or any other database system configured with SQL Server (Docker can be used to quickly spin up a SQL Server container).

Clone the Repository:

git clone https://github.com/yourusername/position-tracking-api.git
cd position-tracking-api

Update the Connection String: Open the appsettings.json file and modify the connection string under the "ConnectionStrings" section to match your database configuration:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Snek;User Id=Snek;Trusted_Connection=True;Encrypt=False;"}
  
Replace localhost, Snek, and other fields with the appropriate values for your environment (e.g., database name, username, password).

Install Dependencies: Run the following command to restore project dependencies=> dotnet restore

Start the Application: You can run the application locally using the following command=> dotnet run

API Documentation
Swagger UI
The Position Tracking API uses Swagger for documenting the available endpoints. You can interact with the API directly through Swagger by navigating to:

[http://localhost:5185/swagger]

Swagger will display a list of available endpoints, allow you to send requests, and show the responses for each endpoint.

Screenshots
Swagger UI

![image](https://github.com/user-attachments/assets/24c45764-bd45-46ae-a8df-239adde664a9)

Postman Request

![image](https://github.com/user-attachments/assets/96f328a6-34f9-4734-98ba-685ed08bc03f)

Tests
Unit tests are included to verify the correct behavior of the API.Haversine formula calculations for distance, and proper error handling.

Running the Tests
Run the following command to execute the unit tests=> dotnet test

The tests include the following scenarios:

Haversine Formula Calculation: Ensures the distance between two positions is correctly calculated.
Error Handling: Verifies that proper errors are logged and returned when issues arise, such as invalid input or database connection failures.


API Endpoints
1. GET /api/positions
Description: Fetches all positions from the database, ordered by pos_name.
Response: A JSON array of position objects:

  {
    "name": "Position1",
    "lat": 40.748817,
    "lon": -73.985428
  },
  {
    "name": "Position2",
    "lat": 34.052235,
    "lon": -118.243683
  }

2. POST /api/positions
Description: Adds a new position to the database. The position must have a valid name, latitude, and longitude.

Request Body:
{
  "name": "Position1",
  "lat": 40.748817,
  "lon": -73.985428
}
Response:

201 Created on success.
400 BadRequest if the input is invalid or the position already exists.

3. GET /api/positions/{name}/distance
Description: Calculates the distance between the specified position (via path parameter name) and all other positions in the database using the Haversine formula.
Response:
  {
    "name": "Position2",
    "distance_km": 3936.0
  }



