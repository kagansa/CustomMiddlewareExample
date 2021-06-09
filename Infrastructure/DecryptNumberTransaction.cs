using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CustomMiddlewareExample.Infrastructure
{
    public class DecryptNumberTransaction
    {
        private RequestDelegate _request;

        public DecryptNumberTransaction(RequestDelegate request)
        {
            _request = request;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            var originBody = httpContext.Response.Body;
            try
            {
                var memStream = new MemoryStream();
                httpContext.Response.Body = memStream;
                await _request(httpContext).ConfigureAwait(false);
                memStream.Position = 0;
                var responseBody = new StreamReader(memStream).ReadToEnd();

                //Response Sonucuna +10 Ekle
                string sonuc = (Convert.ToInt32(responseBody) + 10).ToString();

                //Response Sonucunu Şifrele
                EncryptionManager encryptionManager = new EncryptionManager();
                responseBody = "Şifrelenmiş Sayı : '" + encryptionManager.Encrypt(sonuc) + "' Şifrelenmemiş Sayı '" + sonuc+"'";

                

                var memoryStreamModified = new MemoryStream();
                var sw = new StreamWriter(memoryStreamModified);
                sw.Write(responseBody);
                sw.Flush();
                memoryStreamModified.Position = 0;
                await memoryStreamModified.CopyToAsync(originBody).ConfigureAwait(false);
            }
            finally
            {
                httpContext.Response.Body = originBody;
            }

        }
    }
}