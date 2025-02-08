# Number Classification API

A simple web API built with ASP.NET Core that accepts a number as input and returns interesting mathematical properties along with a fun fact fetched from an external API.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation & Setup](#installation--setup)
- [API Usage](#api-usage)
- [Project Structure](#project-structure)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Number Classification API processes an integer provided via an HTTP GET request. It returns a JSON object containing:
- Whether the number is prime.
- Whether the number is perfect.
- A list of properties that indicate if the number is Armstrong and whether it is even or odd.
- The sum of its digits.
- A fun fact about the number fetched from the [Numbers API](http://numbersapi.com/).

## Features

- **Input Validation:**
  Validates that the input is a non-empty integer. If the input is invalid, returns a 400 Bad Request with an error message.

- **Mathematical Computations:**
  - **Prime Check:** Determines if a number is prime.
  - **Perfect Number Check:** Checks if a number equals the sum of its proper divisors.
  - **Armstrong Number Check:** Determines if the number is an Armstrong (narcissistic) number.
  - **Digit Sum:** Calculates the sum of the digits of the number.

- **Fun Fact Retrieval:**
  Retrieves a fun fact using the Numbers API (math type) and includes it in the response.

- **Proper Error Handling:**
  Returns structured error responses for invalid inputs.

## Technologies Used

- **Language:** C#
- **Framework:** ASP.NET Core
- **HTTP Client:** `HttpClient`
- **JSON Parsing:** Newtonsoft.Json
- **External API:** [Numbers API](http://numbersapi.com/)

## Installation & Setup

### Prerequisites

- [.NET 6.0 SDK (or later)](https://dotnet.microsoft.com/download)
- An IDE or text editor of your choice (e.g., Visual Studio, Visual Studio Code)

### Steps to Run Locally

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/your-username/number-classification-api.git
   cd number-classification-api
 HnGDevopsNumberClassificationApi



Restore Dependencies:

From the project directory, run:

bash
Copy
Edit
dotnet restore
Run the Application:

bash
Copy
Edit
dotnet run
Access the API:

Open your browser or API testing tool (e.g., Postman) and send a GET request to:

bash
Copy
Edit
http://localhost:5000/api/classify-number?number=371
Adjust the port number if needed (the default is often 5000 or 5001 for HTTPS).

CORS Configuration
Ensure that Cross-Origin Resource Sharing (CORS) is configured in your Program.cs or Startup.cs if you plan to access the API from a different domain. For example:

csharp
Copy
Edit
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
...
app.UseCors("AllowAll");
API Usage
Endpoint
typescript
Copy
Edit
GET /api/classify-number?number={number}
Query Parameter
number: A valid integer (e.g., 371).
Success Response (HTTP 200)
json
Copy
Edit
{
  "number": 371,
  "is_prime": false,
  "is_perfect": false,
  "properties": ["armstrong", "odd"],
  "digit_sum": 11,
  "fun_fact": "371 is an Armstrong number because 3^3 + 7^3 + 1^3 = 371"
}
Error Response (HTTP 400)
If the input is missing or not a valid integer:


Edit
{
  "number": "alphabet",
  "error": true
}
Project Structure

number-classification-api/
├── Controllers/
│   └── ClassificationController.cs   // API endpoint and logic
├── Program.cs                          // Application entry point and configuration
├── Startup.cs (if applicable)          // ASP.NET Core configuration (for older templates)
├── README.md                           // Project documentation
└── number-classification-api.csproj    // Project file

Controllers/ClassificationController.cs:
Contains the logic for validating input, performing calculations (prime, perfect, Armstrong, digit sum), and fetching the fun fact.

Program.cs / Startup.cs:
Configures services, middleware (including CORS), and sets up the routing for the API.

Deployment
The API should be deployed to a publicly accessible endpoint. You can use platforms such a

Contributing
If you wish to contribute, please follow these guidelines:

Fork the repository.
Create a new branch for your feature or bug fix.
Write clear and concise code, and add comments where necessary.
Submit a pull request detailing your changes.
