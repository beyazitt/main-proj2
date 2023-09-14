using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using OnionBase.Application.Repositories;
using OnionBase.Domain.Entities.Identity;
using OnionBase.Persistance.Contexts;
using OnionBase.Presentation.Commands;
using OnionBase.Presentation.Helpers;
using OnionBase.Presentation.Interfaces;
using OnionBase.Presentation.Models;
using OnionBase.Presentation.ViewModels;
using System.Linq;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using static System.Net.Mime.MediaTypeNames;

namespace OnionBase.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private ILogger<AccountController> _logger;
        private UserDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISmsHelper _smsHelper;
        public AccountController(ILogger<AccountController> logger,
            UserDbContext dbContext,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IHttpClientFactory httpClientFactory,
            ISmsHelper smsHelper
            )
        //ITokenHandler tokenHandler)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _roleManager = roleManager;
            _httpClientFactory = httpClientFactory;
            _smsHelper = smsHelper;
            //_tokenHandler = tokenHandler;

        }
        [HttpGet]
        public IActionResult Login()
        {
            TempData["SuccessLogin"] = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)//Authentication başarılı
                {
                    //var model2 = new ProfileViewModel
                    //{
                    //    Email = user.Email,
                    //    Name = user.Name,
                    //    PhoneNumber = user.PhoneNumber
                    //};
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["SuccessLogin"] = "Başarıyla giriş yapıldı;";
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorLogin"] = "Kullanıcı adı veya şifre hatalı";
                return View("Login");
            }

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(SMS sms, RegisterViewModel model, SmsViewModel smsVM)
        
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "İsim alanı boş bırakılamaz!");
                return View(model);
            }

            Random random = new Random();
            int code = random.Next(100000, 1000000);

            IdentityResult result = await _userManager.CreateAsync(new AppUser()
            {
                Email = model.Email,
                Name = model.Name,
                UserName = model.UserName,
                SmsCode = code,
                PhoneNumber = model.PhoneNumber,
            }, model.Password);

            if (result.Succeeded)
            {
                await _roleManager.CreateAsync(new AppRole("user"));
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                TempData["UserId"] = user.Id;

                string message = "Your registration is successful. Your verification code is {0}".FormatWith(code);
                string targetPhoneNumber = "9{0}".FormatWith(user.PhoneNumber); // Use user.PhoneNumber instead of model.PhoneNumber
                bool smsSent = await _smsHelper.SendSms(message, targetPhoneNumber);

                //if (smsSent)
                //{
                //    // Handle SMS sent successfully, redirect to a success page or show a success message
                //    return RedirectToAction("SmsVerification");
                //}
                //else
                //{
                //    // Handle SMS failed to send, show an error message
                //    ModelState.AddModelError("", "SMS could not be sent. Please try again.");
                //}
                return View("Login");

            }

            // If user registration failed
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> LogOut(LoginViewModel model)
        {
            var user = _dbContext.UserDatas.FirstOrDefault(x => x.Email == model.Email);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var name = User.Identity.Name;
                var userDetail = await _userManager.FindByNameAsync(name);
                ProfileViewModel model = new ProfileViewModel
                {
                    Name = userDetail.Name,
                    Email = userDetail.Email,
                    PhoneNumber = userDetail.PhoneNumber,
                    ProfileImage = userDetail.ProfileImage
                };
                return View(model);
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProfileUpdate()
        {
            var name = User.Identity.Name;
            var userDetail = await _userManager.FindByNameAsync(name);
            ProfileViewModel model = new ProfileViewModel
            {
                Name = userDetail.Name,
                Email = userDetail.Email,
                PhoneNumber = userDetail.PhoneNumber
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProfileUpdate(ProfileViewModel model, IFormFile ProfileImage)
        {
            //using (var memoryStream = new MemoryStream())
            //{
            //    ProfileImage.CopyTo(memoryStream);
            //    string base64String = Convert.ToBase64String(memoryStream.ToArray());
            //    string fileName = Guid.NewGuid().ToString() + ".txt";
            //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Image", fileName);
            //    System.IO.File.WriteAllText(filePath, base64String);
            //    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //    currentUser.ProfileImage = base64String;
            //    var user = await _userManager.GetUserAsync(User);
            //    if (model.Name != null)
            //    {
            //        user.Name = model.Name;
            //    }
            //    if (model.Email != null)
            //    {
            //        user.Email = model.Email;
            //    }
            //    if (model.PhoneNumber != null)
            //    {
            //        user.PhoneNumber = model.PhoneNumber;
            //    }
            //    if (model.ProfileImage != null)
            //    {
            //        user.ProfileImage = base64String;
            //    }
            //    var result = await _userManager.UpdateAsync(user);
            //}

            //return RedirectToAction("Profile");
            if (ProfileImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ProfileImage.CopyTo(memoryStream);
                    string base64String = Convert.ToBase64String(memoryStream.ToArray());
                    string fileName = Guid.NewGuid().ToString() + ".txt";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Image", fileName);
                    System.IO.File.WriteAllText(filePath, base64String);
                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    currentUser.ProfileImage = base64String;
                }
            }

            var user = await _userManager.GetUserAsync(User);
            if (model.Name != null)
            {
                user.Name = model.Name;
            }
            if (model.Email != null)
            {
                user.Email = model.Email;
            }
            if (model.PhoneNumber != null)
            {
                user.PhoneNumber = model.PhoneNumber;
            }
            if (model.ProfileImage != null)
            {
                user.ProfileImage = model.ProfileImage;
            }
            var result = await _userManager.UpdateAsync(user);

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            var role = createRoleViewModel.roleName;
            AppRole Role = new AppRole(role);
            if (role != null)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    await _roleManager.CreateAsync(Role);
                    return RedirectToAction("CreateRole");
                }
                else
                {
                    return RedirectToAction("CreateRole");
                }
            }
            else
            {
                return RedirectToAction("CreateRole");
            }
        }

        [HttpGet]
        public IActionResult RoleManagment()
        {
            var users = _dbContext.UserDatas.ToList();
            var roles = _roleManager.Roles.ToList();
            var viewModel = new RoleManagmentViewModel
            {
                Users = users,
                Roles = roles
            };
            var viewModel2 = new CombinedModelsForRoleManagement
            {
                Model2 = viewModel
            };
            return View(viewModel2);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userName = viewModel.SelectedUser;
                var role = await _roleManager.FindByNameAsync(viewModel.SelectedRole);
                var user = _dbContext.Users.Where(x => x.Name == userName).FirstOrDefault();
                if (user != null && role != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, role.Name);

                    if (result.Succeeded)
                    {
                        TempData["SuccessAssignedRole"] = "Role assigned successfully.";
                        TempData["MessageAssignedRole"] = $"The role '{role.Name}' has been assigned to the user '{viewModel.SelectedRole}'.";
                    }
                    else
                    {
                        TempData["ErrorRoleAssign"] = "Failed to assign role.";
                    }
                }
                else
                {
                    TempData["ErrorRoleFound"] = "User or role not found.";
                }
            }
            else
            {
                TempData["Error"] = "Invalid model state.";
            }

            return RedirectToAction("Admin", "AllProducts");
        }


        [HttpGet]
        public IActionResult SmsVerification()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SmsVerification(RegisterViewModel model,SmsViewModel smsVM)
        {
            var userr = _dbContext.UserDatas.Where(x => x.Email == model.Email).FirstOrDefault();
            string userId = TempData["UserId"].ToString();
            AppUser user = await _dbContext.Users.FindAsync(userId);
            try
            {
                if (ModelState.IsValid || user.SmsCode == smsVM.code)
                {
                    
                    user.SmsVerify = true;
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                TempData["SmsError"] = "Sms kodunuz yanlış, Lütfen tekrar deneyin.";
                if (ModelState.IsValid || user.SmsCode == smsVM.code)
                {
                    user.SmsVerify = true;
                    return View("Login");
                }
            }
            
            return View("Register");
        }


        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var name = User.Identity.Name;
            var userDetail = await _userManager.FindByNameAsync(name);
            var currentUsersOrders = _dbContext.Orders.Where(x => x.CustomerId == userDetail.Id).ToList();
            return View(currentUsersOrders);
        }
    }
}
