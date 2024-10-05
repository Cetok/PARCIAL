using Microsoft.AspNetCore.Mvc;
using PARCIAL.Data;
using PARCIAL.Models;
using PARCIAL.Services;
using System.Threading.Tasks;

namespace PARCIAL.Controllers
{
    public class RemesasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrencyConversionService _currencyService;

        public RemesasController(ApplicationDbContext context, ICurrencyConversionService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Listado()
        {
            var remesas = _context.DataRemesas.ToList();
            return View(remesas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Remesas remesa)
        {
            if (ModelState.IsValid)
            {
                remesa.TasaCambio = await _currencyService.GetExchangeRateAsync(remesa.TipoMoneda, remesa.TipoMoneda == "USD" ? "BTC" : "USD");
                remesa.MontoFinal = remesa.MontoEnviado * (remesa.TipoMoneda == "USD" ? remesa.TasaCambio : 1 / remesa.TasaCambio);

                _context.DataRemesas.Add(remesa);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Listado));
            }
            return View("Index", remesa);
        }
    }
}