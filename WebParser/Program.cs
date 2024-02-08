using System.Threading.Channels;

namespace WebParser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Console.Out.WriteLineAsync("Введите url статьи с Хабра для парсинга: ");
            string url = Console.ReadLine();
            var article = Parser.CreateArticle(await Parser.GetHTML(url),url);            
            await ArticleWriter.WriteArticleAsync(article);
        }      
    }
}
