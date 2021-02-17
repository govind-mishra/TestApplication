using CodeHollow.FeedReader;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestApplication.Models.ModelClasses;
using TestApplication.Models.ModelIRepositories;

namespace TestApplication.Models.ModelRepositories
{
    public class RssFeedRepository : IRssFeedRepository
    {

        public IEnumerable<FeedArticlesAndDetail> ConvertAResult(IList<FeedItem> items)
        {
            List<FeedArticlesAndDetail> articlesAndDetails = new List<FeedArticlesAndDetail>();
            foreach (var item in items)
            {
                articlesAndDetails.Add(new FeedArticlesAndDetail()
                {
                    ArticleBody = item.Description.Substring(0, 10),
                    ArticleTitle = item.Title.Substring(0, 10)
                });
            }

            return articlesAndDetails;

        }

        public IEnumerable<FeedItem> getFeedItemsFromURL(string url)
        {
            string feedUrl = getFeedURL(url);
            IEnumerable<FeedItem> feedItems = null;
            var readerTask = FeedReader.ReadAsync(feedUrl);
            readerTask.ConfigureAwait(false);
            feedItems =  readerTask.Result.Items;
            return feedItems;
        }

        public string getFeedURL(string url)
        {
            var urlsTask = FeedReader.GetFeedUrlsFromUrlAsync(url);
            var urls = urlsTask.Result;   
            string feedUrl = url;
            if (urls == null || urls.Count() < 1)
                return url;
            else 
                return urls.First().Url; //if 1 or 2 urls, then its usually a feed and a comments feed, so take the first per default
        }

        public StringBuilder ReadCSV(IFormFile csvfile)
        {
            var csv = new StringBuilder();
            using (var sreader = new StreamReader(csvfile.OpenReadStream()))
            {
                string[] headers = sreader.ReadLine().Split(',');     //Title
                while (!sreader.EndOfStream)                          //get all the content in rows 
                {
                    string[] rows = sreader.ReadLine().Split(',');
                    var temprow = "";
                    Array.ForEach(rows, row =>
                    {
                        row = row.Replace("uzabase", "Uzabase, Inc.");
                        temprow += $"{row},";
                    });
                    csv.AppendLine(temprow.TrimEnd(','));
                }

            }

            return csv;
        }

        public string CreateCSVFileInRoot(string rootPath, string fileName, StringBuilder csv)
        {
            
              string CSVFolderFullPath = Path.Combine(rootPath, "CSVs");
                string uniqueFileName = $"{Guid.NewGuid().ToString()}_{fileName}";
            string convertedTextFile = Path.ChangeExtension(uniqueFileName, ".txt");
                string saveFile = Path.Combine(CSVFolderFullPath, convertedTextFile);
            File.WriteAllText(saveFile, csv.ToString());
            return convertedTextFile;
        }
    }
}
