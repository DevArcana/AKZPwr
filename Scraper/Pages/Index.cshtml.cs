using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Scraper.Pages
{
  public class Course
  {
    public string Code { get; set; }
    public string Group { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public string Lecturer { get; set; }
    public string ZZU { get; set; }
    public string IsStationary { get; set; }
  }
  
  public class IndexModel : PageModel
  {
    private readonly HttpClient _http;
    
    public IEnumerable<Course> Entries { get; set; }

    public IndexModel(HttpClient http)
    {
      _http = http;
    }

    public async Task OnGet()
    {
      var config = Configuration.Default.WithDefaultLoader();
      var context = BrowsingContext.New(config);
      var document = await context.OpenAsync("http://akz.pwr.wroc.pl/katalog_zap.html");

      var table = document.GetElementsByTagName("tbody")[0];

      var entries = new List<Course>();
      foreach (var row in table.Children)
      {
        var columns = row.Children;

        if (columns[8].InnerHtml.StartsWith("I "))
        {
          var entry = new Course()
          {
            Code = columns[0].InnerHtml,
            Group = columns[1].InnerHtml,
            Name = columns[2].InnerHtml,
            Date = columns[3].InnerHtml,
            Lecturer = columns[4].InnerHtml,
            ZZU = columns[6].InnerHtml,
            IsStationary = columns[7].InnerHtml
          };

          if (!entry.Name.Contains("Jezyk") && !entry.Code.StartsWith("WFW"))
          {
            entries.Add(entry);
          }
        }
      }

      Entries = entries;
    }
  }
}