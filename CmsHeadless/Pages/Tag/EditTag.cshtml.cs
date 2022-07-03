using CmsHeadless.ViewModels.Tag;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using Microsoft.AspNetCore.Authorization;

namespace CmsHeadless.Pages.Tag
{
    [Authorize]
    public class EditTagModel : PageModel
    {
        IQueryable<Models.Tag> selectTagQuery;
        public static int EditTagId = 0;
        public static int lastEdit = 0;
        public Models.Tag tag;
        public Models.Tag EditTagNew { get; set; }
        [BindProperty]
        public EditTagViewModel _formEditTagModel { get; set; }
        private readonly Models.CmsHeadlessDbContext _context;
        public List<Models.Tag> TagAvailable { get; set; }

        public EditTagModel(CmsHeadlessDbContext context)
        {
            _context = context;
            TagAvailable = new List<Models.Tag>();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            selectTagQuery = from Tag in _context.Tag select Tag;
            TagAvailable = selectTagQuery.ToList<Models.Tag>();
            if (id == null)
            {
                if (EditTagId != 0)
                {
                    id = EditTagId;
                }
                else
                {
                    return NotFound();
                }

            }
            EditTagId = (int)id;
            if (_context == null || _context.Tag == null)
            {
                return NotFound();
            }
            tag = await _context.Tag.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int tagId)
        {
            lastEdit = 0;
            var tagToUpdate = await _context.Tag.FindAsync(tagId);

            if (tagToUpdate == null)
            {
                return NotFound();
            }

            if (tagToUpdate.Name != _formEditTagModel.Name)
            {
                var tagToSearch = from Tag in _context.Tag
                                       where Tag.Name == _formEditTagModel.Name
                                       select Tag;
                if (tagToSearch.Count<Models.Tag>() != 0)
                {
                    ModelState.AddModelError("Make", "Tag già esistente. Inserirne un altro");
                    selectTagQuery = from Tag in _context.Tag select Tag;
                    TagAvailable = selectTagQuery.ToList<Models.Tag>();
                    tag = await _context.Tag.FindAsync(tagId);

                    return Page();
                }

            }

            

            tagToUpdate.Name = _formEditTagModel.Name;
            tagToUpdate.Url = _formEditTagModel.Url;
            
            lastEdit = await _context.SaveChangesAsync();

            selectTagQuery = from Tag in _context.Tag select Tag;
            TagAvailable = selectTagQuery.ToList<Models.Tag>();
            tag = await _context.Tag.FindAsync(tagId);
            return RedirectToPage("./EditTag", new { id = tagId });
        }
    }
}
