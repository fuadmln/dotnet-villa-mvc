using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class AccountController(
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<IdentityRole> roleManager) : Controller
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public IActionResult Login(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        LoginVM loginVM = new()
        {
            RedirectUrl = returnUrl
        };

        return View(loginVM);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (ModelState.IsValid)
        {
            var signIn = await _signInManager.PasswordSignInAsync(
                loginVM.Email,
                loginVM.Password,
                loginVM.RememberMe,
                lockoutOnFailure: false
            );

            if (signIn.Succeeded)
            {
                if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(loginVM.RedirectUrl);
            }

            // fail sign in
            ModelState.AddModelError("", "Invalid login attempt.");
        }

        return View(loginVM);
    }

    public IActionResult Register(string? returnUrl = null)
    {
		returnUrl ??= Url.Content("~/");

		if (!_roleManager.RoleExistsAsync(SD.Role_Administrator).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Administrator)).Wait();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
        }

        RegisterVM registerVm = new()
        {
            RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }),
            RedirectUrl = returnUrl
        };

        return View(registerVm);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber,
                NormalizedEmail = registerVM.Email.ToUpper(),
                EmailConfirmed = true,
                UserName = registerVM.Email,
                CreatedAt = DateTime.Now
            };

            var insertUser = await _userManager.CreateAsync(user, registerVM.Password);

            if (insertUser.Succeeded)
            {
                if (!string.IsNullOrEmpty(registerVM.Role))
                    await _userManager.AddToRoleAsync(user, registerVM.Role);
                else
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);

                // automatically sign in user
                await _signInManager.SignInAsync(user, isPersistent: false);

                if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    return RedirectToAction("Index", "Home");

                return LocalRedirect(registerVM.RedirectUrl);
            }

            // error handling
            foreach (var error in insertUser.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        registerVM.RoleList = _roleManager.Roles.Select(r => new SelectListItem()
        {
            Text = r.Name,
            Value = r.Name
        });

        return View(registerVM);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("index", "Home");
    }

    public IActionResult AccessDenied()
    {
		return View();
	}
}
