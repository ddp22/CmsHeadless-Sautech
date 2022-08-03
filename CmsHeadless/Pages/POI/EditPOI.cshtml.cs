using CmsHeadless.Models;
using CmsHeadless.ViewModels.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace CmsHeadless.Pages.POI
{
    [Authorize]
    public class EditPOIModel : PageModel
    {
        IQueryable<Models.Attributes> selectAttributesQuery;
        IQueryable<Models.Attributes> selectAttributesQueryOrder;
        public static int EditAttributesId = 0;
        public static int lastEdit = 0;
        public static int lastEditTypology = 0;
        public Models.Attributes attributes;
        public Models.Attributes EditAttributesNew { get; set; }
        [BindProperty]
        public EditPOIViewModel _formEditPOIModel { get; set; }
        private readonly Models.CmsHeadlessDbContext _context;
        public List<Models.Attributes> AttributesAvailable { get; set; }
        public List<Models.Typology> TypologyAvailable { get; set; }
        public List<int> TypologySelected { get; set; }
        public List<AttributesTypology> AttributesTypologySelected { get; set; }
        public List<AttributesTypology> AttributesTypology { get; set; }
        public List<Models.Content> ContentAvailable { get; set; }
        public int selectedContent { get; set; }
        public EditPOIModel(CmsHeadlessDbContext context)
        {
            _context = context;
            AttributesAvailable = new List<Models.Attributes>();

            IQueryable<Models.Typology> selectTypologyQuery = from Typology in _context.Typology select Typology;
            TypologyAvailable = selectTypologyQuery.ToList<Models.Typology>();

            TypologySelected = new List<int>();

            IQueryable<Models.AttributesTypology> selectAttributesTypologyQuery = from AttributesTypology in _context.AttributesTypology select AttributesTypology;
            AttributesTypologySelected=selectAttributesTypologyQuery.ToList<Models.AttributesTypology>();

            IQueryable<Models.AttributesTypology> AttributesTypologyQuery = from AttributesTypology in _context.AttributesTypology select AttributesTypology;
            AttributesTypology = AttributesTypologyQuery.ToList<Models.AttributesTypology>();

            IQueryable<Models.Content> ContentQuery = from Content in _context.Content select Content;
            ContentAvailable = ContentQuery.ToList<Models.Content>();

        }
        public async Task<IActionResult> OnGetAsync(int? id, string? value)
        {
            selectAttributesQueryOrder = from Attributes in _context.Attributes where Attributes.AttributeName=="POI" select Attributes;
            selectAttributesQuery = selectAttributesQueryOrder.OrderByDescending(c => c.AttributesId);
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
            if(value == null)
            {
                return NotFound();
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
            selectedContent = Int32.Parse(value);
            AttributesTypologySelected = AttributesTypologySelected.Where(c => c.AttributesId == id).ToList();
            if (AttributesTypologySelected != null && AttributesTypologySelected.Count()>0)
            {
                foreach (var i in AttributesTypologySelected)
                {
                    TypologySelected.Add(i.TypologyId);
                }
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

            if (attributesToUpdate.AttributeValue != _formEditPOIModel.AttributeValue)
            {
                var attributesToSearch = from Attributes in _context.Attributes
                                  where Attributes.AttributeName == "POI" && Attributes.AttributeValue== _formEditPOIModel.AttributeValue
                                  select Attributes;
                if (attributesToSearch.Count<Models.Attributes>() != 0)
                {
                    ModelState.AddModelError("Make", "Attributo già esistente. Inserirne un altro");
                    selectAttributesQueryOrder = from Attributes in _context.Attributes where Attributes.AttributeName == "POI" select Attributes;
                    selectAttributesQuery = selectAttributesQueryOrder.OrderByDescending(c => c.AttributesId);
                    AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
                    attributes = await _context.Attributes.FindAsync(attributesId);

                    return Page();
                }

            }

            attributesToUpdate.AttributeName = "POI";
            attributesToUpdate.AttributeValue = _formEditPOIModel.AttributeValue;

            /*AttributesTypology*/
            if (_formEditPOIModel.Typology == null)
            {
                _formEditPOIModel.Typology = new List<int>();
            }
            var tempAttributesTypology = new Models.AttributesTypology();
            IQueryable<int> selectTypologyIdQuery = from AttributesTypology in _context.AttributesTypology where AttributesTypology.AttributesId == attributesId select AttributesTypology.TypologyId;
            List<int> selectTypologyIdList = selectTypologyIdQuery.ToList<int>();
            foreach (int i in _formEditPOIModel.Typology)
            {
                if (!selectTypologyIdList.Contains(i))
                {
                    var entryAttributesTypology = _context.Add(new Models.AttributesTypology());
                    tempAttributesTypology.AttributesId = attributesId;
                    tempAttributesTypology.TypologyId = i;
                    entryAttributesTypology.CurrentValues.SetValues(tempAttributesTypology);
                    lastEditTypology = await _context.SaveChangesAsync();
                    if (lastEditTypology <= 0)
                    {
                        ModelState.AddModelError("Make", "Errore nella modifica");
                        return Page();
                    }
                }
                else
                {
                    selectTypologyIdList.Remove(i);
                }

            }
            if (selectTypologyIdList.Count > 0)
            {

                foreach (int i in selectTypologyIdList)
                {
                    IQueryable<AttributesTypology> selectTypologyRemainIdQuery = from AttributesTypology in _context.AttributesTypology where (AttributesTypology.AttributesId == attributesId && AttributesTypology.TypologyId == i) select AttributesTypology;
                    AttributesTypology AttributesTypologyToDelete = selectTypologyRemainIdQuery.ToList<AttributesTypology>().First<AttributesTypology>();
                    _context.AttributesTypology.Remove(AttributesTypologyToDelete);
                }
            }


            lastEdit = await _context.SaveChangesAsync();

            selectAttributesQueryOrder = from Attributes in _context.Attributes where Attributes.AttributeName == "POI" select Attributes;
            selectAttributesQuery = selectAttributesQueryOrder.OrderByDescending(c => c.AttributesId);
            AttributesAvailable = selectAttributesQuery.ToList<Models.Attributes>();
            attributes = await _context.Attributes.FindAsync(attributesId);
            return RedirectToPage("./EditPOI", new { id = attributesId, value=attributes.AttributeValue });
        }
    }
}
