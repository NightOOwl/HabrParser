using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser
{
    public static class Parser
    {
        /// <summary>
        /// Получает HTML разметку статьи Хабр по переданному url адресу
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Возвращает объект HtmlDocument</returns>
        /// <exception cref="InvalidOperationException"></exception>
        
        public static async Task<HtmlDocument> GetHTML( string url)
        {
            using (HttpClient client = new HttpClient())
            {              
                HttpResponseMessage response = await client.GetAsync(url);
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
        }

        /// <summary>
        /// Формирует экземпляр Article из переданного HtmlDocument 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="url"></param>
        /// <returns>Возвращает экземпляр статьи Хабр</returns>
        /// <exception cref="Exception"></exception>
        
        public static  Article CreateArticle(HtmlDocument html, string url)
        {
            Article article;

            string publishDate;
            string title;
            string difficulty;
            string timeToRead;
            string[] body;

            try
            {
                 title = html.DocumentNode
                .SelectSingleNode("//h1")
                .InnerText.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }

            try
            {
                 difficulty = html.DocumentNode
                .SelectSingleNode(".//div[@class='tm-article-snippet__stats']//span[@class='tm-article-complexity__label']")
                .InnerText.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                timeToRead = html.DocumentNode
                    .SelectSingleNode(".//div[@class='tm-article-snippet__stats']//span[@class='tm-article-reading-time__label']")
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
                    .Where(str=>!string.IsNullOrWhiteSpace(str))
                    .Select(str=>str.Trim())
                    .ToArray();
            }
            catch ( Exception ex)
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
            return article = new Article(difficulty, timeToRead, title, body, url,publishDate);
        }
    }
}
