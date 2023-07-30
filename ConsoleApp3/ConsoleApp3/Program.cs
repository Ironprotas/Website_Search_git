using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks;

class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        string apiUrl = "http://localhost:5246/api/find"; // Замените на свой API URL
        string searchQuery = "Python"; // Замените на строку, которую вы хотите найти

        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(1); // Установите желаемый тайм-аут (например, 5 минут)

                Log.Information($"Sending request for: {searchQuery}");
                var content = new StringContent(searchQuery);
                var response = await httpClient.PostAsync(apiUrl, content);

                Log.Information("Request completed.");

                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    // Обработка ответа JSON
                    Console.WriteLine(resultJson);
                }
                else
                {
                    Log.Error($"Request failed with status code: {response.StatusCode}");
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred: {Message}");
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
