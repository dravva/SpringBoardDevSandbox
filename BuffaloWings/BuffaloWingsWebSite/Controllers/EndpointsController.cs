using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuffaloWingsWebSite.Controllers
{
    public class EndpointsController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post()
        {
            try
            {
                IEnumerable<String> apiHeaders = Request.Headers.GetValues("api");
                string api = apiHeaders.FirstOrDefault();

                if (!String.IsNullOrEmpty(api))
                {
                    var bodyContent = Request.Content.ReadAsByteArrayAsync().Result;

                    HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format(api, ""));
                    WebReq.Method = "POST";
                    WebReq.ContentType = Request.Content.Headers.ContentType.MediaType;
                    var requestStream = WebReq.GetRequestStream();
                    requestStream.Write(bodyContent, 0, bodyContent.Length);

                    try
                    {
                        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                        Stream Answer = WebResp.GetResponseStream();
                        StreamReader _Answer = new StreamReader(Answer);

                        string XMLString = _Answer.ReadToEnd();

                        return Request.CreateResponse(HttpStatusCode.OK, XMLString);
                    }
                    catch (WebException err)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                            err.Message + "/" + api);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed,
                        "api header is not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                                    ex.Message);                                
            }
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                IEnumerable<String> apiHeaders = Request.Headers.GetValues("api");
                string api = apiHeaders.FirstOrDefault();

                if (!String.IsNullOrEmpty(api))
                {
                    HttpWebRequest WebReq = (HttpWebRequest) WebRequest.Create(string.Format(api, ""));
                    WebReq.Method = "GET";

                    try
                    {
                        HttpWebResponse WebResp = (HttpWebResponse) WebReq.GetResponse();
                        Stream Answer = WebResp.GetResponseStream();
                        StreamReader _Answer = new StreamReader(Answer);

                        string XMLString = _Answer.ReadToEnd();

                        return Request.CreateResponse(HttpStatusCode.OK, XMLString);
                    }
                    catch (WebException err)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                            err.Message + "/" + api);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed,
                        "api header is not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
    }
}