namespace ArticlesAnalyzer.Core.Domain;

public class ArticleParserStub : IArticleParser
{
    public async Task<Article> ParseArticle(string url)
    {
        ArgumentNullException.ThrowIfNull(url);
        return new Article("difficulty", "timeToRead", "title", new string[] { "body" }, url, "publishDate");
    }
}