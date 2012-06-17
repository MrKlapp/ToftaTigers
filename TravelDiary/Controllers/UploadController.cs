using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Diary.Models;
using Diary.Models.Files;
using TravelDiary.Models.Files;
using Image = System.Drawing.Image;

namespace Models.Controllers
{
    public class UploadController : Controller
    {
        readonly string _imageRoot = ConfigurationManager.AppSettings["UploadUrl"];

        public ActionResult SaveComments(string category, string id, FormCollection collection)
        {
            //var path = Path.Combine(_imageRoot, b.Path + "\\" + id + "");
            var path = string.Format("{0}{1}\\{2}\\", _imageRoot, category, id);
            foreach (var item in collection.AllKeys) {
                var text = collection[item];
                var fileUrl = path + item + ".txt";
                UpdateCommentFile(fileUrl, text);

            }
            return Redirect("/Home/UploadEdit/"+ category + "/" + id);
        }

        public ActionResult Day(BasePath b)
            {
                var d = DayStory.GetSingleDay(b) ?? new DayStory {Day = b.Date};
                return View("Day", d);
            }

        public void UploadFile()
        {
            Image thumbnailImage = null;
            Image originalImage = null;
            Bitmap finalImage = null;
            Graphics graphic = null;
            MemoryStream ms = null;
            
            var c = Request["category"];
            var id = Request["day"];

            if (id.Length == 0) {
                //Redirect("/Home/Day/"+ DateTime.Now.ToShortDateString());
            }

            try
            {
                // Get the data
                var jpegImageUpload = Request.Files["Filedata"];
                var newFolder = id;

                var newPath = Path.Combine(_imageRoot, c, newFolder);
                Directory.CreateDirectory(newPath);

                var newPathThumbs = Path.Combine(newPath, c, "thumbnails");
                Directory.CreateDirectory(newPathThumbs);

                newPath = string.Format("{0}\\", newPath);
                newPathThumbs = string.Format("{0}thumbnails\\", newPath);

                var imgName = jpegImageUpload.FileName;
                var imgPath = newPath + imgName;
                var imgPathThumb = newPathThumbs + imgName;

                // Retrieve the uploaded image
                originalImage = Image.FromStream(jpegImageUpload.InputStream);

                // Calculate the new width and height
                var width = originalImage.Width;
                var height = originalImage.Height;
                const int targetWidth = 100;
                const int targetHeight = 100;
                int newWidth, newHeight;

                const float targetRatio = targetWidth/(float) targetHeight;
                var imageRatio = width/(float) height;

                if (targetRatio > imageRatio)
                {
                    newHeight = targetHeight;
                    newWidth = (int) Math.Floor(imageRatio*targetHeight);
                }
                else
                {
                    newHeight = (int) Math.Floor(targetWidth/imageRatio);
                    newWidth = targetWidth;
                }

                newWidth = newWidth > targetWidth ? targetWidth : newWidth;
                newHeight = newHeight > targetHeight ? targetHeight : newHeight;


                // Create the thumbnail

                thumbnailImage = new Bitmap(targetWidth, targetHeight);
                graphic = Graphics.FromImage(thumbnailImage);
                graphic.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, targetWidth, targetHeight));
                var pasteX = (targetWidth - newWidth)/2;
                var pasteY = (targetHeight - newHeight)/2;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic; /* new way */
                graphic.DrawImage(originalImage, pasteX, pasteY, newWidth, newHeight);

                // Store the data in my custom Thumbnail object
			    ms = new MemoryStream();
                string thumbnail_id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                
                thumbnailImage.Save(imgPathThumb, ImageFormat.Jpeg);
                thumbnailImage.Save(ms, ImageFormat.Jpeg);
			    var thumb = new Thumbnail(thumbnail_id, ms.GetBuffer());
                List<Thumbnail> thumbnails = Session["file_info"] as List<Thumbnail>;
			    if (thumbnails == null)
			    {
				    thumbnails = new List<Thumbnail>();
				    Session["file_info"] = thumbnails;
			    }
			    thumbnails.Add(thumb);


                if (width > height)
                {
                    newWidth = 600;
                    newHeight = 400;
                }
                else
                {
                    newWidth = 400;
                    newHeight = 600;
                }

                finalImage = new Bitmap(newWidth, newHeight);
                graphic = Graphics.FromImage(finalImage);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic; /* new way */
                graphic.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                finalImage.Save(imgPath, ImageFormat.Jpeg);
                TextFile.CreateFile(imgPath + ".txt");

                Response.StatusCode = 200;
			    Response.Write(thumbnail_id);

            }
            catch(Exception ex)
            {
                // If any kind of error occurs return a 500 Internal Server error
                Response.StatusCode = 500;
                Response.Write("An error occured");
                Response.End();
                //throw ex;
            }
            finally
            {
                 //Clean up
                if (finalImage != null) finalImage.Dispose();
                if (graphic != null) graphic.Dispose();
                if (originalImage != null) originalImage.Dispose();
                if (thumbnailImage != null) thumbnailImage.Dispose();
                if (ms != null) ms.Close();
                Response.End();
            }

            //return Redirect("/Home/Upload/"+c+"/"+id);
        }

        private void UpdateCommentFile(string file, string text)
        {
            file = file.Replace("_____", ".");
            TextFile.DeleteFile(file);
            TextFile.CreateFile(file);
            TextFile.AppendToFile(file, text);
        }

        [WebMethod]
        public void DeleteDay(string day, string name, string category)
        {
            var path = string.Format("{0}{1}\\{2}\\", _imageRoot, category, day);
            name = name.Replace("_____", ".");
            TextFile.DeleteFile(path + name + ".txt");
            System.IO.File.Delete(path + name);
            System.IO.File.Delete(path + "thumbnails\\" + name);
        }


    }
}
