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
            /*
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             */
            

            if (content == null)
            {
                return NotFound();
            }


            if (content.ContentTag != null)
            {
                foreach(var i in content.ContentTag.ToList<ContentTag>())
                {
                    TagSelected.Add(i.TagId);
                }
            }
            if (content.ContentAttributes != null)
            {
                foreach (var i in content.ContentAttributes.ToList<ContentAttributes>())
                {
                    TagSelected.Add(i.AttributesId);
                }
            }
            if (content.ContentCategory != null)
            {
                foreach (var i in content.ContentCategory.ToList<ContentCategory>())
                {
                    TagSelected.Add(i.CategoryId);
                }
            }

            return Page();
        }
/*
        public async Task<IActionResult> OnPostEditAsync(int ContentId)
        {
            lastEdit = 0;
            var ContentToUpdate = await _context.Content.FindAsync(ContentId);

            if (ContentToUpdate == null)
            {
                return NotFound();
            }

            if (ContentToUpdate.Name != _formEditContentModel.Name)
            {
                var ContentToSearch = from Content in _context.Content
                                       where Content.Name == _formEditContentModel.Name
                                       select Content;
                if (ContentToSearch.Count<Models.Content>() != 0)
                {
                    ModelState.AddModelError("Make", "Categoria già esistente. Inserirne un'altra");
                    selectContentQuery = from Content in _context.Content select Content;
                    ContentAvailable = selectContentQuery.ToList<Models.Content>();
                    Content = await _context.Content.FindAsync(ContentId);

                    CreationDate = Content.CreationDate.ToString("yyyy-MM-dd");
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

            ContentToUpdate.Name = _formEditContentModel.Name;
            ContentToUpdate.Description = _formEditContentModel.Description;
            ContentToUpdate.CreationDate = _formEditContentModel.CreationDate;
            if (_formEditContentModel.Media != null)
            {
                ContentToUpdate.Media = uniqueFileName;
            }
            if (DateTime.Now.Date > _formEditContentModel.CreationDate.Date)
            {
                ModelState.AddModelError("Make", "Inserire una data successiva a quella odierna");
                selectContentQuery = from Content in _context.Content select Content;
                ContentAvailable = selectContentQuery.ToList<Models.Content>();
                Content = await _context.Content.FindAsync(ContentId);

                CreationDate = Content.CreationDate.ToString("yyyy-MM-dd");
                return Page();
            }

            ContentToUpdate.ContentParentId = _formEditContentModel.ContentParentId;
            lastEdit = await _context.SaveChangesAsync();

            selectContentQuery = from Content in _context.Content select Content;
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            Content = await _context.Content.FindAsync(ContentId);
            CreationDate = Content.CreationDate.ToString("yyyy-MM-dd");
            return RedirectToPage("./EditContent" , new { id=ContentId });
        }*/
    }
}
