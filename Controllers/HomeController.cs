using CsvHelper;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using TestApplication.Models.ModelClasses;
using TestApplication.Models.ModelIRepositories;
using TestApplication.ViewModels;

namespace TestApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRssFeedRepository _rssFeedRepository;
        private readonly HostingEnvironment hostingEnvironment;
        public HomeController(IRssFeedRepository rssFeedRepository, HostingEnvironment hostingEnvironment)
        {
            _rssFeedRepository = rssFeedRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        public ViewResult Index()
        {
            return View();
        }

        public IActionResult OnPostDownloadFile(string filename)
        {
            return File($"~/CSVs/{filename}", "application/octet-stream",
                        filename);
        }

        [HttpPost]
        public  ViewResult Index(RssFeedViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View();
            if (viewModel.CSVFile != null)
            {
                string wwwRootPath = hostingEnvironment.WebRootPath;
               var csvText =  _rssFeedRepository.ReadCSV(viewModel.CSVFile);
               string savedFile =  _rssFeedRepository.CreateCSVFileInRoot(wwwRootPath, viewModel.CSVFile.FileName, csvText);
               ViewBag.SavedFile = savedFile;
                return View("ConvertBResult");
            }
                string url = viewModel.RSSFeedURl;
            var feedItems = _rssFeedRepository.getFeedItemsFromURL(url);
            IEnumerable<FeedArticlesAndDetail> feedArticlesAndDetails = _rssFeedRepository.ConvertAResult(feedItems.ToList());
            return View("ConvertAResult",  feedArticlesAndDetails );
        }
    }
}
