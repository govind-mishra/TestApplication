using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApplication.ViewModels
{
    public class RssFeedViewModel
    {
        [Url]
        public string RSSFeedURl { get; set; }

        public IFormFile CSVFile { get; set; }
    }
}
