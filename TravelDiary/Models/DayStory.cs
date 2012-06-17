using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Diary.Models.Files;

namespace Diary.Models
{
    public class DayStory
    {

        public string Day { get; set; }
        public string Header { get; set; }
        public string Img { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        
        public List<Image> Images  { get; set; }
        public List<Reply> Comments  { get; set; }

        //toDo: Exclude data to ext ilayer and dal-class

        public static DayStory GetSingleDay(BasePath b)
        {
            try {
                var d = new DayStory();
                var date = b.Date;
                d.Day = date;
                string sPath = String.Concat(b.Path, b.Category, "\\", b.Date + "\\");
                d.Text = TextFile.ReadFileWithoutEncoding(sPath + "main.txt");
                d.Header = TextFile.ReadFileWithoutEncoding(sPath + "header.txt");
                d.Category = TextFile.ReadFileWithoutEncoding(sPath + "category.txt");
                d.Comments = Reply.GetReplies(sPath);
                if (Image.GetAllImages(b).Count > 0) {
                    d.Img = Image.GetAllImages(b)[0].Url;
                    d.Images = Image.GetAllImages(b);
                }
                return d;
                //diaryText = diaryText.Replace("\r\n", "<br>").Replace("\r", "<br>").Replace("\n", "<br>");
            }
            catch (Exception ex) {
                return null;
                //throw ex;
            }
        }

        public List<DayStory> GetAllDays(string cat)
        {
            var sPath = ConfigurationManager.AppSettings["UploadUrl"]  + cat + "\\";
            
            var stories = new List<DayStory>();
            var dirs = new DirectoryInfo(sPath);
            var comparer = new myReverserClass();
            DirectoryInfo[] fldrs = dirs.GetDirectories();
            Array.Sort(fldrs, comparer);
            foreach (var dirInfo in fldrs) {
                var b = new BasePath(){Date = dirInfo.Name, Category = cat};
                var d = GetSingleDay(b);
                    stories.Add(d);
            }
            return stories;
        }

        public static void Save(DayStory d)
        {
            if (d.Category.Length == 0)
                d.Category = "main";

            var sPath = ConfigurationManager.AppSettings["UploadUrl"] + d.Category + "\\" + d.Day;
            Directory.CreateDirectory(sPath);
            Directory.CreateDirectory(sPath+"\\thumbnails");
            
            TextFile.CreateFile(sPath + "\\main.txt");
            TextFile.AppendToFile(sPath + "\\main.txt", d.Text);
            
            TextFile.CreateFile(sPath + "\\header.txt");
            TextFile.AppendToFile(sPath + "\\header.txt", d.Header);
            
            TextFile.CreateFile(sPath + "\\category.txt");
            TextFile.AppendToFile(sPath + "\\category.txt", d.Category);
        }
       
    }

      public class myReverserClass : IComparer  {
           public int Compare(object o1, object o2)
            {
            var info1 = (DirectoryInfo) o1;
            var info2 = (DirectoryInfo) o2;

            return string.Compare(info2.Name,info1.Name);
            }
        }

    
}