using Application.Interfaces;
using Application.MediaUpload;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MediaUpload
{
    public class MediaUpload : IMediaUpload
    {
        private readonly IOptions<CloudinarySettings> configure;
        private Cloudinary _cloudinary;

        public MediaUpload(IOptions<CloudinarySettings> configure)
        {
            this.configure = configure;

            var account = new Account(
              configure.Value.CloudName,
              configure.Value.ApiKey,
              configure.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }
        MediaUploadResult IMediaUpload.UploadMedia(IFormFile formFile)
        {
            var uploadResult = new ImageUploadResult();

            if (formFile.Length > 0)
            {
                using (var stream = formFile.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(formFile.Name, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            if(uploadResult.Error != null)
            {
              throw new Exception(uploadResult.Error.Message);
            }

            return new MediaUploadResult { 
              PublicId = uploadResult.PublicId,
              Url = uploadResult.SecureUrl.AbsoluteUri
            };
        }
    }
}
