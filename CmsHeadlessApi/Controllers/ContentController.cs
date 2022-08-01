using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;
using CmsHeadlessApi.Controllers;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using CmsHeadlessApi.ModelsController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using ReverseGeocoding;
using System;
using System.IO;
using ReverseGeocoding.Interface;
using NUnit.Framework;
using System.Net;
using CmsHeadlessApi.Controllers.SupportClassContent;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CmsHeadless.Controllers;

namespace CmsHeadlessApi.Controllers
{
    
    public class ContentController : Controller
    {
        public static string key = "Av8pgMuhmukQRjXA8xa--AppJlojmG57snu33XELOXLcIf4gCm-iOFCe41QC7Tlb";
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        private readonly ServiceController _serviceController;
        //public string pathMedia = AppDomain.CurrentDomain.BaseDirectory;
        public string pathMedia= "https://localhost:7233";
        private readonly IServer _server;
        static HttpClient client = new HttpClient();
        //private static string itDbPath => Path.Combine(TestContext.CurrentContext.TestDirectory, itDb);
        public ContentController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb, IServer server, ServiceController serviceController)
        {
            _logger = logger;
            _contextDb = contextDb;
            _server= server;
            _serviceController = serviceController;
            //var geocoder = new ReverseGeocoder("wwwroot\\lib\\IT.txt");
        }


