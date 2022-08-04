using CmsHeadless.AuthenticationJWT;
using CmsHeadless.Controllers;
using CmsHeadless.Models;
using CmsHeadlessApi.Controllers.SupportClassContent;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CmsHeadlessApi.Controllers
{
    

    //private static string itDbPath => Path.Combine(TestContext.CurrentContext.TestDirectory, itDb);
    
    public class QrCodeController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        private readonly ServiceController _serviceController;
        private readonly IServer _server;
        static HttpClient client = new HttpClient();
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly ResponseApi _response;
        private ContentQrCodeJson ContentQrCodeJson;
        public QrCodeController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb, IServer server, ServiceController serviceController)
        {
            _logger = logger;
            _contextDb = contextDb;
            _server = server;
            _serviceController = serviceController;
        }

        [HttpGet]
        public JsonResult GetContentByQrCode(string label, string mail, string token)
        {
            if (label == null)
            {
                return Json(null);
            }
            if (_serviceController.tokenValidation(mail, token))
            {
                var is_exists_QrCode = _contextDb.QrCode.Where(c => c.QrCodeLabel == label).ToList();
                if (is_exists_QrCode.Count() > 0)
                {
                    int contentId = is_exists_QrCode.FirstOrDefault().ContentId;
                    var is_exists_Content = _contextDb.Content.Where(c => c.ContentId==contentId).ToList();
                    if (is_exists_Content.Count() > 0)
                    {
                        Content content = is_exists_Content.FirstOrDefault();
                        var is_exists_ContentAttributes = _contextDb.ContentAttributes.Where(c => c.ContentId == contentId);
                        if(is_exists_ContentAttributes.Count() > 0)
                        {
                            List<int> tempAttributes = is_exists_ContentAttributes.Select(c => c.AttributesId).ToList();
                            List<string> contentsIdString = _contextDb.Attributes.Where(c=>tempAttributes.Contains(c.AttributesId) && c.AttributeName=="POI").Select(c=>c.AttributeValue).ToList();
                            List<int> contentsId = new List<int>();
                            foreach(string id in contentsIdString)
                            {
                                contentsId.Add(Int32.Parse(id));
                            }
                            List<Content> contentList = _contextDb.Content.Where(c => contentsId.Contains(c.ContentId)).ToList();
                            ContentQrCodeJson = new ContentQrCodeJson(content, contentList);
                            return Json(ContentQrCodeJson);
                        }
                        else
                        {
                            ContentQrCodeJson = new ContentQrCodeJson(content, new List<Content>());
                            return Json(ContentQrCodeJson);
                        }
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }
            }
            return Json("Token error - please login again here: https://localhost:7274/User/LoginUser");
        }
    }
}
