using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api")]
    public class FindController : ControllerBase
    {
        private readonly ILogger<FindController> _logger;
        private static readonly List<SearchResult> _searchResults = new List<SearchResult>();
        private static int _currentId = 1;

        public FindController(ILogger<FindController> logger)
        {
            _logger = logger;
        }

        [HttpPost("find")]
        public async Task<IActionResult> Find()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    string searchString = await reader.ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(searchString))
                    {
                        return BadRequest("Search string cannot be empty");
                    }

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

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for projects");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("find")]
        public IActionResult GetSearchResults()
        {
            try
            {
                return Ok(_searchResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving search results");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpDelete("find/{id}")]
        public IActionResult DeleteSearchResult(int id)
        {
            try
            {
                var resultToRemove = _searchResults.FirstOrDefault(r => r.Id == id);

                if (resultToRemove == null)
                {
                    return NotFound();
                }

                _searchResults.Remove(resultToRemove);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting search result");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
