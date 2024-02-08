using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers;

public class VillaController(IUnitOfWork unitOfWork) : Controller
{
    private readonly IUnitOfWork _uniOfWork = unitOfWork;

    public IActionResult Index()
    {
        var villas = _uniOfWork.VillaRepo.GetAll();
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

		_uniOfWork.VillaRepo.Add(villa);
		_uniOfWork.VillaRepo.Save();
        TempData["success"] = "The villa has been created successfully";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int villaId)
    {
        Villa? villa = _uniOfWork.VillaRepo.Get(villaId);
        if (villa == null)
            return RedirectToAction("Error", "Home");

        return View(villa);
    }

	[HttpPost]
	public IActionResult Update(Villa villa)
	{
		if (!ModelState.IsValid)
			return View();

		_uniOfWork.VillaRepo.Update(villa);
		_uniOfWork.VillaRepo.Save();
        TempData["success"] = "The villa has been updated successfully";

        return RedirectToAction(nameof(Index));
	}

    public IActionResult Delete(int villaId)
    {
		Villa? villa = _uniOfWork.VillaRepo.Get(villaId);
		if (villa == null)
			return RedirectToAction("Error", "Home");

		return View(villa);
    }

	[HttpPost]
    public IActionResult Delete(Villa villa)
    {
		Villa? villaDb = _uniOfWork.VillaRepo.Get(villa.Id);
        if (villaDb == null)
            return RedirectToAction("Error", "Home");

		_uniOfWork.VillaRepo.Remove(villaDb);
		_uniOfWork.VillaRepo.Save();
        TempData["success"] = "The villa has been deleted successfully";

        return RedirectToAction(nameof(Index));
    }
}
