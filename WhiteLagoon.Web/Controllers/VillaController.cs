using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers;

public class VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
{
    private readonly IUnitOfWork _uniOfWork = unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

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

        if (villa.Image == null)
            villa.ImageUrl = "https://placehold.co/600x400";
        else
        {
            // TODO: file extension validation, in model
            string imageFolder = @"images\VillaImages";
			string saveFileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
            string saveFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);

            using (var fileStream = new FileStream(Path.Combine(saveFolder, saveFileName), FileMode.Create))
            {
                villa.Image.CopyTo(fileStream);
            }

            //TODO: check if file exists

            villa.ImageUrl = @"\" + imageFolder + @"\" + saveFileName;
        }

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
		if (!ModelState.IsValid || villa.Id < 1)
			return View();

		if (villa.Image != null)
		{
			// TODO: file extension validation, in model
			string imageFolder = @"images\VillaImages";
			string saveFileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
			string saveFolder = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);

            if (!string.IsNullOrEmpty(villa.ImageUrl))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

			using (var fileStream = new FileStream(Path.Combine(saveFolder, saveFileName), FileMode.Create))
			{
				villa.Image.CopyTo(fileStream);
			}

			//TODO: check if file exists

			villa.ImageUrl = @"\" + imageFolder + @"\" + saveFileName;
		}

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

		if (!string.IsNullOrEmpty(villaDb.ImageUrl))
		{
			var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villaDb.ImageUrl.TrimStart('\\'));

			if (System.IO.File.Exists(oldImagePath))
				System.IO.File.Delete(oldImagePath);
		}

		_uniOfWork.VillaRepo.Remove(villaDb);
		_uniOfWork.VillaRepo.Save();
        TempData["success"] = "The villa has been deleted successfully";

        return RedirectToAction(nameof(Index));
    }
}
