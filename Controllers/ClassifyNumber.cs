using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClassifyNumber.Controllers
{
    [Route("api/classify-number")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ClassificationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Class to deserialize the fun fact response from the Numbers API
        public class FunFactResponse
        {
            public string? text { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? number)
        {
            // Check if input is missing or empty.
            if (string.IsNullOrWhiteSpace(number))
            {
                return BadRequest(new { number = "missing", error = true });
            }

            // Trim spaces and validate input as an integer.
            if (!int.TryParse(number.Trim(), out int validatedNumber))
            {
                return BadRequest(new { number, error = true });
            }

            // Compute mathematical properties.
            bool isPrime = IsPrime(validatedNumber);
            bool isPerfect = IsPerfect(validatedNumber);
            int digitSum = GetDigitSum(validatedNumber);
            bool isArmstrong = IsArmstrong(validatedNumber);

            // Determine the properties array.
            var properties = new List<string>();
            if (isArmstrong) properties.Add("armstrong");
            properties.Add(validatedNumber % 2 == 0 ? "even" : "odd");

            // Fetch a fun fact from the Numbers API.
            // Note: Ensure that CORS is configured globally in your project settings.
            var funFactResponse = await GetFunFact(validatedNumber);
            string funFact = funFactResponse?.text ?? "No fact available";

            // Return the JSON response in the required format.
            return Ok(new
            {
                number = validatedNumber,
                is_prime = isPrime,
                is_perfect = isPerfect,
                properties,
                digit_sum = digitSum,
                fun_fact = funFact
            });
        }

        // Check if a number is an Armstrong number.
        // An Armstrong number equals the sum of its digits each raised to the power of the number of digits.
        private bool IsArmstrong(int num)
        {
            // Use absolute value to handle negative numbers.
            int absNum = Math.Abs(num);
            int sum = 0;
            int nDigits = absNum.ToString().Length;
            int temp = absNum;

            while (temp > 0)
            {
                int digit = temp % 10;
                sum += (int)Math.Pow(digit, nDigits);
                temp /= 10;
            }
            return sum == absNum;
        }

        // Check if a number is prime.
        // A prime number is a number greater than 1 with no divisors other than 1 and itself.
        private bool IsPrime(int num)
        {
            if (num <= 1) return false;
            // Only check up to the square root for efficiency.
            for (int i = 2; i <= Math.Sqrt(num); i++)
            {
                if (num % i == 0) return false;
            }
            return true;
        }

        // Check if a number is perfect.
        // A perfect number equals the sum of its proper divisors (excluding itself).
        private bool IsPerfect(int num)
        {
            if (num <= 0) return false;
            int sum = 1; // 1 is a proper divisor for numbers > 1.
            // Loop through potential divisors up to the square root of num.
            for (int i = 2; i <= Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                {
                    sum += i;
                    if (i != num / i)
                    {
                        sum += num / i;
                    }
                }
            }
            return sum == num && num != 1;
        }

        // Calculate the sum of digits of a number.
        // Using Math.Abs ensures that negative numbers are handled correctly.
        private int GetDigitSum(int num)
        {
            int sum = 0;
            int absNum = Math.Abs(num);
            while (absNum > 0)
            {
                sum += absNum % 10;
                absNum /= 10;
            }
            return sum;
        }

        // Fetch a fun fact from the Numbers API using the 'math' endpoint.
        private async Task<FunFactResponse?> GetFunFact(int num)
        {
            try
            {
                // Request the fun fact in JSON format.
                var response = await _httpClient.GetStringAsync($"http://numbersapi.com/{num}/math?json");
                return JsonConvert.DeserializeObject<FunFactResponse>(response);
            }
            catch
            {
                // If there is an error fetching the fact, return a default message.
                return new FunFactResponse { text = "No fact available" };
            }
        }
    }
}

