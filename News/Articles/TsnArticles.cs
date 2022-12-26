using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using News.Models;


namespace News.Articles;

public class TsnArticles : Articles
{
    public TsnArticles() : base("https://tsn.ua/", "//div[@data-sidebar='article']//article//a[@class='c-card__link']") {}

    protected override IList<DateTime> GetTimes(string xpath)
    {
        IEnumerable<string?> datesStr = GetNodes(xpath).Select(n => n?.Attributes["datetime"]?.Value);

        IList<DateTime> dates = new List<DateTime>();
        foreach (string? dateStr in datesStr)
        {
            bool res = DateTime.TryParse(dateStr!, out DateTime date);
            dates.Add(date);
        }

        return dates;
    }

    public override IEnumerable<Article> GetAllArticles()
    {
        string timeXPath = "//div[@data-sidebar='article']//article//time[@datetime]";

        string footerOfArticle = "//footer[@class='c-card__foot']";
        string authorNameXPath = $"{footerOfArticle}//span[@class='c-bar__spacer-l sr-only--sdxl']";
        string authorLinkXPath = $"{footerOfArticle}//a[@class='c-bar__label']";

        string textXPath = "//div[@class='c-article__body']//div[@data-content]";

        return Build(xpathToArticleLink, authorNameXPath, authorLinkXPath, timeXPath ,textXPath, xpathToArticleLink);
    }
}
