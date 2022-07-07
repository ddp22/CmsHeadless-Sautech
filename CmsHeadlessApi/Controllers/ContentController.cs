using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;
using CmsHeadlessApi.Controllers;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using CmsHeadlessApi.ModelsController;

namespace CmsHeadlessApi.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public string pathMedia = "/img/content/";
        public ContentController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb)
        {
            _logger = logger;
            _contextDb = contextDb;
        }

        

        [HttpGet]
        public JsonResult getContentById(int? id)
        {
            if (id == null)
            {
                return Json(null);
            }
            List<Content> temp = _contextDb.Content.FromSqlInterpolated($"Exec Selectcontent @ContentId = {id}").ToList<Content>();
            return Json(temp);
        }

        [HttpGet]
        public JsonResult getContentByIdDetails(int? id)
        {
            List<ContentControllerModel> model = new List<ContentControllerModel>();
            List<Content> c = new List<Content>();
            if (id == null)
            {
                c = _contextDb.Content.Include(ca=>ca.ContentAttributes).ThenInclude(a=>a.Attributes).Include(ct=>ct.ContentCategory).ThenInclude(c=>c.Category).Include(ctag=>ctag.ContentTag).ThenInclude(t=>t.Tag).ToList();
            }
            else
            {
                c = _contextDb.Content.Where(c=>c.ContentId==id).Include(ca => ca.ContentAttributes).ThenInclude(a => a.Attributes).Include(ct => ct.ContentCategory).ThenInclude(c => c.Category).Include(ctag => ctag.ContentTag).ThenInclude(t => t.Tag).ToList();
            }
            List<User> user = _contextDb.User.ToList();
            List<ContentAttributes> contentAttributes = (from Attributes in _contextDb.Attributes join ContentAttributes in _contextDb.ContentAttributes on Attributes.AttributesId equals ContentAttributes.AttributesId select ContentAttributes).ToList();
            List<ContentTag> contentTag = (from Tag in _contextDb.Tag join ContentTag in _contextDb.ContentTag on Tag.TagId equals ContentTag.TagId select ContentTag).ToList();
            List<ContentCategory> contentCategory = (from Category in _contextDb.Category join ContentCategory in _contextDb.ContentCategory on Category.CategoryId equals ContentCategory.CategoryId select ContentCategory).ToList();
            foreach (var item in c)
            {
                var email = user.Where(u=>u.Id==item.UserId).First().Email;
                
                List<Attributes> attributes = contentAttributes.Where(a => a.ContentId == item.ContentId).Select(a => new Attributes(a.Attributes.AttributesId, a.Attributes.AttributeName, a.Attributes.AttributeValue)).ToList();
                List<Tag> tag = contentTag.Where(a => a.ContentId == item.ContentId).Select(a => new Tag(a.Tag.TagId, a.Tag.Name, a.Tag.Url)).ToList();
                List<Category> category = contentCategory.Where(a => a.ContentId == item.ContentId).Select(a => new Category(a.Category.CategoryId, a.Category.Name, a.Category.Description, a.Category.CategoryParentId, a.Category.Media, a.Category.CreationDate)).ToList();
                model.Add(new ContentControllerModel(item, pathMedia, email, attributes, tag, category));
            }
            return Json(model);
        }
    }
}