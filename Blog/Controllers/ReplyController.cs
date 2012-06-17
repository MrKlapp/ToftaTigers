using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diary.Models;

namespace Diary.Controllers
{
    public class ReplyController : Controller
    {
        //
        // GET: /Reply/

        public ActionResult Index()
        {
            return View();
        }

        public object Save(FormCollection collection)
        {
            var comment = collection[0];
            var from = collection[1];
            var day = collection[2];
            var category = collection[3];
            var r = new Reply();
            r.Create(comment, from, day, category);
            return Redirect("/");
        }

        //public object Save(string comment, string from, string day)
        //{
        //    var r = new Reply();
        //    r.Create(comment, from, day);
        //    return Redirect("/");
        //}

    }
}
