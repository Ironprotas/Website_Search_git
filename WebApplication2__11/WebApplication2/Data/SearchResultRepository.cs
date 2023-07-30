using System.Collections.Generic;

namespace WebApplication2.Data
{
    public class SearchResultRepository
    {
        private readonly List<SearchResult> _searchResults = new List<SearchResult>();
        private int _nextId = 1;

        public SearchResult Add(SearchResult searchResult)
        {
            searchResult.Id = _nextId++;
            _searchResults.Add(searchResult);
            return searchResult;
        }

        public IEnumerable<SearchResult> GetAll()
        {
            return _searchResults;
        }

        public SearchResult GetById(int id)
        {
            return _searchResults.Find(r => r.Id == id);
        }

        public void Remove(int id)
        {
            var searchResult = _searchResults.Find(r => r.Id == id);
            if (searchResult != null)
            {
                _searchResults.Remove(searchResult);
            }
        }
    }
}
