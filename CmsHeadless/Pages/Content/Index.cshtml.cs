using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using CmsHeadless.Controllers;
using CmsHeadless.ViewModels.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using System.Data;

namespace CmsHeadless.Pages.Content
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        private readonly LogListController _logController;
        public const int numberPage = 5;
        [BindProperty]
        public ContentViewModel _formContentModel { get; set; }
        public static int lastCreate = 0;
        public static int lastDelete = 0;
        public static bool callDelete = false;
        public string pathMedia = "/img/content/";
        public int idRegionNone { get; set; }
        public int idNationNone { get; set; }
        public int idProvinceNone { get; set; }

        public static int ContentId = 0;

        public static string searchString { get; set; }
        public List<Models.Content> ContentAvailable { get; set; }

        public List<Models.Attributes> AttributesAvailable { get; set; }
        public List<Models.Category> CategoryAvailable { get; set; }
        public List<Models.Tag> TagAvailable { get; set; }


        public List<Models.Nation> NationAvailable { get; set; }
        public List<Models.Region> RegionAvailable { get; set; }
        public List<Models.Province> ProvinceAvailable { get; set; }
        public List<Models.Location> LocationAvailable { get; set; }
        public List<Models.User> Users { get; set; }
        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration, LogListController logController)
        {
            _context = context;
            Configuration = configuration;
            _logController = logController;
            ContentAvailable = new List<Models.Content>();

            IQueryable<Models.Attributes> selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();

            IQueryable<Models.Category> selectCategoryQuery = from Category in _context.Category select Category;
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();

            IQueryable<Models.Tag> selectTagQuery = from Tag in _context.Tag select Tag;
            TagAvailable = selectTagQuery.ToList<Models.Tag>();

            IQueryable<Models.Nation> selectNationQuery = from Nation in _context.Nation select Nation;
            NationAvailable = selectNationQuery.ToList<Models.Nation>();

            IQueryable<Models.Region> selectRegionQuery = from Region in _context.Region select Region;
            RegionAvailable = selectRegionQuery.ToList<Models.Region>();

            IQueryable<Models.Province> selectProvinceQuery = from Province in _context.Province select Province;
            ProvinceAvailable = selectProvinceQuery.ToList<Models.Province>();

            IQueryable<Models.Location> selectLocationQuery = from Location in _context.Location select Location;
            LocationAvailable = selectLocationQuery.ToList<Models.Location>();

            IQueryable<Models.User> selectUsersQuery = from User in _context.User select User;
            Users = selectUsersQuery.ToList<Models.User>();
        }

        public ContentList<Models.Content> ContentList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? searchString)
        {

            IQueryable<Models.Content> selectContentQueryOrder = from Content in _context.Content select Content;
            IQueryable<Models.Content> selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
            if (!string.IsNullOrEmpty(searchString))
            {
                IndexModel.searchString = searchString;

                List<int> tempContentAttribute = (from c in _context.Content
                                                  join ca in _context.ContentAttributes
                                                  on c.ContentId equals ca.ContentId
                                                  join a in _context.Attributes
                                                  on ca.AttributesId equals a.AttributesId
                                                  where a.AttributeName.Contains(searchString)
                                                  select c.ContentId).ToList<int>();

                List<int> tempContentTag = (from c in _context.Content
                                            join ct in _context.ContentTag
                                            on c.ContentId equals ct.ContentId
                                            join t in _context.Tag
                                            on ct.TagId equals t.TagId
                                            where t.Name.Contains(searchString)
                                            select c.ContentId).ToList<int>();

                List<int> tempContentCategory = (from c in _context.Content
                                                 join cc in _context.ContentCategory
                                                 on c.ContentId equals cc.ContentId
                                                 join ca in _context.Category
                                                 on cc.CategoryId equals ca.CategoryId
                                                 where ca.Name.Contains(searchString)
                                                 select c.ContentId).ToList<int>();

                selectContentQuery = selectContentQuery.Where(s => (s.Title.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Text.Contains(searchString))
                || tempContentAttribute.Contains(s.ContentId)
                || tempContentTag.Contains(s.ContentId)
                || tempContentCategory.Contains(s.ContentId));

            }
            ContentAvailable = selectContentQuery.ToList<Models.Content>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            ContentList = await ContentList<Models.Content>.CreateAsync(
            selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostCreateAsync(int? pageIndex)
        {
            lastCreate = 0;
            lastDelete = 0;
            IQueryable<Models.Content> selectContentQuery;
            IQueryable<Models.Content> selectContentQueryOrder;

            var pageSize = 5;

            int is_exsist = _context.Content.Where(c => c.Title == _formContentModel.Title).Count();

            if (is_exsist > 0)
            {
                ModelState.AddModelError("Make", "Non è stato possibile inserire il content perchè già esiste");
                _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - Content Already Created", HttpContext);
                selectContentQueryOrder = from Content in _context.Content select Content;
                selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
                ContentAvailable = selectContentQuery.ToList<Models.Content>();
                if (pageIndex == null)
                {
                    pageIndex = 1;
                }
                pageSize = Configuration.GetValue("PageSize", numberPage);
                ContentList = await ContentList<Models.Content>.CreateAsync(selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
                return Page();
            }

            string uniqueFileName = null;
            if (_formContentModel.Media != null)
            {
                string uploadsFolder = Path.Combine("wwwroot/img/content");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + _formContentModel.Media.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    _formContentModel.Media.CopyTo(fileStream);
                }
            }


            Models.Content temp = new Models.Content();

            temp.Title = _formContentModel.Title;
            temp.Description = _formContentModel.Description;

            if (_formContentModel.Media != null)
            {
                temp.Media = pathMedia + uniqueFileName;
            }
            temp.Text = _formContentModel.Text;
            temp.InsertionDate = DateTime.Now.Date;
            temp.LastEdit = null;
            temp.PubblicationDate = _formContentModel.PubblicationDate;
            if (DateTime.Now.Date > _formContentModel.PubblicationDate)
            {
                temp.PubblicationDate = null;
            }


            temp.UserId = User.Identity.GetUserId();


            var entry = _context.Add(new Models.Content());
            entry.CurrentValues.SetValues(temp);
            lastCreate = await _context.SaveChangesAsync();
            if (lastCreate <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - Saving Changes Error", HttpContext);
                return Page();
            }

            selectContentQuery = from Content in _context.Content where Content.Title == temp.Title select Content;
            Models.Content tempContent = selectContentQuery.ToList<Models.Content>().First<Models.Content>();
            ContentId = tempContent.ContentId;

            /* ContentAttributes */
            if (_formContentModel.ContentAttributes != null)
            {
                var tempContentAttributes = new Models.ContentAttributes();
                //temp.ContentAttributes = new List<Models.ContentAttributes>();
                foreach (int i in _formContentModel.ContentAttributes)
                {
                    var entryContentAttributes = _context.Add(new Models.ContentAttributes());
                    tempContentAttributes.AttributesId = i;
                    tempContentAttributes.ContentId = tempContent.ContentId;

                    //temp.ContentAttributes.Add(tempContentAttributes);

                    entryContentAttributes.CurrentValues.SetValues(tempContentAttributes);
                    int k = await _context.SaveChangesAsync();
                    if (k <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - Saving Changes Error", HttpContext);
                        return Page();
                    }
                }
            }

            /* ContentTag */
            if (_formContentModel.ContentTag != null)
            {
                var tempContentTag = new Models.ContentTag();
                //temp.ContentTag = new List<Models.ContentTag>();
                foreach (int i in _formContentModel.ContentTag)
                {
                    var entryContentTag = _context.Add(new Models.ContentTag());
                    tempContentTag.TagId = i;
                    tempContentTag.ContentId = tempContent.ContentId;

                    //temp.ContentTag.Add(tempContentTag);

                    entryContentTag.CurrentValues.SetValues(tempContentTag);
                    int k = await _context.SaveChangesAsync();
                    if (k <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - Saving Changes Error", HttpContext);
                        return Page();
                    }
                }
            }

            /* ContentCategory */
            if (_formContentModel.ContentCategory != null)
            {
                var tempContentCategory = new Models.ContentCategory();
                //temp.ContentCategory = new List<Models.ContentCategory>();
                foreach (int i in _formContentModel.ContentCategory)
                {
                    var entryContentCategory = _context.Add(new Models.ContentCategory());
                    tempContentCategory.CategoryId = i;
                    tempContentCategory.ContentId = tempContent.ContentId;

                    //temp.ContentCategory.Add(tempContentCategory);

                    entryContentCategory.CurrentValues.SetValues(tempContentCategory);
                    int k = await _context.SaveChangesAsync();
                    if (k <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - Saving Changes Error", HttpContext);
                        return Page();
                    }
                }
            }

            /* ContentLocation */
            int ? nation = (_formContentModel.Nation == null || _formContentModel.Nation <= 0) ? null : _formContentModel.Nation;
            int? region = (_formContentModel.Region == null || _formContentModel.Region <= 0) ? null : _formContentModel.Region;
            int? province = (_formContentModel.Province == null || _formContentModel.Province <= 0) ? null : _formContentModel.Province;
            string? city = _formContentModel.City == null ? null : _formContentModel.City;
            var is_exists = LocationAvailable.Where(c => c.NationId == nation
                                              && c.RegionId == region
                                              && c.ProvinceId == province
                                              && c.City == city).ToList();
            int? id = null;
            if (is_exists.Count() > 0)
            {
                id = is_exists.First().LocationId;
            }

            if (nation > 0)
            {
                if (id == null)
                {
                    var entryLocation = _context.Add(new Location());
                    Location tempLocation = new Location(nation, region, province, city);
                    entryLocation.CurrentValues.SetValues(tempLocation);
                    int l = await _context.SaveChangesAsync();
                    if (l <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - Saving Changes Error", HttpContext);
                        return Page();
                    }
                    id = (from Location in _context.Location
                          where (Location.NationId == nation
                         && Location.RegionId == region
                         && Location.ProvinceId == province
                         && Location.City == city)
                          select Location.LocationId).ToList().First();
                }

                var tempContentLocation = new Models.ContentLocation();
                var entryContentLocation = _context.Add(new Models.ContentLocation());
                tempContentLocation.LocationId = (int)id;
                tempContentLocation.ContentId = tempContent.ContentId;

                entryContentLocation.CurrentValues.SetValues(tempContentLocation);
                int j = await _context.SaveChangesAsync();
                if (j <= 0)
                {
                    ModelState.AddModelError("Make", "Errore nell'inserimento");
                    _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - Saving Changes Error", HttpContext);
                    return Page();
                }

            }


            selectContentQueryOrder = from Content in _context.Content select Content;
            selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            pageSize = Configuration.GetValue("PageSize", numberPage);
            ContentList = await ContentList<Models.Content>.CreateAsync(selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            _logController.SaveLog(User.Identity.Name, LogListController.ContentsCreatedCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Content Created", HttpContext);
            return RedirectToPage("./Index");
        }




        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? contentId)
        {
            lastDelete = 0;
            lastCreate = 0;
            callDelete = true;
            if (contentId == null)
            {
                return NotFound();
            }

            var content = await _context.Content.FindAsync(contentId);

            IQueryable<Models.Content> selectContentQuery;
            IQueryable<Models.Content> selectContentQueryOrder;
            if (content == null)
            {
                return NotFound();
            }
            string strPhysicalFolder = "wwwroot";
            FileInfo file = new FileInfo(strPhysicalFolder+content.Media);
            if (file.Exists)
            {
                file.Delete();
            }
            _context.Content.Remove(content);
            lastDelete = await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'eliminazione");
                _logController.SaveLog(User.Identity.Name, LogListController.ContentsDeletedWarningCode, "L'utente " + User.Identity.Name + " ha creato un contenuto.", "Warning - No Last Deleted Content(s)", HttpContext);
                return Page();
            }
            selectContentQueryOrder = from Content in _context.Content select Content;
            selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            ContentList = await ContentList<Models.Content>.CreateAsync(selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            _logController.SaveLog(User.Identity.Name, LogListController.ContentsDeletedCode, "L'utente " + User.Identity.Name + " ha eliminato un contenuto.", "Content(s) Deleted", HttpContext);
            return RedirectToPage("./Index");
        }

    }
}