        [HttpGet]
        public JsonResult getContentById(int? id, string mail, string token)
        {
            if(_serviceController.tokenValidation(mail, token))
            {
                if (id == null)
                {
                    return Json(null);
                }
                List<Content> temp = _contextDb.Content.FromSqlInterpolated($"Exec Selectcontent @ContentId = {id}").ToList<Content>();
                return Json(temp);
            }
            return Json("Token error - please login again here: https://localhost:7274/User/LoginUser");
        }

        
        static async Task<SupportClassContent.Root> GetLocationAsync(double latitude, double longitude)
        {
            Root root = null;
            string path = "http://dev.virtualearth.net/REST/v1/Locations/"+latitude.ToString().Replace(",", ".")+","+longitude.ToString().Replace(",", ".") + "?key="+key;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                root = await response.Content.ReadFromJsonAsync<Root>();
            }
            return root;
        }

        [HttpGet]
        public JsonResult GetContentWithLocation(double latitude, double longitude, string mail, string token)
        {
            if(_serviceController.tokenValidation(mail, token))
            {
                string jsonString = JsonSerializer.Serialize(GetLocationAsync(latitude, longitude).Result);
                Root myDeserializedClass = JsonSerializer.Deserialize<Root>(jsonString);
                List<ResourceSet> resourceSets = myDeserializedClass.resourceSets;
                List<Resource> resources = resourceSets.First().resources;
                List<LocationString> locationStrings = new List<LocationString>();
                foreach (Resource resource in resources)
                {
                    LocationString temp = new LocationString();
                    Address address = resource.address;
                    temp.Nation = address.countryRegion;
                    temp.Region = address.adminDistrict;
                    temp.Province = address.adminDistrict2;
                    temp.City = address.locality;
                    locationStrings.Add(temp);
                }
                return Json(locationStrings);
            }
            return Json("Token error - please login again here: https://localhost:7274/User/LoginUser");
        }

        [HttpGet]
        public JsonResult getContent(int? id, double? latitude, double? longitude, string mail, string token)
        {
            if(_serviceController.tokenValidation(mail, token))
            {
                List<ContentControllerModel> model = new List<ContentControllerModel>();
                List<Content> c = new List<Content>();

                List<CmsUser> user = _contextDb.CmsUser.ToList();
                List<ContentAttributes> contentAttributes = (from Attributes in _contextDb.Attributes join ContentAttributes in _contextDb.ContentAttributes on Attributes.AttributesId equals ContentAttributes.AttributesId select ContentAttributes).ToList();
                List<ContentTag> contentTag = (from Tag in _contextDb.Tag join ContentTag in _contextDb.ContentTag on Tag.TagId equals ContentTag.TagId select ContentTag).ToList();
                List<ContentCategory> contentCategory = (from Category in _contextDb.Category join ContentCategory in _contextDb.ContentCategory on Category.CategoryId equals ContentCategory.CategoryId select ContentCategory).ToList();
                List<ContentLocation> contentLocation = (from ContentLocation in _contextDb.ContentLocation
                                                         join Location in _contextDb.Location on ContentLocation.LocationId equals Location.LocationId
                                                         select ContentLocation).Include(c => c.Location).ToList();
                List<Location> locations = (from Location in _contextDb.Location select Location).ToList();
                List<Nation> nations = (from Nation in _contextDb.Nation select Nation).ToList();
                List<Region> regions = (from Region in _contextDb.Region select Region).ToList();
                List<Province> provinces = (from Province in _contextDb.Province select Province).ToList();

                if (id == null)
                {
                    c = _contextDb.Content.Include(ca => ca.ContentAttributes).ThenInclude(a => a.Attributes)
                        .Include(ct => ct.ContentCategory).ThenInclude(c => c.Category)
                        .Include(ctag => ctag.ContentTag).ThenInclude(t => t.Tag)
                        .Include(cl => cl.ContentLocation).ThenInclude(c => c.Location).ThenInclude(c => c.Nation)
                        .Include(cl => cl.ContentLocation).ThenInclude(c => c.Location).ThenInclude(c => c.Province)
                        .Include(cl => cl.ContentLocation).ThenInclude(c => c.Location).ThenInclude(c => c.Region)
                        .ToList();
                }
                else
                {
                    c = _contextDb.Content.Where(c => c.ContentId == id).Include(ca => ca.ContentAttributes).ThenInclude(a => a.Attributes)
                        .Include(ct => ct.ContentCategory).ThenInclude(c => c.Category)
                        .Include(ctag => ctag.ContentTag).ThenInclude(t => t.Tag)
                        .Include(cl => cl.ContentLocation).ThenInclude(c => c.Location).ThenInclude(c => c.Nation)
                        .Include(cl => cl.ContentLocation).ThenInclude(c => c.Location).ThenInclude(c => c.Province)
                        .Include(cl => cl.ContentLocation).ThenInclude(c => c.Location).ThenInclude(c => c.Region)
                        .ToList();
                }

                if (latitude != null && longitude != null)
                {
                    List<int> intLocations = new List<int>();
                    string jsonString = JsonSerializer.Serialize(GetLocationAsync((double)latitude, (double)longitude).Result);
                    Root myDeserializedClass = JsonSerializer.Deserialize<Root>(jsonString);
                    if (myDeserializedClass == null || myDeserializedClass.resourceSets == null)
                    {
                        return Json(null);
                    }
                    List<ResourceSet> resourceSets = myDeserializedClass.resourceSets;
                    List<Resource> resources = resourceSets.First().resources;
                    LocationString locationStrings = new LocationString();
                    if (resources == null || resources.Count() <= 0)
                    {
                        return Json(null);
                    }
                    var t = resources.First();
                    Address address = t.address;
                    locationStrings.Nation = address.countryRegion;
                    locationStrings.Region = address.adminDistrict;
                    locationStrings.Province = address.adminDistrict2;
                    locationStrings.City = address.locality;
                    int intNation = nations.Where(c => c.NationName == locationStrings.Nation).Select(c => c.NationId).ToList().First();
                    int intRegion = regions.Where(c => c.RegionName == locationStrings.Region).Select(c => c.RegionId).ToList().First();
                    int intProvince = provinces.Where(c => c.ProvinceName == locationStrings.Province).Select(c => c.ProvinceId).ToList().First();
                    intLocations = locations.Where(c => c.NationId == intNation
                    && (c.RegionId == intRegion || c.Region == null)
                    && (c.ProvinceId == intProvince || c.Province == null)
                    && (c.City == locationStrings.City || c.City == null))
                    .Select(c => c.LocationId).ToList();
                    List<int> listContent = contentLocation.Where(c => intLocations.Contains(c.LocationId)).Select(c => c.ContentId).ToList();
                    c = c.Where(c => listContent.Contains(c.ContentId)).ToList();
                }

                foreach (var item in c)
                {
                    var email = user.Where(u => u.Id == item.UserId).First().Email;

                    List<Attributes> attributes = contentAttributes.Where(a => a.ContentId == item.ContentId).Select(a => new Attributes(a.Attributes.AttributesId, a.Attributes.AttributeName, a.Attributes.AttributeValue)).ToList();
                    List<Tag> tag = contentTag.Where(a => a.ContentId == item.ContentId).Select(a => new Tag(a.Tag.TagId, a.Tag.Name, a.Tag.Url)).ToList();
                    List<Category> category = contentCategory.Where(a => a.ContentId == item.ContentId).Select(a => new Category(a.Category.CategoryId, a.Category.Name, a.Category.Description, a.Category.CategoryParentId, a.Category.Media, a.Category.CreationDate)).ToList();
                    List<string> LocationsOfContentAvailable = new List<string>();
                    List<Location> LocationsOfContent = contentLocation.Where(c => c.ContentId == item.ContentId).Select(c => c.Location).ToList();

                    foreach (var location in LocationsOfContent)
                    {

                        string tempString = null;
                        if (location.Nation != null)
                        {
                            tempString = location.Nation.NationName;
                            if (location.Region != null)
                            {
                                tempString += ", " + location.Region.RegionName;
                                if (location.Province != null)
                                {
                                    tempString += ", " + location.Province.ProvinceName;
                                }
                                if (location.City != null)
                                {
                                    tempString += ", " + location.City;
                                }
                            }
                        }
                        LocationsOfContentAvailable.Add(tempString);
                    }

                    model.Add(new ContentControllerModel(item, pathMedia, email, attributes, tag, category, LocationsOfContentAvailable));
                }
                return Json(model);
            }
            return Json("Token error - please login again here: https://localhost:7274/User/LoginUser");
        }

        
    }
}