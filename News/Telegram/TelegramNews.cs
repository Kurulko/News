using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace News.Telegram;

public class TelegramNews
{
    TelegramBotClient botClient;
    public TelegramNews(string token)
        => botClient = new(token);
    public TelegramNews(TelegramBotClient botClient)
        => this.botClient = botClient;

    public async Task<Message> SendTextMessageAsync(ChatId chatId, Article article)
    {
        try
        {
            string text = $"<strong>{article.Header}</strong> {article.Text} {article.Author?.Name}";
            return await botClient.SendTextMessageAsync(chatId, text, ParseMode.Html);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return default!;
    }

    public async Task<IEnumerable<Message>> SendTextMessagesAsync(ChatId chatId, IEnumerable<Article> articles)
    {
        List<Message> messages = new();

        foreach (Article article in articles)
            messages.Add(await SendTextMessageAsync(chatId, article));

        return messages;
    }
}
