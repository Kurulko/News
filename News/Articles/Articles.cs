using HtmlAgilityPack;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Articles;

public abstract class Articles
{
    protected HtmlDocument htmlMain;
    protected readonly string resource;
    protected readonly string xpathToArticleLink;
    public Articles(string resource, string xpathToArticleLink)
    {
        this.resource = resource;
        this.xpathToArticleLink = xpathToArticleLink;
        htmlMain = new HtmlWeb().Load(resource);
    }

    protected IEnumerable<HtmlNode> GetNodes(string xpath)
    {
        IEnumerable<HtmlNode> nodes = htmlMain.DocumentNode.SelectNodes(xpath);

        if (nodes is null)
        {
            IEnumerable<string?> links = GetLinks(xpathToArticleLink);
            nodes = links.Select(link => new HtmlWeb().Load(link).DocumentNode.SelectSingleNode(xpath));
        }

        return nodes!;
    }
    protected IList<string?> GetTextOfNodes(string xpath)
        => GetNodes(xpath).Select(n => n?.InnerText).ToList();


    protected virtual IList<string?> GetHeaders(string xpath)
        => GetTextOfNodes(xpath);

    protected virtual IList<string?> GetAuthorsName(string xpath)
        => GetTextOfNodes(xpath);

    protected virtual IList<string?> GetAuthorsLink(string xpath)
        => GetNodes(xpath).Select(n => n?.Attributes["href"]?.Value).ToList();

    protected virtual IList<DateTime> GetTimes(string xpath)
    {
        IEnumerable<string?> datesStr = GetTextOfNodes(xpath);

        IList<DateTime> dates = new List<DateTime>();
        foreach (string? dateStr in datesStr)
        {
            bool res = DateTime.TryParse(dateStr!, out DateTime date);
            dates.Add(date);
        }

        return dates;
    }

    protected virtual IList<string?> GetTexts(string xpath)
        => GetTextOfNodes(xpath);

    protected virtual IList<string?> GetLinks(string xpath)
        => GetNodes(xpath).Select(n => n?.Attributes["href"]?.Value).ToList();

    protected IEnumerable<Article> Build(string headerPath, string authorNamePath, string authorLinkPath, string timePath, string textPath, string linkPath)
    {
        IList<string?> headers = GetHeaders(headerPath), links = GetLinks(linkPath);
        IList<string?> authorsName = GetAuthorsName(authorNamePath), authorsLink = GetAuthorsLink(authorLinkPath), texts = GetTexts(textPath);
        IList<DateTime> times = GetTimes(timePath);

        IList<Article> articles = new List<Article>();

        for (int i = 0; i < headers.Count(); i++)
        {
            Author author = new() { Name = authorsName[i]?.Trim()!, Link = authorsLink[i]! };
            Article article = new() { Resource = resource, Header = headers[i]?.Trim()!, Link = links[i], Author = author, Text = texts[i], Time = times[i] };
            articles.Add(article);
        }

        return articles;
    }

    public abstract IEnumerable<Article> GetAllArticles();
    public virtual IEnumerable<Article> GetLatestArticles(TimeSpan time)
        => GetAllArticles().Where(art => art.Time > DateTime.Now.Add(-time));
}
