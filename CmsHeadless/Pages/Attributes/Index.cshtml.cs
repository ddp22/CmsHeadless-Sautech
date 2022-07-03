using CmsHeadless.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Attributes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public const int numberPage = 5;
        public static int lastCreate = 0;
        public static int lastDelete = 0;

        [BindProperty]
        public ViewModels.Attributes.AttributesViewModel _formAttributesModel { get; set; }
        public Models.Attributes AttributesNew { get; set; }
        public List<Models.Attributes> AttributesAvailable { get; set; }

        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            AttributesAvailable = new List<Models.Attributes>();
        }

        public AttributesList<Models.Attributes> AttributesList { get; set; }


        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            IQueryable<Models.Attributes> selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            AttributesList = await AttributesList<Models.Attributes>.CreateAsync(
                selectAttributesQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(int? pageIndex)
        {
            lastCreate = 0;
            lastDelete = 0;
            IQueryable<Models.Attributes> selectAttributesQuery;

            var pageSize = 5;

            int is_exsist = _context.Attributes.Where(c => c.AttributeName == _formAttributesModel.AttributeName).Count();

            if (is_exsist > 0)
            {
                ModelState.AddModelError("Make", "Non è stato possibile inserire l'attributo perchè già esiste");
                selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
                AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
                if (pageIndex == null)
                {
                    pageIndex = 1;
                }
                pageSize = Configuration.GetValue("PageSize", numberPage);
                AttributesList = await AttributesList<Models.Attributes>.CreateAsync(selectAttributesQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
                return Page();
            }

            Models.Attributes temp = new Models.Attributes();
            temp.AttributeName = _formAttributesModel.AttributeName;
            temp.AttributeValue = _formAttributesModel.AttributeValue;
            var entry = _context.Add(new Models.Attributes());
            entry.CurrentValues.SetValues(temp);
            lastCreate = await _context.SaveChangesAsync();
            if (lastCreate <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }

            selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            pageSize = Configuration.GetValue("PageSize", numberPage);
            AttributesList = await AttributesList<Models.Attributes>.CreateAsync(selectAttributesQuery.AsNoTracking(), pageIndex ?? 1, pageSize);

            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? attributesId)
        {
            lastDelete = 0;
            lastCreate = 0;
            if (attributesId == null)
            {
                return NotFound();
            }

            var attributes = await _context.Attributes.FindAsync(attributesId);

            IQueryable<Models.Attributes> selectAttributesQuery;
            if (attributes == null)
            {
                return NotFound();
            }
            _context.Attributes.Remove(attributes);
            lastDelete = await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'eliminazione");
                return Page();
            }
            selectAttributesQuery = from Attributes in _context.Attributes select attributes;
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            AttributesList = await AttributesList<Models.Attributes>.CreateAsync(selectAttributesQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./Index");
        }

    }
}
