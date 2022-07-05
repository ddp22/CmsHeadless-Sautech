using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;

namespace CmsHeadlessApi.Controllers
{
    public class AttributesController : Controller
    {
        private readonly ILogger<AttributesController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public AttributesController(ILogger<AttributesController> logger, CmsHeadlessDbContext contextDb)
        {
            _logger = logger;
            _contextDb = contextDb;
        }

        [HttpGet]
        public JsonResult GetAllAttributes(int? idAttributes, string? NameAttributes)
        {
            if (idAttributes == null && NameAttributes == null) {
                return Json(_contextDb.Attributes.ToList<Attributes>());
            }
            else if(NameAttributes == null){
                var attributesItem = _contextDb.Attributes.FindAsync(idAttributes);
                if (attributesItem == null)
                {
                    return Json(null);
                }
                else
                {
                    return Json(attributesItem.Result);
                }
            }
            else if(idAttributes == null)
            {
                var attributesItem = _contextDb.Attributes.Where(c=>c.AttributeName.Contains(NameAttributes)).ToList<Attributes>();
                if (attributesItem.Count()==0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(attributesItem);
                }
            }
            else
            {
                var attributesItem = _contextDb.Attributes.Where(c => c.AttributesId == idAttributes && c.AttributeName.Contains(NameAttributes)).ToList<Attributes>();
                if (attributesItem.Count() == 0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(attributesItem);
                }
            }
        }

        
    }
}