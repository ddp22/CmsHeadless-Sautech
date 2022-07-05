using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;

namespace CmsHeadlessApi.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public TagController(ILogger<TagController> logger, CmsHeadlessDbContext contextDb)
        {
            _logger = logger;
            _contextDb = contextDb;
        }

        [HttpGet]
        public JsonResult GetAllTag(int? idTag, string? NameTag)
        {
            if (idTag == null && NameTag == null) {
                return Json(_contextDb.Tag.ToList<Tag>());
            }
            else if(NameTag == null){
                var tagItem = _contextDb.Tag.FindAsync(idTag);
                if (tagItem == null)
                {
                    return Json(null);
                }
                else
                {
                    return Json(tagItem.Result);
                }
            }
            else if(idTag == null)
            {
                var tagItem = _contextDb.Tag.Where(c=>c.Name.Contains(NameTag)).ToList<Tag>();
                if (tagItem.Count()==0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(tagItem);
                }
            }
            else
            {
                var tagItem = _contextDb.Tag.Where(c => c.TagId == idTag && c.Name.Contains(NameTag)).ToList<Tag>();
                if (tagItem.Count() == 0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(tagItem);
                }
            }
        }

        
    }
}