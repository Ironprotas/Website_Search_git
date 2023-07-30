using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SearchResultRepository _searchResultRepository;

        public SearchController(SearchResultRepository searchResultRepository)
        {
            _searchResultRepository = searchResultRepository;
        }

        // POST /api/find
        [HttpPost]
        public IActionResult Search([FromBody] string searchString)
        {
            // Ваш механизм поиска, основанный на searchString
            // Здесь мы просто создаем фиктивный результат поиска для примера
            var searchResult = new SearchResult
            {
                ProjectName = "Sample Project",
                Author = "Sample Author",
                StargazersCount = 100,
                WatchersCount = 50,
                HtmlUrl = "https://github.com/sample/project"
            };

            _searchResultRepository.Add(searchResult);
            return Ok(searchResult);
        }

        // GET /api/find
        [HttpGet]
        public IActionResult GetAllSearchResults()
        {
            var searchResults = _searchResultRepository.GetAll();
            return Ok(searchResults);
        }

        // DELETE /api/find/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSearchResult(int id)
        {
            var searchResult = _searchResultRepository.GetById(id);
            if (searchResult == null)
            {
                return NotFound();
            }

            _searchResultRepository.Remove(id);
            return NoContent();
        }
    }
}
