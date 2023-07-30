using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
    private static readonly List<SearchResult> _searchResults = new List<SearchResult>();
    private static int _currentId = 1;

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting up the API...");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "API terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapPost("/api/find", FindAsync);
                        endpoints.MapGet("/api/find", GetSearchResultsAsync);
                        endpoints.MapDelete("/api/find/{id}", DeleteSearchResultAsync);
                    });
                });
            });

    public static async Task FindAsync(HttpContext context)
    {
        try
        {
            string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Search string cannot be empty");
                return;
            }

            string searchString = requestBody;

            SearchResult result = _searchResults.FirstOrDefault(r => r.SearchString == searchString);

            if (result == null)
            {
                // Implement the logic to query the SQLite database or the GitHub API based on the searchString
                // For the sake of this example, let's assume the projects list is hardcoded.

                var projects = new List<Project>
                {
                    new Project { Id = 1, Name = "Project 1", Author = "Author 1", Stars = 100, Views = 5000 },
                    new Project { Id = 2, Name = "Project 2", Author = "Author 2", Stars = 50, Views = 2000 }
                    // Add more projects here
                };

                result = new SearchResult
                {
                    Id = _currentId++,
                    SearchString = searchString,
                    Projects = projects
                };

                _searchResults.Add(result);
            }

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error occurred while searching for projects");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred while processing your request");
        }
    }

    public static async Task GetSearchResultsAsync(HttpContext context)
    {
        try
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(_searchResults));
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error occurred while retrieving search results");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred while processing your request");
        }
    }

    public static async Task DeleteSearchResultAsync(HttpContext context)
    {
        try
        {
            if (!int.TryParse(context.Request.RouteValues["id"]?.ToString(), out int id))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid search result ID");
                return;
            }

            var resultToRemove = _searchResults.FirstOrDefault(r => r.Id == id);

            if (resultToRemove == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            _searchResults.Remove(resultToRemove);
            context.Response.StatusCode = 200;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error occurred while deleting search result");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred while processing your request");
        }
    }
}
