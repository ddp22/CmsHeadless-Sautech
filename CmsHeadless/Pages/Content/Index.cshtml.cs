using CmsHeadless.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using CmsHeadless.ViewModels.Content;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Content
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public const int numberPage = 5;
        [BindProperty]
        public ContentViewModel _formContentModel { get; set; }
        public static int lastCreate = 0;
        public static int lastDelete = 0;
        

        [BindProperty]
        public Models.Content ContentNew { get; set; }
        public List<Models.Content> ContentAvailable { get; set; }

        public List<Models.Attributes> AttributesAvailable { get; set; }
        public List<Models.Category> CategoryAvailable { get; set; }
        public List<Models.Tag> TagAvailable { get; set; }

        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            ContentAvailable = new List<Models.Content>();

            IQueryable<Models.Attributes> selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable=selectAttributesQuery.ToList<Models.Attributes>();

            IQueryable<Models.Category> selectCategoryQuery = from Category in _context.Category select Category;
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();

            IQueryable<Models.Tag> selectTagQuery = from Tag in _context.Tag select Tag;
            TagAvailable = selectTagQuery.ToList<Models.Tag>();

        }
        public ContentList<Models.Content> ContentList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            IQueryable<Models.Content> selectContentQuery = from Content in _context.Content select Content;
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

        public async Task<IActionResult> OnPostCreateAsync(int? pageIndex)
        {
            lastCreate = 0;
            lastDelete = 0;
            IQueryable<Models.Content> selectContentQuery;

            var pageSize = 5;

            int is_exsist = _context.Content.Where(c => c.Title == _formContentModel.Title).Count();

            if (is_exsist > 0)
            {
                ModelState.AddModelError("Make", "Non è stato possibile inserire il content perchè già esiste");
                selectContentQuery = from Content in _context.Content select Content;
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
                temp.Media = uniqueFileName;
            }
            temp.Text=_formContentModel.Text;
            temp.InsertionDate = DateTime.Now.Date;
            temp.LastEdit = null;
            temp.PubblicationDate = _formContentModel.PubblicationDate;
            if (DateTime.Now.Date > _formContentModel.PubblicationDate)
            {
                temp.PubblicationDate = null;
            }
            temp.UserId = Environment.UserName;

            var entry = _context.Add(new Models.Content());
            entry.CurrentValues.SetValues(temp);
            lastCreate = await _context.SaveChangesAsync();
            if (lastCreate <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }

            selectContentQuery = from Content in _context.Content where Content.Title==temp.Title select Content;
            Models.Content tempContent = selectContentQuery.ToList<Models.Content>().First<Models.Content>();

            /*ContentAttributes*/
            if (_formContentModel.ContentAttributes != null)
            {
                var tempContentAttributes = new Models.ContentAttributes();
                temp.ContentAttributes = new List<Models.ContentAttributes>();
                foreach (int i in _formContentModel.ContentAttributes)
                {
                    var entryContentAttributes = _context.Add(new Models.ContentAttributes());
                    tempContentAttributes.AttributesId = i;
                    tempContentAttributes.ContentId = tempContent.ContentId;

                    temp.ContentAttributes.Add(tempContentAttributes);

                    entryContentAttributes.CurrentValues.SetValues(tempContentAttributes);
                    int k = await _context.SaveChangesAsync();
                    if (k <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        return Page();
                    }
                }
            }

            /*ContentTag*/
            if (_formContentModel.ContentTag != null)
            {
                var tempContentTag = new Models.ContentTag();
                temp.ContentTag = new List<Models.ContentTag>();
                foreach (int i in _formContentModel.ContentTag)
                {
                    var entryContentTag = _context.Add(new Models.ContentTag());
                    tempContentTag.TagId = i;
                    tempContentTag.ContentId = tempContent.ContentId;

                    temp.ContentTag.Add(tempContentTag);

                    entryContentTag.CurrentValues.SetValues(tempContentTag);
                    int k = await _context.SaveChangesAsync();
                    if (k <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        return Page();
                    }
                }
            }

            /*ContentCategory*/
            if (_formContentModel.ContentCategory != null)
            {
                var tempContentCategory = new Models.ContentCategory();
                temp.ContentCategory = new List<Models.ContentCategory>();
                foreach (int i in _formContentModel.ContentCategory)
                {
                    var entryContentCategory = _context.Add(new Models.ContentCategory());
                    tempContentCategory.CategoryId = i;
                    tempContentCategory.ContentId = tempContent.ContentId;

                    temp.ContentCategory.Add(tempContentCategory);

                    entryContentCategory.CurrentValues.SetValues(tempContentCategory);
                    int k = await _context.SaveChangesAsync();
                    if (k <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        return Page();
                    }
                }
            }


            selectContentQuery = from Content in _context.Content select Content;
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            pageSize = Configuration.GetValue("PageSize", numberPage);
            ContentList = await ContentList<Models.Content>.CreateAsync(selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            
            return RedirectToPage("./Index");
        }




        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? contentId)
        {
            lastDelete = 0;
            lastCreate = 0;
            if (contentId == null)
            {
                return NotFound();
            }

            var content = await _context.Content.FindAsync(contentId);

            IQueryable<Models.Content> selectContentQuery;
            if (content == null)
            {
                return NotFound();
            }
            _context.Content.Remove(content);
            lastDelete = await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'eliminazione");
                return Page();
            }
            selectContentQuery = from Content in _context.Content select Content;
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            ContentList = await ContentList<Models.Content>.CreateAsync(selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./Index");
        }
    }
}
