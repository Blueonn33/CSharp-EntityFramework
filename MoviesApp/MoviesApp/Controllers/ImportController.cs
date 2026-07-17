using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services.Interfaces;
using MoviesApp.ViewModels.Import;

namespace MiniCinemaApp.Controllers
{
    public class ImportController : Controller
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ImportIndexViewModel();

            if (TempData.ContainsKey("JsonImportedCount"))
            {
                model.JsonImportedCount = (int)TempData["JsonImportedCount"]!;
            }

            if (TempData.ContainsKey("XmlImportedCount"))
            {
                model.XmlImportedCount = (int)TempData["XmlImportedCount"]!;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ImportJson()
        {
            // TODO (students): implement real import logic in ImportService
            // int importedCount = _importService.ImportMoviesFromJson();
            int importedCount = 0; // placeholder for now

            TempData["JsonImportedCount"] = importedCount;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ImportXml()
        {
            // TODO (students): implement real import logic in ImportService
            // int importedCount = _importService.ImportMoviesFromXml();
            int importedCount = 0; // placeholder for now

            TempData["XmlImportedCount"] = importedCount;

            return RedirectToAction(nameof(Index));
        }
    }
}
