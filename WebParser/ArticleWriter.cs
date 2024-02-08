using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser
{
    public static class ArticleWriter
    {
        public static async Task WriteArticleAsync(Article article)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string folderPath = Path.Combine(desktopPath, "Статьи с Хабра");

            string sanitizedTitle = GetSafeFilename(article.Title);
            string filePath = Path.Combine(folderPath, $"{sanitizedTitle}.txt");

            if (!Directory.Exists(folderPath))
            {              
                Directory.CreateDirectory(folderPath);
                await Console.Out.WriteLineAsync("Папка успешно создана.");
            }
            if (!File.Exists(filePath))
            {                
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    await writer.WriteLineAsync($"URL: {article.Url}");
                    await writer.WriteLineAsync($"Title: {article.Title}");                    
                    await writer.WriteLineAsync($"Publish Date: {article.PublishDate}");
                    await writer.WriteLineAsync($"Difficulty: {article.Difficulty}");
                    await writer.WriteLineAsync($"Time to Read: {article.TimeToRead}");
                    await writer.WriteLineAsync("Body:");
                    foreach (var paragraph in article.Body)
                    {
                        await writer.WriteLineAsync(paragraph);
                    }
                    Console.WriteLine($"Файл '{article.Title}.txt' успешно создан и данные записаны.");
                }
            }     
        }
        private static string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
