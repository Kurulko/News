using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Articles;

public class UnianArticles : Articles
{
    public UnianArticles() : base("https://www.unian.net/", "//div[@data-vr-zone='all_right_news']//a[@data-vr-contentbox]") { }

    public override IEnumerable<Article> GetAllArticles()
    {
        string startArticle = "//div[@class='article']";
        string footerOfArticle = $"{startArticle}//div[@class='article__info']";

        string timeXPath = $"{footerOfArticle}//div[@class='article__info-item time']";
        string authorXPath = $"{footerOfArticle}//a[@class='article__author-name']";

        string textXPath = $"{startArticle}//div[@class='article-text']//div[@data-content]";

        return Build(xpathToArticleLink, authorXPath, authorXPath, timeXPath, textXPath, xpathToArticleLink);
    }
}
