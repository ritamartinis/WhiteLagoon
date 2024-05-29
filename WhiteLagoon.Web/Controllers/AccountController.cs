using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utilities;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //injetar estas classes já existentes no framework core
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        //CONSTRUTOR
        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        //1 - LOGIN
        //GET
        public IActionResult Login(string returnUrl = null!)        //se vier nulo, vai para a homepage
        {
            //se o return do url não for nulo nem vazio, vai devolver o valor do que está aqui.
            returnUrl ??= Url.Content("~/");

            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(loginVM);
        }

        //1 - REGISTER
        //GET
        public IActionResult Register()
        {
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Costumer)).Wait();
            }

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(_ => new SelectListItem
                {
                    Text = _.Name,
                    Value = _.Id
                })
            };
            return View(registerVM);
        }

        //POST
        //aqui vou criar um novo utilizador
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
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

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(registerVM.Role))
                {
                    await _userManager.AddToRoleAsync(user, registerVM.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Costumer);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            //Se tiver erros, mostra-os
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            registerVM.RoleList = _roleManager.Roles.Select(_ => new SelectListItem
            {
                Value = _.Name,
                Text = _.Name
            }).ToList();


            return View(registerVM);
        }
    }
}
