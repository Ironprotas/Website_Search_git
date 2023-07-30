using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SearchApp.Data;
using WebApplication2.Data;

namespace WebApplication2.Services
{
    public class SearchItemService : ISearchItemService
    {
        private readonly ApplicationDbContext _context;

        public SearchItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveSearchItemAsync(SearchItem searchItem)
        {
            _context.SearchItems.Add(searchItem);
            await _context.SaveChangesAsync();
        }
    }
}
