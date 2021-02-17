using CodeHollow.FeedReader;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using TestApplication.Models.ModelClasses;

namespace TestApplication.Models.ModelIRepositories
{
    public interface IRssFeedRepository
    {
        IEnumerable<FeedItem> getFeedItemsFromURL(string url);

        string getFeedURL(string url);

        IEnumerable<FeedArticlesAndDetail> ConvertAResult(IList<FeedItem> items);

        StringBuilder ReadCSV(IFormFile csvfile);


        string CreateCSVFileInRoot(string rootPath, string fileName, StringBuilder csv);
    }
}
