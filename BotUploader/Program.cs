using System;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.IO;

namespace BotUploader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var botClient = new TelegramBotClient("YYYYYYYYYYY:XXXXXXXXXXXXXXXXXXXXXXXXXX"); //your http api
            botClient.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        async private static Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Photo != null)
            {
                await client.SendTextMessageAsync(message.Chat.Id, "Send a document.");     
            }
            if (message.Document != null)
            {
                string savingName = message.Date.AddHours(3).ToString();
                var fileId = message.Document.FileId;
                var fileInfo = await client.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

                string destinationFilePath = $@"D:\TelegramCloud\{savingName.Replace(":", "-").Replace(".", "_") 
                    + message.Document.FileName.Substring(message.Document.FileName.IndexOf('.'))}"; //you need to create a folder, where ur photos will collect
                try
                {
                    await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                    await client.DownloadFileAsync(filePath, fileStream);
                    fileStream.Close();
                    Console.WriteLine("File saved successfully " + DateTime.Now);
                    await client.SendTextMessageAsync(message.Chat.Id, "File saved!");
                }
                catch (Exception ex)
                { Console.WriteLine(ex);
                    Console.WriteLine("Error of saving: " + savingName);  
                };
                return;
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}