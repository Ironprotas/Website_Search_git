namespace WebApplication2.Data
{
    public class SearchResult
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Author { get; set; }
        public int StargazersCount { get; set; }
        public int WatchersCount { get; set; }
        public string HtmlUrl { get; set; }

        public List<Project> Projects { get; set; }
        public string SearchString { get; set; }
    }
}
