using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Library.Services
{
    public class CloudImageServices : IImageServices
    {
       
            private readonly IConfiguration configuration;
            private readonly Account account;

            public CloudImageServices(IConfiguration configuration)
            {
                this.configuration = configuration;
                account = new Account(
                    configuration.GetSection("Cloudinary")["CloudName"],
                    configuration.GetSection("Cloudinary")["ApiKey"],
                    configuration.GetSection("Cloudinary")["ApiSecret"]);
            }

            public async Task<string> UploadAsync(IFormFile file)
            {
                var client = new Cloudinary(account);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    DisplayName = file.FileName
                };

                var uploadResult = await client.UploadAsync(uploadParams);

                if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUri.ToString();
                }

                return null;
            }
        }
    }

