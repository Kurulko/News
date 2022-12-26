using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models;

public class Author
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Link { get; set; }

    public IEnumerable<Article>? Articles { get; set; }
}
