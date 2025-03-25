using business_logic.Entities;
using business_logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace business_logic.Services
{
    public class ImageHulk : IImageHulk
    {
        private readonly IConfiguration _configuration;

        public ImageHulk(IConfiguration conf)
        {
            _configuration = conf;
        }

        public bool Delete(string fileName)
        {
            try
            {
                var dir = _configuration["ImageFolder"];
                var sizes = _configuration["ImageSizes"].Split(",")
                    .Select(x => int.Parse(x));
                foreach (var size in sizes)
                {
                    string dirSave = Path.Combine(Directory.GetCurrentDirectory(),
                        dir, $"{size}_{fileName}");

                    if (File.Exists(dirSave))
                        File.Delete(dirSave);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> Save(IFormFile image)
        {
            string imageName = String.Empty;

            using (MemoryStream ms = new())
            {
                await image.CopyToAsync(ms);
                var bytes = ms.ToArray();
                imageName = SaveByteArray(bytes);
            }

            return imageName;
        }

        private string SaveByteArray(byte[] bytes)
        {
            string imageName = Guid.NewGuid().ToString() + ".webp";
            var dir = _configuration["ImageFolder"];

            var sizes = _configuration["ImageSizes"].Split(",")
                    .Select(x => int.Parse(x));
            foreach (var size in sizes)
            {
                string dirSave = Path.Combine(Directory.GetCurrentDirectory(),
                    dir, $"{size}_{imageName}");
                using (var imageLoad = Image.Load(bytes))
                {
                    imageLoad.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(size, size),
                        Mode = ResizeMode.Max
                    }));
                    imageLoad.Save(dirSave, new WebpEncoder());
                }
            }
            return imageName;
        }

        public async Task<string> Save(string urlImage)
        {
            string imageName = String.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(urlImage).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                        imageName = SaveByteArray(imageBytes);
                    }
                }
            }
            catch
            {
                return imageName;
            }
            return imageName;
        }
    }
}
