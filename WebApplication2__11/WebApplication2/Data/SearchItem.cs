using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data
{
    public class SearchItem
    {
        [Key]
        public int Id { get; set; }

        public string SearchQuery { get; set; }

        public string ResultJson { get; set; }

        public string? ProjectName { get; set; }

        public string? Author { get; set; }

        public int StargazersCount { get; set; }

        public int WatchersCount { get; set; }
        public string HtmlUrl { get; set; }
    }
}