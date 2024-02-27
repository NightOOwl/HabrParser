namespace ArticlesAnalyzer.Core.Domain;

public interface IArticleParser
{
    /// <summary>
    /// Формирует экземпляр <see cref="Article"/>
    /// </summary>
    Task<Article> ParseArticle(string url);
}