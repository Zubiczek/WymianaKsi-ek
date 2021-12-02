using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Functions
{
    public class SaveImg : ISaveImg
    {
        public string SaveImage(IFormFile image)
        {
            if (image != null)
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                return imageName;
            }
            return null;
        }
    }
}
