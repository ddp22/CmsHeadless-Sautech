using CmsHeadless.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Attributes
{
    [Authorize]
    public class ViewAttributesModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public static int lastDelete = 0;
        public Models.Attributes AttributesNew { get; set; }

        public List<Models.Attributes> attributesAvailable { get; set; }

        public ViewAttributesModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            attributesAvailable = new List<Models.Attributes>();
        }


        public AttributesList<Models.Attributes> AttributesList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {

            IQueryable<Models.Attributes> selectAttributesQuery = from Attributes in _context.Attributes select Attributes;
            attributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            AttributesList = await AttributesList<Models.Attributes>.CreateAsync(
                selectAttributesQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? attributesId)
        {
            lastDelete = 0;
            if (attributesId == null)
            {
                return NotFound();
            }

            var Attributes = await _context.Attributes.FindAsync(attributesId);

            IQueryable<Models.Attributes> selectAttributesQuery;
            if (Attributes == null)
            {
                return NotFound();
            }
            _context.Attributes.Remove(Attributes);
            lastDelete = await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }
            selectAttributesQuery = from attributes in _context.Attributes select Attributes;
            attributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            AttributesList = await AttributesList<Models.Attributes>.CreateAsync(selectAttributesQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./ViewAttributes");
        }

    }
}
