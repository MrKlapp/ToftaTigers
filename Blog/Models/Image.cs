using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diary.Models.Files;

namespace Diary.Models
{

    public class Image
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Thumb { get; set; }
        public string Comment { get; set; }

        public static List<Image> GetAllImages(BasePath b)
        {
            var l = new List<Image>();
            var dir = new DirectoryInfo(String.Concat(b.Path, b.Category, "\\", b.Date + "\\"));
            foreach (var fileInfo in dir.GetFiles())
            {
                if (fileInfo.Extension.ToLower() != ".jpg") continue;

                var imageComment = TextFile.ReadFileWithoutEncoding(fileInfo.FullName + ".txt");

                var img = new Image
                          {
                              Name = fileInfo.Name.Replace(".","_____"),
                                  Url = "/Upload/" + b.Category + "/" + b.Date + "/" + fileInfo.Name,
                                  Thumb = "/Upload/" + b.Category + "/" + b.Date + "/thumbnails/" + fileInfo.Name,
                                  Comment = imageComment
                              };
                l.Add(img);
            }
            return l;
        }

          //public static void Delete(string name)
          //{
          //    name = name.Replace("___", ".");
          //    var imageRoot = AppDomain.CurrentDomain.BaseDirectory + "Upload\\";
          //    TextFile.DeleteFile(imageRoot + name + ".txt");
          //    File.Delete(imageRoot + name);
          //    File.Delete(imageRoot + "thumbnails\\" + name);
          //}

    }
}