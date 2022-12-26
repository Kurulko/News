using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models;

public class Article
{
    public long Id { get; set; }
    public string? Resource { get; set; }
    public string? Header { get; set; }
    public DateTime Time { get; set; }
    public string? Text { get; set; }
    public string? Link { get; set; }

    public long? AuthorId { get; set; }
    public Author? Author { get; set; }
}
