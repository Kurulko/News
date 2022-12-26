using News.Db;
using News.Articles;
using News.Telegram;
using News.Models;

TimeSpan time = new(0, 10, 0);
var news = new List<Articles>() { new TsnArticles(), new UnianArticles() };
PeriodicallySendToTChatAndAddToDbArticles(time, news);
Console.ReadLine();



static void AddArticlesToDb(IEnumerable<Article> articles)
{
    using NewsContext db = new();
    foreach (Article article in articles)
    {
        Author? author = article.Author;
        if (author is not null)
        {
            var auth = db.Authors.FirstOrDefault(a => a.Link == author.Link && a.Name == author.Name);
            if (auth is not null)
            {
                article.AuthorId = auth!.Id;
                article.Author = null;
            }
            else if(author.Name is null)
                article.Author = null;
        }
        db.Articles.Add(article);
        db.SaveChanges();
    }
}

static async Task SendArticlesToTChatAsync(IEnumerable<Article> articles)
{
    const string token = Environment.GetEnvironmentVariable("MY_TELEGRAM_CHAT_NEWS");
    TelegramNews tNews = new(token);
    const string chatId = Environment.GetEnvironmentVariable("MY_TELEGRAM_TOKEN");
    await tNews.SendTextMessagesAsync(chatId, articles);
}

static async Task SendToTChatAndAddToDbArticlesAsync(IEnumerable<Article> articles)
{
    AddArticlesToDb(articles);
    await SendArticlesToTChatAsync(articles);
}

static void PeriodicallySendToTChatAndAddToDbArticles(TimeSpan time, IEnumerable<Articles> news)
{
    TimerCallback tm = async (object state) =>
    {
        foreach (Articles articles in news)
        {
            var latestArticles = articles.GetLatestArticles(time);
            if ((latestArticles?.Count() ?? 0) != 0)
            {
                await SendToTChatAndAddToDbArticlesAsync(latestArticles!);
                global::System.Console.WriteLine("Not empty");
            }
            else
                global::System.Console.WriteLine("Empty");
        }
    };
    Timer timer = new(tm, null, 0, (int)time.TotalMilliseconds);
}
