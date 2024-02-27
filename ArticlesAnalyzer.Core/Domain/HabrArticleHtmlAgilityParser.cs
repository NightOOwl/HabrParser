using HtmlAgilityPack;

namespace ArticlesAnalyzer.Core.Domain;

public class HabrArticleHtmlAgilityParser : IArticleParser
{
    private readonly HttpClient _client;

    public HabrArticleHtmlAgilityParser()
    {
        _client = new HttpClient();
    }
    
    public async Task<Article> ParseArticle(string url)
    {
        ArgumentNullException.ThrowIfNull(url);
        HtmlDocument html = await GetHTML(url);
        return ParseArticleInternal(html, url);
    }

    /// <summary>
    /// Получает HTML разметку статьи Хабр по переданному url адресу
    /// </summary>
    /// <returns>Возвращает объект HtmlDocument</returns>
    /// <exception cref="InvalidOperationException">...</exception>
    private async Task<HtmlDocument> GetHTML(string url)
    {
        ArgumentNullException.ThrowIfNull(url);
            
        HttpResponseMessage response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var html = new HtmlDocument();
            html.LoadHtml(await response.Content.ReadAsStringAsync());
            return html;
        }
        else
        {
            throw new InvalidOperationException($"Ошибка: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }

    private Article ParseArticleInternal(HtmlDocument html, string url)
    {
        ArgumentNullException.ThrowIfNull(html);
        ArgumentNullException.ThrowIfNull(url);
        Article article;

        string publishDate;
        string title;
        string difficulty;
        string timeToRead;
        string[] body;

        HtmlNode? h1 = html.DocumentNode.SelectSingleNode("//h1"); //NRT
        if(h1 is null)
        {
            throw new InvalidOperationException("Не удалось получить заголовок статьи (тег h1)");
            // TagNotFoundException ДЗ
        }
            
        title = h1.InnerText.Trim();

        try
        {
            difficulty = html.DocumentNode
                .SelectSingleNode(
                    ".//div[@class='tm-article-snippet__stats']//span[@class='tm-article-complexity__label']")
                .InnerText.Trim();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        try
        {
            timeToRead = html.DocumentNode
                .SelectSingleNode(
                    ".//div[@class='tm-article-snippet__stats']//span[@class='tm-article-reading-time__label']")
                .InnerText.Trim();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        try
        {
            body = html.DocumentNode
                .SelectNodes(".//div[@class='tm-article-body']//p")
                .Select(p => p.InnerText)
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .Select(str => str.Trim())
                .ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        try
        {
            publishDate = html.DocumentNode
                .SelectSingleNode(".//span[@class='tm-article-datetime-published']//time")
                .InnerText.Trim();
        }
        catch (Exception)
        {
            throw;
        }

        return article = new Article(difficulty, timeToRead, title, body, url, publishDate);
    }
}