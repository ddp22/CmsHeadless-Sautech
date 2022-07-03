using CmsHeadless.Models;
using CmsHeadless.ViewModels.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CmsHeadless.Pages.Content
{

    public class EditContentModel : PageModel
    {
        IQueryable<Models.Content> selectContentQuery;
        public static int EditContentId=0;
        public static int lastEdit = 0;
        public static int lastEditAttributes = 0;
        public static int lastEditTag = 0;
        public static int lastEditCategory = 0;
        public Models.Content content;
        public Models.Content EditContentNew { get; set; }
        [BindProperty]
        public EditContentViewModel _formEditContentModel { get; set; }
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

        public EditContentModel(CmsHeadlessDbContext context)
        {
            _context = context;
            ContentAvailable= new List<Models.Content>();


            IQueryable<Models.Attributes> selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();

            IQueryable<Models.Category> selectCategoryQuery = from Category in _context.Category select Category;
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();

            IQueryable<Models.Tag> selectTagQuery = from Tag in _context.Tag select Tag;
            TagAvailable = selectTagQuery.ToList<Models.Tag>();

            AttributesSelected = new List<int>();
            TagSelected = new List<int>();
            CategorySelected= new List<int>();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            selectContentQuery = from Content in _context.Content select Content;
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

            var tempContentAttributes = new Models.ContentAttributes();
            IQueryable<Models.ContentAttributes> selectContentAttributesQuery = from ContentAttributes in _context.ContentAttributes where ContentAttributes.ContentId == id select ContentAttributes;
            if (selectContentAttributesQuery != null)
            {
                foreach(var i in selectContentAttributesQuery.ToList<ContentAttributes>())
                {
                    AttributesSelected.Add(i.AttributesId);
                }
            }

            var tempContentTag = new Models.ContentTag();
            IQueryable<Models.ContentTag> selectContentTagQuery = from ContentTag in _context.ContentTag where ContentTag.ContentId == id select ContentTag;
            if (selectContentTagQuery != null)
            {
                foreach (var i in selectContentTagQuery.ToList<ContentTag>())
                {
                    TagSelected.Add(i.TagId);
                }
            }

            var tempContentCategory = new Models.ContentCategory();
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
                    selectContentQuery = from Content in _context.Content select Content;
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
                ContentToUpdate.Media = uniqueFileName;
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

            
            if (_formEditContentModel.ContentAttributes != null)
            {
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
            }

            /*ContentTag*/
            if (_formEditContentModel.ContentTag != null)
            {
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
            }

            /*ContentCategory*/
            if (_formEditContentModel.ContentCategory != null)
            {
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
            }

            ContentToUpdate.InsertionDate = _formEditContentModel.InsertionDate;
            ContentToUpdate.LastEdit = DateTime.Now.Date;
            lastEdit = await _context.SaveChangesAsync();

            selectContentQuery = from Content in _context.Content select Content;
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
            return RedirectToPage("./EditContent" , new { id=contentId });
        }
    }
}
