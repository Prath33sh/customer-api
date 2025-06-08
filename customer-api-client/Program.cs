using System.Net.Http.Json;

Console.Write("Enter the Customer API base URL (e.g., http://localhost:8080/api/customer): ");
var apiBaseUrl = Console.ReadLine();

if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    Console.WriteLine("API base URL is required.");
    return;
}

using var http = new HttpClient();

while (true)
{
    Console.WriteLine("\nCustomer API Client");
    Console.WriteLine("1. Create Customer");
    Console.WriteLine("2. Get Customer By Id");
    Console.WriteLine("3. Update Customer");
    Console.WriteLine("4. Delete Customer");
    Console.WriteLine("Q. Quit");
    Console.Write("Choose an option: ");
    var option = Console.ReadLine();

    if (option?.Equals("Q", StringComparison.OrdinalIgnoreCase) == true)
        break;

    switch (option)
    {
        case "1":
            Console.Write("Enter Customer Details (FirstName, MiddleName, LastName, Email, PhoneNumber) separated by commas: ");
            Console.WriteLine("Example: John, M, Doe, john@example.com, 1234567890");
            var userInput = Console.ReadLine();
            var inputs = userInput?.Split(',');
            var createRequest = new
            {
                firstName = inputs?[0]?.Trim(),
                middleName = inputs?[1]?.Trim(),
                lastName = inputs?[2]?.Trim(),
                email = inputs?[3]?.Trim(),
                phoneNumber = inputs?[4]?.Trim()
            };
            var createResp = await http.PostAsJsonAsync(apiBaseUrl, createRequest);
            if (createResp.IsSuccessStatusCode)
            {
                var created = await createResp.Content.ReadAsStringAsync();
                Console.WriteLine("Created: " + created);
            }
            else
            {
                Console.WriteLine("Error: " + await createResp.Content.ReadAsStringAsync());
            }
            break;

        case "2":
            Console.Write("Enter Customer Id: ");
            var id = Console.ReadLine();
            var getResp = await http.GetAsync($"{apiBaseUrl}/{id}");
            if (getResp.IsSuccessStatusCode)
            {
                var customer = await getResp.Content.ReadAsStringAsync();
                Console.WriteLine("Customer: " + customer);
            }
            else
            {
                Console.WriteLine("Error: " + await getResp.Content.ReadAsStringAsync());
            }
            break;
        case "3":
            Console.Write("Enter Customer Id to Update: ");
            var updateId = Console.ReadLine();
            Console.Write("Enter Details (FirstName, MiddleName, LastName, Email, PhoneNumber) separated by commas: ");
            Console.WriteLine("Example: John, M, Doe, john@example.com, 1234567890");
            userInput = Console.ReadLine();
            inputs = userInput?.Split(',');
            var updateRequest = new
            {
                firstName = inputs?[0]?.Trim(),
                middleName = inputs?[1]?.Trim(),
                lastName = inputs?[2]?.Trim(),
                email = inputs?[3]?.Trim(),
                phoneNumber = inputs?[4]?.Trim()
            };
            var updateResp = await http.PutAsJsonAsync($"{apiBaseUrl}/{updateId}", updateRequest);
            if (updateResp.IsSuccessStatusCode)
            {
                var created = await updateResp.Content.ReadAsStringAsync();
                Console.WriteLine("Updated: " + created);
            }
            else
            {
                Console.WriteLine("Error: " + await updateResp.Content.ReadAsStringAsync());
            }
            break;
        case "4":
            Console.Write("Enter Customer Id: ");
            var deleteId = Console.ReadLine();
            var delResp = await http.DeleteAsync($"{apiBaseUrl}/{deleteId}");
            if (delResp.IsSuccessStatusCode)
            {
                var customer = await delResp.Content.ReadAsStringAsync();
                Console.WriteLine("Customer: " + customer);
            }
            else
            {
                Console.WriteLine("Error: " + await delResp.Content.ReadAsStringAsync());
            }
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

Console.WriteLine("Exiting Customer API Client.");