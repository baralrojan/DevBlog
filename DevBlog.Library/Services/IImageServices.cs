using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Library.Services
{
    public interface IImageServices
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
