using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SearchApp.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication2.Data;

namespace WebApplication2.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public SearchModel(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:109.0) Gecko/20100101 Firefox/114.0");
        }

        [BindProperty]
        public string SearchQuery { get; set; }

        public List<SearchItem> SearchResults { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"https://api.github.com/search/repositories?q={SearchQuery}";
                try
                {
                    var response = await _httpClient.GetStringAsync(apiUrl);
                    var jsonDocument = JsonDocument.Parse(response);
                    var searchItems = new List<SearchItem>();

                    foreach (var item in jsonDocument.RootElement.GetProperty("items").EnumerateArray())
                    {
                        var projectName = item.GetProperty("name").GetString();
                        var author = item.GetProperty("owner").GetProperty("login").GetString();
                        var stargazersCount = item.GetProperty("stargazers_count").GetInt32();
                        var watchersCount = item.GetProperty("watchers_count").GetInt32();
                        var htmlUrl = item.GetProperty("html_url").GetString();

                        var searchItem = new SearchItem
                        {
                            SearchQuery = SearchQuery,
                            ResultJson = response,
                            ProjectName = projectName,
                            Author = author,
                            StargazersCount = stargazersCount,
                            WatchersCount = watchersCount,
                            HtmlUrl = htmlUrl
                        };

                        searchItems.Add(searchItem);
                    }

                    // Сохранение данных в базу данных
                    _context.SearchItems.AddRange(searchItems);
                    await _context.SaveChangesAsync();

                    SearchResults = searchItems;
                }
                catch (HttpRequestException ex)
                {
                    // Обработка ошибки при выполнении запроса к API GitHub
                    // Выводите сообщение об ошибке или выполняйте другие действия при ошибке
                    // ex.Message содержит текст ошибки
                }
                catch (DbUpdateException ex)
                {
                    // Обработка ошибки при сохранении объекта в базу данных
                    // Выводите сообщение об ошибке или выполняйте другие действия при ошибке
                    // ex.Message содержит текст ошибки
                }
            }

            return Page();
        }
    }
}
