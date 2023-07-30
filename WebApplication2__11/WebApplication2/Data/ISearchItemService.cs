using System.Threading.Tasks;
using WebApplication2.Data;

namespace WebApplication2.Services
{
    public interface ISearchItemService
    {
        Task SaveSearchItemAsync(SearchItem searchItem);
    }
}
