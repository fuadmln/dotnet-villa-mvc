using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers;

public class VillaController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public IActionResult Index()
    {
        var villas = _context.Villas.ToList();
        return View(villas);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Villa villa)
    {
        if (villa.Name == villa.Description)
            ModelState.AddModelError("Name", "The description can't be match the name");

        if (!ModelState.IsValid)
            return View();

        _context.Villas.Add(villa);
        _context.SaveChanges();
        TempData["success"] = "The villa has been created successfully";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int villaId)
    {
        Villa? villa = _context.Villas.Find(villaId);
        if (villa == null)
            return RedirectToAction("Error", "Home");

        return View(villa);
    }

	[HttpPost]
	public IActionResult Update(Villa villa)
	{
		if (!ModelState.IsValid)
			return View();

		_context.Villas.Update(villa);
		_context.SaveChanges();
        TempData["success"] = "The villa has been updated successfully";

        return RedirectToAction(nameof(Index));
	}

    public IActionResult Delete(int villaId)
    {
		Villa? villa = _context.Villas.Find(villaId);
		if (villa == null)
			return RedirectToAction("Error", "Home");

		return View(villa);
    }

	[HttpPost]
    public IActionResult Delete(Villa villa)
    {
		Villa? villaDb = _context.Villas.Find(villa.Id);
        if (villaDb == null)
            return RedirectToAction("Error", "Home");

        _context.Villas.Remove(villaDb);
        _context.SaveChanges();
        TempData["success"] = "The villa has been deleted successfully";

        return RedirectToAction(nameof(Index));
    }
}
