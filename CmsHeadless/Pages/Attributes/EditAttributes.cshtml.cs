using CmsHeadless.Models;
using CmsHeadless.ViewModels.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CmsHeadless.Pages.Attributes
{
    [Authorize]
    public class EditAttributesModel : PageModel
    {
        IQueryable<Models.Attributes> selectAttributesQuery;
        public static int EditAttributesId = 0;
        public static int lastEdit = 0;
        public Models.Attributes attributes;
        public Models.Attributes EditAttributesNew { get; set; }
        [BindProperty]
        public EditAttributesViewModel _formEditAttributesModel { get; set; }
        private readonly Models.CmsHeadlessDbContext _context;
        public List<Models.Attributes> AttributesAvailable { get; set; }

        public EditAttributesModel(CmsHeadlessDbContext context)
        {
            _context = context;
            AttributesAvailable = new List<Models.Attributes>();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
            if (id == null)
            {
                if (EditAttributesId != 0)
                {
                    id = EditAttributesId;
                }
                else
                {
                    return NotFound();
                }

            }
            EditAttributesId = (int)id;
            if (_context == null || _context.Attributes == null)
            {
                return NotFound();
            }
            attributes = await _context.Attributes.FindAsync(id);
            if (attributes == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int attributesId)
        {
            lastEdit = 0;
            var attributesToUpdate = await _context.Attributes.FindAsync(attributesId);

            if (attributesToUpdate == null)
            {
                return NotFound();
            }

            if (attributesToUpdate.AttributeName != _formEditAttributesModel.AttributeName)
            {
                var attributesToSearch = from Attributes in _context.Attributes
                                  where Attributes.AttributeName == _formEditAttributesModel.AttributeName
                                  select Attributes;
                if (attributesToSearch.Count<Models.Attributes>() != 0)
                {
                    ModelState.AddModelError("Make", "Attributo già esistente. Inserirne un altro");
                    selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
                    AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
                    attributes = await _context.Attributes.FindAsync(attributesId);

                    return Page();
                }

            }

            attributesToUpdate.AttributeName = _formEditAttributesModel.AttributeName;
            attributesToUpdate.AttributeValue = _formEditAttributesModel.AttributeValue;

            lastEdit = await _context.SaveChangesAsync();

            selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
            attributes = await _context.Attributes.FindAsync(attributesId);
            return RedirectToPage("./EditAttributes", new { id = attributesId });
        }
    }
}
