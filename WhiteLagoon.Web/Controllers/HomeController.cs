using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            var villas = _unitOfWork.VillaRepo.GetAll(includeProperties: "VillaAmenities");

            HomeVM homeVm = new()
            {
                VillaList = villas,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                Nights = 1,
            };

            return View(homeVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
