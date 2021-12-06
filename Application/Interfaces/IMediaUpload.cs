using Application.MediaUpload;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
   public interface IMediaUpload
    {
        MediaUploadResult UploadMedia(IFormFile formFile);
    }
}
