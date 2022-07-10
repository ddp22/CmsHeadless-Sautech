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
        public static bool callDelete = false;
        public static string searchString { get; set; }


        [BindProperty]
        public ViewModels.Attributes.AttributesViewModel _formAttributesModel { get; set; }
        public Models.Attributes AttributesNew { get; set; }
        public List<Models.Attributes> AttributesAvailable { get; set; }
        public List<Models.Typology> TypologyAvailable { get; set; }

        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            AttributesAvailable = new List<Models.Attributes>();

            IQueryable<Models.Typology> selectTypologyQuery = from Typology in _context.Typology select Typology;
            TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();
        }

        public AttributesList<Models.Attributes> AttributesList { get; set; }


        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? searchString)
        {
            IQueryable<Models.Attributes> selectAttributesQuery;
            IQueryable<Models.Attributes> selectAttributesQueryOrder;

            selectAttributesQueryOrder = from Attributes in _context.Attributes select Attributes;
            selectAttributesQuery = selectAttributesQueryOrder.OrderByDescending(c => c.AttributesId);
            if (!string.IsNullOrEmpty(searchString))
            {
                IndexModel.searchString=searchString;
                selectAttributesQuery = selectAttributesQuery.Where(s => s.AttributeName.Contains(searchString) || s.AttributeValue.Contains(searchString));
            }
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
            IQueryable<Models.Attributes> selectAttributesQueryOrder;

            var pageSize = 5;

            int is_exsist = _context.Attributes.Where(c => c.AttributeName == _formAttributesModel.AttributeName).Count();

            if (is_exsist > 0)
            {
                ModelState.AddModelError("Make", "Non è stato possibile inserire l'attributo perchè già esiste");
                selectAttributesQueryOrder = from Attributes in _context.Attributes select Attributes;
                selectAttributesQuery = selectAttributesQueryOrder.OrderByDescending(c => c.AttributesId);
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

            selectAttributesQuery = from Attributes in _context.Attributes where Attributes.AttributeName == temp.AttributeName select Attributes;
            Models.Attributes tempAttributes = selectAttributesQuery.ToList<Models.Attributes>().First<Models.Attributes>();
            int attributesId = tempAttributes.AttributesId;
            /*AttributesTypology*/
            if (_formAttributesModel.Typology != null)
            {
                var tempAttributesTypology = new Models.AttributesTypology();
                foreach (var i in _formAttributesModel.Typology)
                {
                    var entryAttributesTypology = _context.Add(new Models.AttributesTypology());
                    tempAttributesTypology.TypologyId = i;
                    tempAttributesTypology.AttributesId = attributesId;


                    entryAttributesTypology.CurrentValues.SetValues(tempAttributesTypology);
                    int k = await _context.SaveChangesAsync();
                    if (k <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nell'inserimento");
                        return Page();
                    }
                }
            }



            selectAttributesQueryOrder = from Attributes in _context.Attributes select Attributes;
            selectAttributesQuery = selectAttributesQueryOrder.OrderByDescending(c => c.AttributesId);
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
            callDelete = true;
            if (attributesId == null)
            {
                return NotFound();
            }

            var attributes = await _context.Attributes.FindAsync(attributesId);

            IQueryable<Models.Attributes> selectAttributesQuery;
            IQueryable<Models.Attributes> selectAttributesQueryOrder;

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
            selectAttributesQueryOrder = from Attributes in _context.Attributes select Attributes;
            selectAttributesQuery = selectAttributesQueryOrder.OrderByDescending(c => c.AttributesId);
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
