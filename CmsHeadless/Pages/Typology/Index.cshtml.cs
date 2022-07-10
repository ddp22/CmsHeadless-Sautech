using CmsHeadless.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Typology
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public const int numberPage = 5;
        public static int lastCreate = 0;
        public static int lastDelete = 0;
        public static bool callDelete = false;
        public static string searchString { get; set; }

        [BindProperty]
        public ViewModels.Typology.TypologyViewModel _formTypologyModel { get; set; }
        public Models.Typology TypologyNew { get; set; }
        public List<Models.Typology> TypologyAvailable { get; set; }

        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            TypologyAvailable = new List<Models.Typology>();
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
                IndexModel.searchString = searchString;
                selectTypologyQuery = selectTypologyQuery.Where(s => s.Name.Contains(searchString));
            }
            TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TypologyList = await TypologyList<Models.Typology>.CreateAsync(
                selectTypologyQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }


        public async Task<IActionResult> OnPostCreateAsync(int? pageIndex)
        {
            lastCreate = 0;
            lastDelete = 0;
            IQueryable<Models.Typology> selectTypologyQuery;
            IQueryable<Models.Typology> selectTypologyQueryOrder;

            var pageSize = 5;

            int is_exsist = _context.Typology.Where(c => c.Name == _formTypologyModel.TypologyName).Count();

            if (is_exsist > 0)
            {
                ModelState.AddModelError("Make", "Non è stato possibile inserire la tipologia perchè già esiste");
                selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
                selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
                TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();
                if (pageIndex == null)
                {
                    pageIndex = 1;
                }
                pageSize = Configuration.GetValue("PageSize", numberPage);
                TypologyList = await TypologyList<Models.Typology>.CreateAsync(selectTypologyQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
                return Page();
            }

            Models.Typology temp = new Models.Typology();
            temp.Name = _formTypologyModel.TypologyName;
            var entry = _context.Add(new Models.Typology());
            entry.CurrentValues.SetValues(temp);
            lastCreate = await _context.SaveChangesAsync();
            if (lastCreate <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }

            selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
            selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
            TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            pageSize = Configuration.GetValue("PageSize", numberPage);
            TypologyList = await TypologyList<Models.Typology>.CreateAsync(selectTypologyQuery.AsNoTracking(), pageIndex ?? 1, pageSize);

            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? typologyId)
        {
            lastDelete = 0;
            lastCreate = 0;
            callDelete = true;
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
                ModelState.AddModelError("Make", "Errore nell'eliminazione");
                return Page();
            }
            selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
            selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
            TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TypologyList = await TypologyList<Models.Typology>.CreateAsync(selectTypologyQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./Index");
        }
    }
}
