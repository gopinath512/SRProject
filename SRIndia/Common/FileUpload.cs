using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SRIndia_Models;
using System;
using System.IO;

namespace SRIndia.Common
{

    public interface IFileUpload
    {
        UploadeResponse Upload(IFormFile file);
    }
    public class FileUpload : IFileUpload
    {
        private IHostingEnvironment _hostingEnv;
        private ILogger<FileUpload> _logger;

        public FileUpload(IHostingEnvironment env, ILogger<FileUpload> logger)
        {
            _hostingEnv = env;
            _logger = logger;
        }

        public UploadeResponse Upload(IFormFile file)
        {
            var imgId = string.Empty;
            try
            {
                long size = 0;

                var filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"');
                filename = Guid.NewGuid() + "_" + filename;
                imgId = filename;
                filename = _hostingEnv.WebRootPath + "\\Images" + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while uploading image.", ex);
                return new UploadeResponse { ImageID = imgId, Success = false, ErrorDescription = ex.Message };
            }
            return new UploadeResponse { ImageID = imgId, Success = true };
        }
    }
}
