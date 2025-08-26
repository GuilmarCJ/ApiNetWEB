using System.Diagnostics;
using InventarioWeb.Models;
using InventarioWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMaterialService _materialService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMaterialService materialService, ILogger<HomeController> logger)
        {
            _materialService = materialService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 50, string local = null, string search = null)
        {
            try
            {
                var materiales = await _materialService.GetMaterialesAsync(page, pageSize, local);

                // Si no hay materiales o el servicio devuelve null, redirigir al login
                if (materiales == null)
                {
                    _logger.LogWarning("No se pudieron obtener los materiales. Redirigiendo al login.");
                    return RedirectToAction("Login", "Auth");
                }

                // Filtrar por búsqueda si se especifica
                if (!string.IsNullOrEmpty(search) && materiales.Materiales != null)
                {
                    search = search.ToLower();
                    materiales.Materiales = materiales.Materiales
                        .Where(m =>
                            (m.Descripcion?.ToLower().Contains(search) ?? false) ||
                            (m.MaterialCod?.ToLower().Contains(search) ?? false) ||
                            (m.Lote?.ToLower().Contains(search) ?? false) ||
                            (m.Almacen?.ToLower().Contains(search) ?? false) ||
                            (m.Local?.ToLower().Contains(search) ?? false) ||
                            (m.Obs?.ToLower().Contains(search) ?? false) ||
                            (m.Ubicacion?.ToLower().Contains(search) ?? false) ||
                            (m.Usuario?.ToLower().Contains(search) ?? false))
                        .ToList();

                    materiales.TotalCount = materiales.Materiales.Count;
                    materiales.TotalPages = (int)Math.Ceiling(materiales.TotalCount / (double)pageSize);
                }

                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.SelectedLocal = local;
                ViewBag.SearchTerm = search;
                ViewBag.Locales = await _materialService.GetLocalesAsync();

                return View(materiales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener materiales");
                return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
