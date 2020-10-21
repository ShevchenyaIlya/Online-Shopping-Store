using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration _configuration;
        public ImageService(IWebHostEnvironment appEnvironment, IConfiguration configuration)
        {
            _configuration = configuration;
            _appEnvironment = appEnvironment;
        }
        public async Task<string> SaveImageAsync(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = _configuration["ImagePath"] + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                return path;
            }
            return "";
        }

        public void DeleteImage(string Path)
        {
            if(File.Exists(_appEnvironment.WebRootPath + Path))
                File.Delete(_appEnvironment.WebRootPath + Path);
        }
    }
}
