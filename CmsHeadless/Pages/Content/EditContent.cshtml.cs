using CmsHeadless.Models;
using CmsHeadless.ViewModels.Content;
using CmsHeadless.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CmsHeadless.Pages.Content
{

    public class EditContentModel : PageModel
    {
        IQueryable<Models.Content> selectContentQuery;
        IQueryable<Models.Content> selectContentQueryOrder;
        private readonly LogListController _logController;
        public static int EditContentId=0;
        public static int lastEdit = 0;
        public static int lastEditAttributes = 0;
        public static int lastEditTag = 0;
        public static int lastEditCategory = 0;
        public static int lastEditLocation = 0;
        public static int lastDeleteLocation = 0;
        public static bool callDelete = false;
        public string pathName = "/img/content/";

        public Models.Content content;
        public Models.Content EditContentNew { get; set; }
        [BindProperty]
        public EditContentViewModel _formEditContentModel { get; set; }
        [BindProperty]
        public DeleteContentViewModel _formDeleteContentModel { get; set; }
        private readonly CmsHeadlessDbContext _context;
        public List<Models.Content> ContentAvailable { get; set; }
        public string CreationDate;

        public List<Models.Attributes> AttributesAvailable { get; set; }
        public List<Models.Category> CategoryAvailable { get; set; }
        public List<Models.Tag> TagAvailable { get; set; }

        public List<int> AttributesSelected { get; set; }
        public List<int> CategorySelected { get; set; }
        public List<int> TagSelected { get; set; }

        public string? InsertionDateString;
        public string? PubblicationDateString;
        public string? lastEditString;

        public List<Models.Nation> NationAvailable { get; set; }
        public List<Models.Region> RegionAvailable { get; set; }
        public List<Models.Province> ProvinceAvailable { get; set; }
        public List<Models.Location> LocationAvailable { get; set; }
        public List<Models.ContentLocation> ContentLocationAvailable { get; set; }
        public List<Models.Location> LocationsOfContent { get; set; }

        public List<string> stringLocation { get; set; }
        
        

        public EditContentModel(CmsHeadlessDbContext context, LogListController logController)
        {
            _context = context;
            _logController = logController;
            ContentAvailable= new List<Models.Content>();
            stringLocation = new List<string>();

            //LocationsOfContentAvailable = new List<LocationsOfContent>();

            IQueryable<Models.Attributes> selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();

            IQueryable<Models.Category> selectCategoryQuery = from Category in _context.Category select Category;
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();

            IQueryable<Models.Tag> selectTagQuery = from Tag in _context.Tag select Tag;
            TagAvailable = selectTagQuery.ToList<Models.Tag>();

            AttributesSelected = new List<int>();
            TagSelected = new List<int>();
            CategorySelected= new List<int>();

            IQueryable<Models.Nation> selectNationQuery = from Nation in _context.Nation select Nation;
            NationAvailable = selectNationQuery.ToList<Models.Nation>();

            IQueryable<Models.Location> selectLocationQuery = from Location in _context.Location select Location;
            LocationAvailable = selectLocationQuery.ToList<Models.Location>();

            IQueryable<Models.Region> selectRegionQuery = from Region in _context.Region select Region;
            RegionAvailable = selectRegionQuery.ToList<Models.Region>();

            IQueryable<Models.Province> selectProvinceQuery = from Province in _context.Province select Province;
            ProvinceAvailable = selectProvinceQuery.ToList<Models.Province>();

            IQueryable<Models.ContentLocation> selectContentLocationQuery = from ContentLocation in _context.ContentLocation select ContentLocation;
            ContentLocationAvailable = selectContentLocationQuery.ToList<Models.ContentLocation>();

            
        }
        public async Task<IActionResult> OnGetAsync(int? id, string? searchString)
        {
            LocationsOfContent = ContentLocationAvailable.Where(c => c.ContentId == id).Select(c=>c.Location).ToList();

            selectContentQueryOrder = from Content in _context.Content select Content;
            selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
            if (!string.IsNullOrEmpty(searchString))
            {
                selectContentQuery = selectContentQuery.Where(s => s.Title.Contains(searchString));
            }
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            if (id == null)
            {
                if (EditContentId != 0)
                {
                    id = EditContentId;
                }
                else
                {
                    return NotFound();
                }
                
            }
            EditContentId = (int)id;
            if (_context==null || _context.Content == null)
            {
                return NotFound();
            }
            content = await _context.Content.FindAsync(id);

            if (content.PubblicationDate == null)
            {
                PubblicationDateString = null;
            }
            else
            {
                DateTime tempDate =(DateTime) content.PubblicationDate;
                PubblicationDateString = tempDate.ToString("yyyy-MM-dd");
            }

            if (content.InsertionDate == null)
            {
                InsertionDateString = null;
            }
            else
            {
                InsertionDateString = content.InsertionDate.ToString("yyyy-MM-dd");
            }

            if (content.LastEdit == null)
            {
                lastEditString = null;
            }
            else
            {
                DateTime tempDate = (DateTime)content.LastEdit;
                lastEditString = tempDate.ToString("yyyy-MM-dd");
            }

            if (content == null)
            {
                return NotFound();
            }

            //var tempContentAttributes = new Models.ContentAttributes();
            IQueryable<Models.ContentAttributes> selectContentAttributesQuery = from ContentAttributes in _context.ContentAttributes where ContentAttributes.ContentId == id select ContentAttributes;
            if (selectContentAttributesQuery != null)
            {
                foreach(var i in selectContentAttributesQuery.ToList<ContentAttributes>())
                {
                    AttributesSelected.Add(i.AttributesId);
                }
            }

            //var tempContentTag = new Models.ContentTag();
            IQueryable<Models.ContentTag> selectContentTagQuery = from ContentTag in _context.ContentTag where ContentTag.ContentId == id select ContentTag;
            if (selectContentTagQuery != null)
            {
                foreach (var i in selectContentTagQuery.ToList<ContentTag>())
                {
                    TagSelected.Add(i.TagId);
                }
            }

            //var tempContentCategory = new Models.ContentCategory();
            IQueryable<Models.ContentCategory> selectContentCategoryQuery = from ContentCategory in _context.ContentCategory where ContentCategory.ContentId == id select ContentCategory;
            if (selectContentCategoryQuery != null)
            {
                foreach (var i in selectContentCategoryQuery.ToList<ContentCategory>())
                {
                    CategorySelected.Add(i.CategoryId);
                }
            }



            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int contentId)
        {
            lastEdit = 0;
            lastEditAttributes = 0;
            lastEditTag = 0;
            lastEditCategory = 0;
            lastEditLocation = 0;
            lastDeleteLocation = 0;
            callDelete = false;
            var ContentToUpdate = await _context.Content.FindAsync(contentId);

            if (ContentToUpdate == null)
            {
                return NotFound();
            }

            if (ContentToUpdate.Title != _formEditContentModel.Title)
            {
                var ContentToSearch = from Content in _context.Content
                                       where Content.Title == _formEditContentModel.Title
                                       select Content;
                if (ContentToSearch.Count<Models.Content>() != 0)
                {
                    ModelState.AddModelError("Make", "Content già esistente. Inserirne un altro");
                    selectContentQueryOrder = from Content in _context.Content select Content;
                    selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
                    ContentAvailable = selectContentQuery.ToList<Models.Content>();
                    content = await _context.Content.FindAsync(contentId);
                    if (content.PubblicationDate == null)
                    {
                        PubblicationDateString = null;
                    }
                    else
                    {
                        DateTime tempDate = (DateTime)content.PubblicationDate;
                        PubblicationDateString = tempDate.ToString("yyyy-MM-dd");
                    }

                    if (content.InsertionDate == null)
                    {
                        InsertionDateString = null;
                    }
                    else
                    {
                        InsertionDateString = content.InsertionDate.ToString("yyyy-MM-dd");
                    }

                    if (content.LastEdit == null)
                    {
                        lastEditString = null;
                    }
                    else
                    {
                        DateTime tempDate = (DateTime)content.LastEdit;
                        lastEditString = tempDate.ToString("yyyy-MM-dd");
                    }
                    return Page();
                }
                
            }

            string uniqueFileName = null;
            if (_formEditContentModel.Media != null)
            {
                string strPhysicalFolder = "wwwroot";
                FileInfo file = new FileInfo(strPhysicalFolder + ContentToUpdate.Media);
                if (file.Exists)
                {
                    file.Delete();
                }
                string uploadsFolder = Path.Combine("wwwroot/img/Content");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + _formEditContentModel.Media.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    _formEditContentModel.Media.CopyTo(fileStream);
                }
            }

            ContentToUpdate.Title = _formEditContentModel.Title;
            ContentToUpdate.Description = _formEditContentModel.Description;
            ContentToUpdate.Text = _formEditContentModel.Text;
            ContentToUpdate.PubblicationDate = _formEditContentModel.PubblicationDate;
            if (_formEditContentModel.Media != null)
            {
                ContentToUpdate.Media = pathName + uniqueFileName;
            }

            DateTime tempPubblicationDate=DateTime.Now.AddYears(-1).Date;
            if (_formEditContentModel.PubblicationDate!=null)
            {
                tempPubblicationDate = (DateTime)_formEditContentModel.PubblicationDate;
            }
            if (DateTime.Now.Date > tempPubblicationDate.Date)
            {
                ContentToUpdate.PubblicationDate = null;
            }

            /*ContentAttributes*/


            if (_formEditContentModel.ContentAttributes == null)
            {
                _formEditContentModel.ContentAttributes = new List<int>();
            }
            var tempContentAttributes = new Models.ContentAttributes();
            IQueryable<int> selectAttributesIdQuery = from ContentAttributes in _context.ContentAttributes where ContentAttributes.ContentId == contentId select ContentAttributes.AttributesId;
            List<int> selectAttributesIdList = selectAttributesIdQuery.ToList<int>();
            foreach (int i in _formEditContentModel.ContentAttributes)
            {
                if (!selectAttributesIdList.Contains(i))
                {
                    var entryContentAttributes = _context.Add(new Models.ContentAttributes());
                    tempContentAttributes.AttributesId = i;
                    tempContentAttributes.ContentId = contentId;
                    entryContentAttributes.CurrentValues.SetValues(tempContentAttributes);
                    lastEditAttributes = await _context.SaveChangesAsync();
                    if (lastEditAttributes <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nella modifica");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsModifiedWarningCode, "L'utente " + User.Identity.Name + " ha modificato un contenuto.", "Warning - No Last Edited Attribute(s)", HttpContext);
                        return Page();
                    }
                }
                else
                {
                    selectAttributesIdList.Remove(i);
                }
                    
            }
            if(selectAttributesIdList.Count > 0)
            {

                foreach(int i in selectAttributesIdList)
                {
                    IQueryable<ContentAttributes> selectAttributesRemainIdQuery = from ContentAttributes in _context.ContentAttributes where (ContentAttributes.ContentId == contentId && ContentAttributes.AttributesId==i) select ContentAttributes;
                    ContentAttributes ContentAttributesToDelete = selectAttributesRemainIdQuery.ToList<ContentAttributes>().First<ContentAttributes>();
                    _context.ContentAttributes.Remove(ContentAttributesToDelete);
                }
            }
            

            /*ContentTag*/
            if (_formEditContentModel.ContentTag == null)
            {
                _formEditContentModel.ContentTag = new List<int>();
            }
            var tempContentTag = new Models.ContentTag();
            IQueryable<int> selectTagIdQuery = from ContentTag in _context.ContentTag where ContentTag.ContentId == contentId select ContentTag.TagId;
            List<int> selectTagIdList = selectTagIdQuery.ToList<int>();
            foreach (int i in _formEditContentModel.ContentTag)
            {
                if (!selectTagIdQuery.Contains(i))
                {
                    var entryContentTag = _context.Add(new Models.ContentTag());
                    tempContentTag.TagId = i;
                    tempContentTag.ContentId = contentId;
                    entryContentTag.CurrentValues.SetValues(tempContentTag);
                    lastEditTag = await _context.SaveChangesAsync();
                    if (lastEditTag <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nella modifica");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsModifiedWarningCode, "L'utente " + User.Identity.Name + " ha modificato un contenuto.", "Warning - No Last Edited Tag(s)", HttpContext);
                        return Page();
                    }
                }
                else
                {
                    selectTagIdList.Remove(i);
                }
            }
            if (selectTagIdList.Count > 0)
            {
                foreach (int i in selectTagIdList)
                {
                    IQueryable<ContentTag> selectTagRemainIdQuery = from ContentTag in _context.ContentTag where (ContentTag.ContentId == contentId && ContentTag.TagId == i) select ContentTag;
                    ContentTag ContentTagToDelete = selectTagRemainIdQuery.ToList<ContentTag>().First<ContentTag>();
                    _context.ContentTag.Remove(ContentTagToDelete);
                }
            }
            

            /*ContentCategory*/
            if (_formEditContentModel.ContentCategory == null)
            {
                _formEditContentModel.ContentCategory = new List<int>();
            }
            var tempContentCategory = new Models.ContentCategory();
            IQueryable<int> selectCategoryIdQuery = from Contentcategory in _context.ContentCategory where Contentcategory.ContentId == contentId select Contentcategory.CategoryId;
            List<int> selectCategoryIdList = selectCategoryIdQuery.ToList<int>();
            foreach (int i in _formEditContentModel.ContentCategory)
            {
                if (!selectCategoryIdQuery.Contains(i))
                {
                    var entryContentCategory = _context.Add(new Models.ContentCategory());
                    tempContentCategory.CategoryId = i;
                    tempContentCategory.ContentId = contentId;
                    entryContentCategory.CurrentValues.SetValues(tempContentCategory);
                    lastEditCategory = await _context.SaveChangesAsync();
                    if (lastEditCategory <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nella modifica");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsModifiedWarningCode, "L'utente " + User.Identity.Name + " ha modificato un contenuto.", "Warning - No Last Edited Category(ies)", HttpContext);
                        return Page();
                    }
                }
                else
                {
                    selectCategoryIdList.Remove(i);
                }
            }
            if (selectCategoryIdList.Count > 0)
            {
                foreach (int i in selectCategoryIdList)
                {
                    IQueryable<ContentCategory> selectCategoryRemainIdQuery = from ContentCategory in _context.ContentCategory where (ContentCategory.ContentId == contentId && ContentCategory.CategoryId == i) select ContentCategory;
                    ContentCategory ContentCategoryToDelete = selectCategoryRemainIdQuery.ToList<ContentCategory>().First<ContentCategory>();
                    _context.ContentCategory.Remove(ContentCategoryToDelete);
                }
            }
            

            ContentToUpdate.InsertionDate = _formEditContentModel.InsertionDate;
            ContentToUpdate.LastEdit = DateTime.Now.Date;

            /*start ContentLocation*/

            int? nation = (_formEditContentModel.NationAdd == null || _formEditContentModel.NationAdd <= 0) ? null : _formEditContentModel.NationAdd;
            if (nation != null)
            {


                int? region = (_formEditContentModel.RegionAdd == null || _formEditContentModel.RegionAdd <= 0) ? null : _formEditContentModel.RegionAdd;
                int? province = (_formEditContentModel.ProvinceAdd == null || _formEditContentModel.ProvinceAdd <= 0) ? null : _formEditContentModel.ProvinceAdd;
                string? city = _formEditContentModel.CityAdd == null ? null : _formEditContentModel.CityAdd;
                var is_exists = LocationAvailable.Where(c => c.NationId == nation
                                                  && c.RegionId == region
                                                  && c.ProvinceId == province
                                                  && c.City == city).ToList();
                int? locationId = null;
                int? contentLocationId = null;
                if (is_exists.Count() > 0)
                {
                    locationId = is_exists.First().LocationId;
                }
                if (locationId != null)
                {
                    var is_present = ContentLocationAvailable.Where(c => c.ContentId == contentId
                                                  && c.LocationId == locationId).ToList();
                    if (is_present.Count() > 0)
                    {
                        contentLocationId = is_present.First().Id;
                    }
                }
                if (contentLocationId == null)
                {
                    if (locationId == null)
                    {
                        var entryLocation = _context.Add(new Location());
                        Location tempLocation = new Location(nation, region, province, city);
                        entryLocation.CurrentValues.SetValues(tempLocation);
                        int j = await _context.SaveChangesAsync();
                        if (j <= 0)
                        {
                            ModelState.AddModelError("Make", "Errore nella modifica");
                            _logController.SaveLog(User.Identity.Name, LogListController.ContentsModifiedWarningCode, "L'utente " + User.Identity.Name + " ha modificato un contenuto.", "Warning - Saving Change(s) Error", HttpContext);
                            return Page();
                        }
                        locationId = (from Location in _context.Location
                                      where (Location.NationId == nation
                                     && Location.RegionId == region
                                     && Location.ProvinceId == province
                                     && Location.City == city)
                                      select Location.LocationId).ToList().First();
                    }
                    var tempContentLocation = new Models.ContentLocation();
                    var entryContentLocation = _context.Add(new Models.ContentLocation());
                    tempContentLocation.LocationId = (int)locationId;
                    tempContentLocation.ContentId = contentId;

                    entryContentLocation.CurrentValues.SetValues(tempContentLocation);
                    lastEditLocation = await _context.SaveChangesAsync();
                    if (lastEditLocation <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsDeletedWarningCode, "L'utente " + User.Identity.Name + " ha eliminato un contenuto.", "Warning - No Last Deleted Location(s)", HttpContext);
                        return Page();
                    }
                }
            }

            /*end ContentLocation*/

            lastEdit = await _context.SaveChangesAsync();

            selectContentQueryOrder = from Content in _context.Content select Content;
            selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            content = await _context.Content.FindAsync(contentId);
            if (content.PubblicationDate == null)
            {
                PubblicationDateString = null;
            }
            else
            {
                DateTime tempDate = (DateTime)content.PubblicationDate;
                PubblicationDateString = tempDate.ToString("yyyy-MM-dd");
            }

            if (content.InsertionDate == null)
            {
                InsertionDateString = null;
            }
            else
            {
                InsertionDateString = content.InsertionDate.ToString("yyyy-MM-dd");
            }

            if (content.LastEdit == null)
            {
                lastEditString = null;
            }
            else
            {
                DateTime tempDate = (DateTime)content.LastEdit;
                lastEditString = tempDate.ToString("yyyy-MM-dd");
            }
            _logController.SaveLog(User.Identity.Name, LogListController.ContentsModifiedCode, "L'utente " + User.Identity.Name + " ha modificato un contenuto.", "Content(s) Modified", HttpContext);
            return RedirectToPage("./EditContent" , new { id=contentId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int contentId)
        {
            lastEdit = 0;
            lastEditAttributes = 0;
            lastEditTag = 0;
            lastEditCategory = 0;
            lastEditLocation = 0;
            lastDeleteLocation = 0;
            callDelete = true;
            if (_formDeleteContentModel.LocationDelete!=null && _formDeleteContentModel.LocationDelete.Count() > 0)
            {
                ContentLocationAvailable = ContentLocationAvailable.Where(c => _formDeleteContentModel.LocationDelete.Contains(c.LocationId)).ToList();
                if (ContentLocationAvailable.Count() > 0)
                {
                    foreach (var l in ContentLocationAvailable)
                    {
                        _context.ContentLocation.Remove(l);
                        
                    }
                    lastDeleteLocation = await _context.SaveChangesAsync();
                    if (lastDeleteLocation <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        _logController.SaveLog(User.Identity.Name, LogListController.ContentsDeletedWarningCode, "L'utente " + User.Identity.Name + " ha eliminato un contenuto.", "Warning - No Last Deleted Location(s)", HttpContext);
                        return Page();
                    }
                }
            }

            //lastEdit = await _context.SaveChangesAsync();

            selectContentQueryOrder = from Content in _context.Content select Content;
            selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            content = await _context.Content.FindAsync(contentId);
            if (content.PubblicationDate == null)
            {
                PubblicationDateString = null;
            }
            else
            {
                DateTime tempDate = (DateTime)content.PubblicationDate;
                PubblicationDateString = tempDate.ToString("yyyy-MM-dd");
            }

            if (content.InsertionDate == null)
            {
                InsertionDateString = null;
            }
            else
            {
                InsertionDateString = content.InsertionDate.ToString("yyyy-MM-dd");
            }

            if (content.LastEdit == null)
            {
                lastEditString = null;
            }
            else
            {
                DateTime tempDate = (DateTime)content.LastEdit;
                lastEditString = tempDate.ToString("yyyy-MM-dd");
            }
            _logController.SaveLog(User.Identity.Name, LogListController.ContentsDeletedCode, "L'utente " + User.Identity.Name + " ha eliminato un contenuto.", "Content Deleted", HttpContext);
            return RedirectToPage("./EditContent", new { id = contentId });
        }
    }
}
