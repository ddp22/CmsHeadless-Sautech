using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;
using CmsHeadlessApi.Controllers;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using CmsHeadlessApi.ModelsController;
using Microsoft.AspNetCore.Http;

namespace CmsHeadlessApi.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public string pathMedia = AppDomain.CurrentDomain.BaseDirectory;

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
                c = _contextDb.Content.Include(ca=>ca.ContentAttributes).ThenInclude(a=>a.Attributes).Include(ct=>ct.ContentCategory).ThenInclude(c=>c.Category).Include(ctag=>ctag.ContentTag).ThenInclude(t=>t.Tag).Include(cl=>cl.ContentLocation).ThenInclude(c=>c.Location).ToList();
            }
            else
            {
                c = _contextDb.Content.Where(c=>c.ContentId==id).Include(ca => ca.ContentAttributes).ThenInclude(a => a.Attributes).Include(ct => ct.ContentCategory).ThenInclude(c => c.Category).Include(ctag => ctag.ContentTag).ThenInclude(t => t.Tag).Include(cl => cl.ContentLocation).ThenInclude(c => c.Location).ToList();
            }
            List<User> user = _contextDb.User.ToList();
            List<ContentAttributes> contentAttributes = (from Attributes in _contextDb.Attributes join ContentAttributes in _contextDb.ContentAttributes on Attributes.AttributesId equals ContentAttributes.AttributesId select ContentAttributes).ToList();
            List<ContentTag> contentTag = (from Tag in _contextDb.Tag join ContentTag in _contextDb.ContentTag on Tag.TagId equals ContentTag.TagId select ContentTag).ToList();
            List<ContentCategory> contentCategory = (from Category in _contextDb.Category join ContentCategory in _contextDb.ContentCategory on Category.CategoryId equals ContentCategory.CategoryId select ContentCategory).ToList();
            List<ContentLocation> contentLocation = (from ContentLocation in _contextDb.ContentLocation 
                                                     join Location in _contextDb.Location on ContentLocation.LocationId equals Location.LocationId
                                                     select ContentLocation).ToList();
            
            List<Nation> NationAvailable=(from Nation in _contextDb.Nation select Nation).ToList();
            List<Region> RegionAvailable=(from Region in _contextDb.Region select Region).ToList();
            List<Province> ProvinceAvailable=(from Province in _contextDb.Province select Province).ToList();
            foreach (var item in c)
            {
                var email = user.Where(u=>u.Id==item.UserId).First().Email;
                
                List<Attributes> attributes = contentAttributes.Where(a => a.ContentId == item.ContentId).Select(a => new Attributes(a.Attributes.AttributesId, a.Attributes.AttributeName, a.Attributes.AttributeValue)).ToList();
                List<Tag> tag = contentTag.Where(a => a.ContentId == item.ContentId).Select(a => new Tag(a.Tag.TagId, a.Tag.Name, a.Tag.Url)).ToList();
                List<Category> category = contentCategory.Where(a => a.ContentId == item.ContentId).Select(a => new Category(a.Category.CategoryId, a.Category.Name, a.Category.Description, a.Category.CategoryParentId, a.Category.Media, a.Category.CreationDate)).ToList();
                List<string> LocationsOfContentAvailable = new List<string>();
                List<Location> LocationsOfContent = contentLocation.Where(c => c.ContentId == item.ContentId).Select(c => c.Location).ToList();

                foreach (var location in LocationsOfContent)
                {
                    
                    var tempNation = NationAvailable.Find(c => c.NationId == location.NationId);
                    string nation = tempNation != null ? tempNation.NationName : null;
                    string region = null;
                    string province = null;
                    string city = location.City;
                    if (nation != null)
                    {
                        var tempRegion = RegionAvailable.Find(c => c.RegionId == location.RegionId);
                        region = tempRegion != null ? tempRegion.RegionName : null;
                        if (region != null)
                        {
                            var tempProvince = ProvinceAvailable.Find(c => c.ProvinceId == location.ProvinceId);
                            province = tempProvince != null ? tempProvince.ProvinceName : null;
                        }
                    }
                    LocationsOfContentAvailable.Add(new LocationsOfContentModel(location.LocationId, nation, region, province, city).LocationString);
                }

                model.Add(new ContentControllerModel(item, pathMedia, email, attributes, tag, category, LocationsOfContentAvailable));
            }
            return Json(model);
        }

        
    }
}