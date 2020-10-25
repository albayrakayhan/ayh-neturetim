using aceka.web_api.Models;
using aceka.web_api.Models.AnonymousModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Dosya yükleme işlemleri için kullanılan servis
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FileUploadController : ApiController
    {
        /// <summary>
        /// File Upload POST Metodu. Aynı anda birden fazla dosya upload edilebilir.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<HttpResponseMessage> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var serverUploadFolder = System.Web.Hosting.HostingEnvironment.MapPath("/content/files/");
            //var provider = new MultipartFormDataStreamProvider(root);

            var streamProvider = new MultipartFormDataStreamProvider(serverUploadFolder);
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            try
            {

                List<FileUpload> fileUpload = new List<FileUpload>();

                var FileNames = streamProvider.FileData.Select(entry => entry.LocalFileName).ToList();
                var Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName).ToList();
                var ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType).ToList();
                for (int i = 0; i < FileNames.Count; i++)
                {

                    var newFileName =
                        string.Format("{0}{1}",
                        System.IO.Path.GetFileNameWithoutExtension(Names[i].Replace("\"", "")) + "_" + DateTime.Now.Ticks.ToString(),
                        System.IO.Path.GetExtension(Names[i].Replace("\"", ""))
                        );


                    System.IO.File.Move(System.IO.Path.Combine(serverUploadFolder, FileNames[i]), System.IO.Path.Combine(serverUploadFolder, newFileName));

                    fileUpload.Add(
                        new FileUpload { FileName = newFileName, ContentType = ContentTypes[i] }
                        );
                }


                return Request.CreateResponse(HttpStatusCode.OK, fileUpload);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// File Upload DELETE Metodu. Aynı anda birden fazla dosya silinebilir. 
        /// NOT: Array içerisinde sadece "fileName" alanı doldurulmalıdır!
        /// </summary>
        /// <param name="ekler"></param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage Delete(List<Models.StokkartModel.StokkartEkler> ekler)
        {
            string errMessage = "";

            if (ekler != null && ekler.Count > 0)
            {
                var serverUploadFolder = System.Web.Hosting.HostingEnvironment.MapPath("/content/files/");
                foreach (var item in ekler)
                {
                    if (!string.IsNullOrEmpty(item.filename.Trim()) && System.IO.File.Exists(serverUploadFolder + item.filename.Trim()))
                    {
                        try
                        {
                            System.IO.File.Delete(serverUploadFolder + item.filename.Trim());
                        }
                        catch (Exception ex)
                        {
                            errMessage = ex.Message;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(errMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, errMessage);
                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

    }
}
