using CmsHeadless.Models;
using CmsHeadless.ViewModels.Typology;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CmsHeadless.Pages.Typology
{
    [Authorize]
    public class EditTypologyModel : PageModel
    {
        IQueryable<Models.Typology> selectTypologyQuery;
        IQueryable<Models.Typology> selectTypologyQueryOrder;
        public static int EditTypologyId = 0;
        public static int lastEdit = 0;
        public Models.Typology typology;
        public Models.Typology EditTypologyNew { get; set; }
        [BindProperty]
        public EditTypologyViewModel _formEditTypologyModel { get; set; }
        private readonly Models.CmsHeadlessDbContext _context;
        public List<Models.Typology> TypologyAvailable { get; set; }

        public EditTypologyModel(CmsHeadlessDbContext context)
        {
            _context = context;
            TypologyAvailable = new List<Models.Typology>();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
            selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
            TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();
            if (id == null)
            {
                if (EditTypologyId != 0)
                {
                    id = EditTypologyId;
                }
                else
                {
                    return NotFound();
                }

            }
            EditTypologyId = (int)id;
            if (_context == null || _context.Typology == null)
            {
                return NotFound();
            }
            typology = await _context.Typology.FindAsync(id);
            if (typology == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int typologyId)
        {
            lastEdit = 0;
            var typologyToUpdate = await _context.Typology.FindAsync(typologyId);

            if (typologyToUpdate == null)
            {
                return NotFound();
            }

            if (typologyToUpdate.Name != _formEditTypologyModel.TypologyName)
            {
                var typologyToSearch = from Typology in _context.Typology
                                         where Typology.Name == _formEditTypologyModel.TypologyName
                                         select Typology;
                if (typologyToSearch.Count<Models.Typology>() != 0)
                {
                    ModelState.AddModelError("Make", "Tipologia già esistente. Inserirne un altro");
                    selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
                    selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
                    TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();
                    typology = await _context.Typology.FindAsync(typologyId);

                    return Page();
                }

            }

            typologyToUpdate.Name = _formEditTypologyModel.TypologyName;

            lastEdit = await _context.SaveChangesAsync();

            selectTypologyQueryOrder = from Typology in _context.Typology select Typology;
            selectTypologyQuery = selectTypologyQueryOrder.OrderByDescending(c => c.Id);
            TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();
            typology = await _context.Typology.FindAsync(typologyId);
            return RedirectToPage("./EditTypology", new { id = typologyId });
        }
    }
}
