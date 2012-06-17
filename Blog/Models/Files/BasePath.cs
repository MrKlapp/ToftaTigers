using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Diary.Models.Files
{
    
    public class BasePath
    {
        public string Path { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
     

    public BasePath()
    {
        Path = ConfigurationManager.AppSettings["UploadUrl"];
    }
    }


}