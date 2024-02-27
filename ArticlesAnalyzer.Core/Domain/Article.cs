namespace ArticlesAnalyzer.Core.Domain
{
    public class Article
    {
        public string Url { get; set; }
        public string PublishDate { get; set; }
        public string Difficulty {  get; set; }
        public string TimeToRead { get; set; }
        public string Title { get; set; }
        public string[] Body { get; set; }
        public Article( string difficulty, string timeToRead, string title, string[] body, string url , string publishDate)
        {
            Difficulty = difficulty;
            TimeToRead = timeToRead;
            Title = title;
            Body = body;
            Url = url;
            PublishDate = publishDate;
        }
        
    }
}
