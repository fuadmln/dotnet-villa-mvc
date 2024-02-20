using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Repositories;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class VillaNumberController(IUnitOfWork unitOfWork) : Controller
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public IActionResult Index()
    {
        var villaNumbers = _unitOfWork.VillaNumberRepo.GetAll(includeProperties: "Villa");

		return View(villaNumbers);
    }

    public IActionResult Create()
    {
        IEnumerable<SelectListItem> villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
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
    public IActionResult Create(VillaNumberVM villaNumberVM)
    {
		var isVillaNumberExist = _unitOfWork.VillaNumberRepo.GetAll().Any(vn => vn.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

		if (!ModelState.IsValid || isVillaNumberExist)
        {
			if (isVillaNumberExist)
				TempData["error"] = "create failed, villa number already exist";

			villaNumberVM.Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			});

			return View(villaNumberVM);
		}

		_unitOfWork.VillaNumberRepo.Add(villaNumberVM.VillaNumber);
		_unitOfWork.VillaNumberRepo.Save();

        TempData["success"] = "The villa number has been created successfully";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int villaNumberId)
    {
        VillaNumber? villaNumber = _unitOfWork.VillaNumberRepo.Get(villaNumberId);
        if (villaNumber == null)
            return RedirectToAction("Error", "Home");

        VillaNumberVM villaNumberVM = new()
        {
            VillaNumber = villaNumber,
            Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            })
        };

        return View(villaNumberVM);
    }

	[HttpPost]
	public IActionResult Update(VillaNumberVM villaNumberVM)
	{
		if (!ModelState.IsValid)
		{
			villaNumberVM.Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			});

			return View(villaNumberVM);
		}

		_unitOfWork.VillaNumberRepo.Update(villaNumberVM.VillaNumber);
		_unitOfWork.VillaNumberRepo.Save();

        TempData["success"] = "The villa has been updated successfully";

        return RedirectToAction(nameof(Index));
	}

	public IActionResult Delete(int villaNumberId)
	{
		VillaNumber? villaNumber = _unitOfWork.VillaNumberRepo.Get(villaNumberId);
		if (villaNumber == null)
			return RedirectToAction("Error", "Home");

		VillaNumberVM villaNumberVM = new()
		{
			VillaNumber = villaNumber,
			Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			})
		};

		return View(villaNumberVM);
	}

	[HttpPost]
    public IActionResult Delete(VillaNumberVM villaNumberVM)
    {
		VillaNumber? villaNumberDb = _unitOfWork.VillaNumberRepo.Get(villaNumberVM.VillaNumber.Villa_Number);
        if (villaNumberDb == null)
            return RedirectToAction("Error", "Home");

		_unitOfWork.VillaNumberRepo.Remove(villaNumberDb);
		_unitOfWork.VillaNumberRepo.Save();

        TempData["success"] = "The villa has been deleted successfully";

        return RedirectToAction(nameof(Index));
    }
}
