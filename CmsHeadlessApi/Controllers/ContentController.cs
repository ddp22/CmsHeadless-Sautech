using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;

namespace CmsHeadlessApi.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public ContentController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb)
        {
            _logger = logger;
            _contextDb = contextDb;
        }

        [HttpGet]
        public JsonResult GetAllContent(int? idContent, string? TitleContent)
        {
            if (idContent == null && TitleContent == null) {
                return Json(_contextDb.Content.ToList<Content>());
            }
            else if(TitleContent == null){
                var contentItem = _contextDb.Content.FindAsync(idContent);
                if (contentItem == null)
                {
                    return Json(null);
                }
                else
                {
                    return Json(contentItem.Result);
                }
            }
            else if(idContent == null)
            {
                var contentItem = _contextDb.Content.Where(c=>c.Title.Contains(TitleContent)).ToList<Content>();
                if (contentItem.Count()==0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(contentItem);
                }
            }
            else
            {
                var contentItem = _contextDb.Content.Where(c => c.ContentId == idContent && c.Title.Contains(TitleContent)).ToList<Content>();
                if (contentItem.Count() == 0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(contentItem);
                }
            }
        }

        
    }
}