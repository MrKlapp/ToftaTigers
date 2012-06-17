using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diary.Models;
using Diary.Models.Files;

namespace Diary.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _root = ConfigurationManager.AppSettings["UploadUrl"];
        
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Details(BasePath b)
        {
            if (b.Date != null) {
                return View(DayStory.GetSingleDay(b));
            }
            return null;
        }
        
        public ActionResult Upload(DayStory d)
        {
            return View("Upload", d);
        }

        public ActionResult UploadEdit(string category, string id)
        {
            var b = new BasePath() { Date = id, Category = category};
            var d = DayStory.GetSingleDay(b);
            //var day = DayStory.GetSingleDay(new BasePath(){Category = d.Category, Date = d.Day, Path = ConfigurationManager.AppSettings["UploadUrl"]}) ?? new DayStory {Day = d.Day};
            return View("Upload", d);
        }

        public ActionResult Day(BasePath b)
        {
            var d = new DayStory();
            if (b.Date == null) {
                d.Day = DateTime.Now.ToShortDateString();
                //d.Category = "main";
            }
            else {
                d = DayStory.GetSingleDay(b);
            }

            return View("Day", d);
        }
        
        public ActionResult DayEdit(string category, string id)
        {
            var b = new BasePath(){ Category = category, Date = id};
            var d = DayStory.GetSingleDay(b);
            return View("Day", d);
        }
        
        public ActionResult ReplySave(string category, string id, FormCollection collection)
        {
            var comment = collection[0];
            var from = collection[1];
            var r = new Reply();
            r.Create(comment, from, id, category);
            
            return Redirect("/Home/Index");
        }

        public ActionResult Adm(string id)
        {
            var list = new List<DayStory>();
            var dirs = new DirectoryInfo(_root);
            var fldrs = dirs.GetDirectories();
            foreach (var dirInfo in fldrs) {
                var d = new DayStory().GetAllDays(dirInfo.Name);
                list.AddRange(d);
            }
            return View(list);
        }
    }
}
