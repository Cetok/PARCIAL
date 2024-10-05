using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PARCIAL.Data;
using PARCIAL.Models;

namespace PARCIAL.Controllers
{
    [Route("[controller]")]
    public class RemesasController : Controller
    {
        private readonly ILogger<RemesasController> _logger;
        private readonly ApplicationDbContext _context;

        public RemesasController(ILogger<RemesasController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Listado")]
        public IActionResult Listado()
        {
            var remesas = _context.DataRemesas.ToList();
            return View(remesas);
        }

        [HttpPost]
        public IActionResult Create(Remesas remesa)
        {
            if (ModelState.IsValid)
            {

                remesa.MontoFinal = remesa.MontoEnviado * remesa.TasaCambio;

                _context.DataRemesas.Add(remesa);
                _context.SaveChanges();

                return RedirectToAction("Listado");
            }

            return View("Index", remesa);
        }


        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
