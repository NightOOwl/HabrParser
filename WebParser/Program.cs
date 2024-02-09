namespace WebParser;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var httpClient = new HttpClient();
        IArticleParser parser = new HabrArticleHtmlAgilityParser(httpClient);
        await Console.Out.WriteLineAsync("Введите url статьи с Хабра для парсинга: ");
        string url = Console.ReadLine();
        //валидация url
        var article = await parser.ParseArticle(url);
        await ArticleWriter.WriteArticleAsync(article);
        // ДЗ: Еще одну реализацию IArticleParser, которая будет парсить через Playwright
    }
}