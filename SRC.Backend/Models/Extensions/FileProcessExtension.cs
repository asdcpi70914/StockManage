using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SRC.Backend.Models.Extensions
{
    public static class FileProcessExtension
    {
        public static string ToBase64(this IFormFile file)
        {
            string base64Fmt = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byte[] photoByte = ms.ToArray();

                string ext = new FileInfo(file.FileName).Extension?.Remove(0, 1);
                base64Fmt = $"data:image/{ext};base64,{Convert.ToBase64String(photoByte)}";
            }

            return base64Fmt;
        }

        public static string ToBase64(this FileInfo file)
        {
            if (file == null || !file.Exists)
            {
                return $"";
            }

            byte[] photoByte = null;
            string base64Fmt = string.Empty;
            using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                photoByte = new byte[fs.Length];
                fs.Read(photoByte, 0, photoByte.Length);
            }
            string ext = file.Extension.Remove(0, 1);
            base64Fmt = $"data:image/{ext};base64,{Convert.ToBase64String(photoByte)}";

            return base64Fmt;
        }
    }
}
