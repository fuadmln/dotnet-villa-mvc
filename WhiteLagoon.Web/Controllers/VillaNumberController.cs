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
    public IActionResult Create(VillaNumberVM villaNumberVM)
    {
		var isVillaNumberExist = _context.VillaNumbers.Any(vn => vn.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

		if (!ModelState.IsValid || isVillaNumberExist)
        {
			if (isVillaNumberExist)
				TempData["error"] = "create failed, villa number already exist";

			villaNumberVM.Villas = _context.Villas.ToList().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			});

			return View(villaNumberVM);
		}

        _context.VillaNumbers.Add(villaNumberVM.VillaNumber);
        _context.SaveChanges();
        TempData["success"] = "The villa number has been created successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Update(int villaNumberId)
    {
        VillaNumber? villaNumber = _context.VillaNumbers.Find(villaNumberId);
        if (villaNumber == null)
            return RedirectToAction("Error", "Home");

        VillaNumberVM villaNumberVM = new()
        {
            VillaNumber = villaNumber,
            Villas = _context.Villas.ToList().Select(v => new SelectListItem
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
			villaNumberVM.Villas = _context.Villas.ToList().Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString()
			});

			return View(villaNumberVM);
		}

		_context.VillaNumbers.Update(villaNumberVM.VillaNumber);
		_context.SaveChanges();
        TempData["success"] = "The villa has been updated successfully";

        return RedirectToAction("Index");
	}

	public IActionResult Delete(int villaNumberId)
	{
		VillaNumber? villaNumber = _context.VillaNumbers.Find(villaNumberId);
		if (villaNumber == null)
			return RedirectToAction("Error", "Home");

		VillaNumberVM villaNumberVM = new()
		{
			VillaNumber = villaNumber,
			Villas = _context.Villas.ToList().Select(v => new SelectListItem
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
		VillaNumber? villaNumberDb = _context.VillaNumbers.Find(villaNumberVM.VillaNumber.Villa_Number);
        if (villaNumberDb == null)
            return RedirectToAction("Error", "Home");

        _context.VillaNumbers.Remove(villaNumberDb);
        _context.SaveChanges();
        TempData["success"] = "The villa has been deleted successfully";

        return RedirectToAction("Index");
    }
}
