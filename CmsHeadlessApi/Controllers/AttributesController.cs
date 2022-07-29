using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;
using CmsHeadlessApi.ModelsController;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadlessApi.Controllers
{
    public class AttributesController : Controller
    {
        private readonly ILogger<AttributesController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        List<AttributesTypology> AttributesTypology { get; set; }
        List<string> TypologyAvailable { get; set; }
        public AttributesController(ILogger<AttributesController> logger, CmsHeadlessDbContext contextDb)
        {
            _logger = logger;
            _contextDb = contextDb;

            AttributesTypology = (from AttributesTypology in _contextDb.AttributesTypology select AttributesTypology).ToList();
            
        }

        [HttpGet]
        public JsonResult GetAllAttributes(int? idAttributes, string? NameAttributes, string token)
        {

            if (token == null) {
                return Json("false"); 
            }

            if (token != "123")
            {
                return Json("false");
            }

            List<AttributesControllerModel> model = new List<AttributesControllerModel>();
            List<Attributes> a=new List<Attributes>();
            if (idAttributes == null && NameAttributes == null) {
                a = _contextDb.Attributes.Include(c => c.AttributesTypology).ThenInclude(c => c.Typology).ToList();
                foreach(var item in a)
                {
                    TypologyAvailable = new List<string>();
                    TypologyAvailable = AttributesTypology.Where(c => c.AttributesId == item.AttributesId).Select(c => c.Typology.Name).ToList();
                    model.Add(new AttributesControllerModel(item.AttributesId, item.AttributeName, item.AttributeValue, TypologyAvailable));
                }
                
                return Json(model);
            }
            else if(NameAttributes == null){
                Attributes attributesItem = _contextDb.Attributes.FindAsync(idAttributes).Result;
                if (attributesItem == null)
                {
                    return Json(null);
                }
                else
                {
                    a = _contextDb.Attributes.Where(c=>c.AttributesId==idAttributes).Include(c => c.AttributesTypology).ThenInclude(c => c.Typology).ToList();
                    TypologyAvailable = new List<string>();
                    foreach(var item in a)
                    {
                        TypologyAvailable = AttributesTypology.Where(c => c.AttributesId == item.AttributesId).Select(c => c.Typology.Name).ToList();
                    }
                    model.Add(new AttributesControllerModel(attributesItem.AttributesId, attributesItem.AttributeName, attributesItem.AttributeValue, TypologyAvailable));
                    return Json(model);
                }
            }
            else if(idAttributes == null)
            {
                a = _contextDb.Attributes.Where(c => c.AttributeName.Contains(NameAttributes)).Include(c => c.AttributesTypology).ThenInclude(c => c.Typology).ToList();

                if (a.Count()==0)
                {
                    return Json(null);
                }
                else
                {
                    foreach (var item in a)
                    {
                        TypologyAvailable = new List<string>();
                        TypologyAvailable = AttributesTypology.Where(c => c.AttributesId == item.AttributesId).Select(c => c.Typology.Name).ToList();
                        model.Add(new AttributesControllerModel(item.AttributesId, item.AttributeName, item.AttributeValue, TypologyAvailable));
                    }

                    return Json(model);
                }
            }
            else
            {
                a = _contextDb.Attributes.Where(c => c.AttributesId == idAttributes && c.AttributeName.Contains(NameAttributes)).Include(c => c.AttributesTypology).ThenInclude(c => c.Typology).ToList();

                if (a.Count() == 0)
                {
                    return Json(null);
                }
                else
                {
                    foreach (var item in a)
                    {
                        TypologyAvailable = new List<string>();
                        TypologyAvailable = AttributesTypology.Where(c => c.AttributesId == item.AttributesId).Select(c => c.Typology.Name).ToList();
                        model.Add(new AttributesControllerModel(item.AttributesId, item.AttributeName, item.AttributeValue, TypologyAvailable));
                    }

                    return Json(model);
                }
            }
        }

        
    }
}