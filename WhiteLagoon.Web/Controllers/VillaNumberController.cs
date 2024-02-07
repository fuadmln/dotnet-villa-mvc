using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class VillaNumberController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public IActionResult Index()
    {
        var villaNumbers = _context.VillaNumbers.Include(vn => vn.Villa).ToList();
        return View(villaNumbers);
    }

    public IActionResult Create()
    {
        IEnumerable<SelectListItem> villas = _context.Villas.ToList().Select(v => new SelectListItem
        { 
            Text = v.Name,
            Value = v.Id.ToString()
        });

        VillaNumberVM villaNumberVM = new()
        { 
            Villas = villas
	    };

        return View(villaNumberVM);
    }

    [HttpPost]
    public IActionResult Create(VillaNumber villaNumber)
    {
        ModelState.Remove("Villa");
        if (!ModelState.IsValid)
            return View();

        _context.VillaNumbers.Add(villaNumber);
        _context.SaveChanges();
        TempData["success"] = "The villa number has been created successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Update(int villaNumberId)
    {
        VillaNumber? villaNumber = _context.VillaNumbers.Find(villaNumberId);
        if (villaNumber == null)
            return RedirectToAction("Error", "Home");

        return View(villaNumber);
    }

	[HttpPost]
	public IActionResult Update(VillaNumber villaNumber)
	{
		if (!ModelState.IsValid)
			return View();

		_context.VillaNumbers.Update(villaNumber);
		_context.SaveChanges();
        TempData["success"] = "The villa has been updated successfully";

        return RedirectToAction("Index");
	}

    public IActionResult Delete(int villaNumberId)
    {
		VillaNumber? villaNumber = _context.VillaNumbers.Find(villaNumberId);
		if (villaNumber == null)
			return RedirectToAction("Error", "Home");

		return View(villaNumber);
    }

	[HttpPost]
    public IActionResult Delete(VillaNumber villaNumber)
    {
		VillaNumber? villaNumberDb = _context.VillaNumbers.Find(villaNumber.Villa_Number);
        if (villaNumberDb == null)
            return RedirectToAction("Error", "Home");

        _context.VillaNumbers.Remove(villaNumberDb);
        _context.SaveChanges();
        TempData["success"] = "The villa has been deleted successfully";

        return RedirectToAction("Index");
    }
}
