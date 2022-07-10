using CmsHeadless.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Typology
{
    [Authorize]
    public class ViewTypologyModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public static int lastDelete = 0;
        public static string searchString { get; set; }
        public Models.Typology TypologyNew { get; set; }

        public List<Models.Typology> typologyAvailable { get; set; }

        public ViewTypologyModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            typologyAvailable = new List<Models.Typology>();
        }


        public TypologyList<Models.Typology> TypologyList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? searchString)
        {
            IQueryable<Models.Typology> selectTypologyQuery;
            IQueryable<Models.Typology> selectTypologyQueryOrder;

            selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
            selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
            if (!string.IsNullOrEmpty(searchString))
            {
                ViewTypologyModel.searchString = searchString;
                selectTypologyQuery = selectTypologyQuery.Where(s => (s.Name.Contains(searchString)));
            }
            typologyAvailable = selectTypologyQuery.ToList<Models.Typology>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TypologyList = await TypologyList<Models.Typology>.CreateAsync(
                selectTypologyQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? typologyId)
        {
            lastDelete = 0;
            if (typologyId == null)
            {
                return NotFound();
            }

            var typology = await _context.Typology.FindAsync(typologyId);

            IQueryable<Models.Typology> selectTypologyQuery;
            IQueryable<Models.Typology> selectTypologyQueryOrder;

            if (typology == null)
            {
                return NotFound();
            }
            _context.Typology.Remove(typology);
            lastDelete = await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }
            selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
            selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
            typologyAvailable = selectTypologyQueryOrder.ToList<Models.Typology>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TypologyList = await TypologyList<Models.Typology>.CreateAsync(selectTypologyQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./ViewTypology");
        }
    }
}
