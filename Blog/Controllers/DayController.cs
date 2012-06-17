using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diary.Models;
using Diary.Models.Files;
using RiaLibrary.Web;

namespace Diary.Controllers
{
    public class DayController : Controller
    {
        private readonly string _root = ConfigurationManager.AppSettings["UploadUrl"];
        
       [HttpPost, ValidateInput(false)]
        public ActionResult  SaveDay(FormCollection collection)
        {
            var d = new DayStory {Day = collection[0], Header = collection[1], Text = collection[3], Category = collection[2]};

           try {
                DayStory.Save(d);
               return Redirect("/Home/UploadEdit/" + d.Category + "/" + d.Day);

           }
            catch(Exception ex) {
                throw ex;
            }
        }
        
        public ActionResult Refresh(BasePath b)
        {
           var d = DayStory.GetSingleDay(b);
           return View("Upload", d);
        }

        public void Delete(string category, string id)
        {
            try {
                var root = _root + category + "\\" + id;
                Directory.Delete(root, true);
                Directory.Delete(root);
            }
            catch(Exception ex) {
                
            }
        }


    }
}
