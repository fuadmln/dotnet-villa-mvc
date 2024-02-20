using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class AmenityController(IUnitOfWork unitOfWork) : Controller
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public IActionResult Index()
	{
		var amenities = _unitOfWork.AmenityRepo.GetAll(includeProperties: "Villa");

		return View(amenities);
	}

	public IActionResult Create()
	{
		IEnumerable<SelectListItem> villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
		{
			Text = v.Name,
			Value = v.Id.ToString()
		});

		AmenityVM amenityVM = new()
		{
			Villas = villas
		};

		return View(amenityVM);
	}

	[HttpPost]
	public IActionResult Create(AmenityVM amenityVM)
	{
		if(!ModelState.IsValid)
		{
			amenityVM.Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			});

			return View(amenityVM);
		}

		_unitOfWork.AmenityRepo.Add(amenityVM.Amenity);
		_unitOfWork.AmenityRepo.Save();

		TempData["success"] = "The amenity has been created successfully";

		return RedirectToAction(nameof(Index));
	}

	public IActionResult Update(int amenityId)
	{
		Amenity? amenity = _unitOfWork.AmenityRepo.Get(amenityId, null);

		if (amenity == null)
			return RedirectToAction("Error", "Home");

		AmenityVM amenityVM = new()
		{
			Amenity = amenity,
			Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			})
		};

		return View(amenityVM);
	}

	[HttpPost]
	public IActionResult Update(AmenityVM amenityVM)
	{
		if (!ModelState.IsValid)
		{
			amenityVM.Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			});

			return View(amenityVM);
		}

		_unitOfWork.AmenityRepo.Update(amenityVM.Amenity);
		_unitOfWork.AmenityRepo.Save();

		TempData["success"] = "The amenity has been updated successfully";

		return RedirectToAction(nameof(Index));
	}
	
	public IActionResult Delete(int amenityId)
	{
		Amenity? amenity = _unitOfWork.AmenityRepo.Get(amenityId, null);

		if (amenity == null)
			return RedirectToAction("Error", "Home");

		AmenityVM amenityVM = new()
		{
			Amenity = amenity,
			Villas = _unitOfWork.VillaRepo.GetAll().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			})
		};

		return View(amenityVM);
	}

	[HttpPost]
	public IActionResult Delete(AmenityVM amenityVM)
	{
		Amenity? amenityInDb = _unitOfWork.AmenityRepo.Get(amenityVM.Amenity.Id, null);

		if (amenityInDb == null)
			return RedirectToAction("Error", "Home");

		_unitOfWork.AmenityRepo.Remove(amenityInDb);
		_unitOfWork.AmenityRepo.Save();

		TempData["success"] = "The villa has been deleted successfully";

		return RedirectToAction(nameof(Index));
	}
}
