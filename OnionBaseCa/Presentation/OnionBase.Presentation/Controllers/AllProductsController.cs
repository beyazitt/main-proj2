using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnionBase.Application.Repositories;
using OnionBase.Application.Services;
using OnionBase.Domain.Entities;
using OnionBase.Domain.Entities.Identity;
using OnionBase.Persistance.Contexts;
using OnionBase.Persistance.Repositories;
using OnionBase.Presentation.DTOs;
using OnionBase.Presentation.ViewModels;

namespace OnionBase.Presentation.Controllers
{
    public class AllProductsController : Controller
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private ILogger<AllProductsController> _logger;
        private UserDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductReadRepository _productReadRepository;
        private readonly SignInManager<AppUser> _signInManager;

        public AllProductsController(ILogger<AllProductsController> logger, UserDbContext dbContext, IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository,SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var result = _productReadRepository.GetAll().ToList();
            return View(result);
        }
        public IActionResult ProductDetail(int ProductCode)
        {
            var result = _productReadRepository.GetAll().Where(x => x.ProductCode == ProductCode).ToList();
            return View(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            var user = _dbContext.UserDatas.Where(x => x.IsAdmin == true).ToList();
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Profile", "Account");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel addProductViewModel, IFormFile Image)
        {
            Random random = new Random();
            int code = random.Next(10000, 999999);
            
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Image.CopyTo(memoryStream);
                    string base64String = Convert.ToBase64String(memoryStream.ToArray());
                    string fileName = Guid.NewGuid().ToString() + ".txt";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Image", fileName);
                    System.IO.File.WriteAllText(filePath, base64String);
                    var product = new Product

                    {
                        ProductName = addProductViewModel.ProductName,
                        ProductDescription = addProductViewModel.ProductDescription,
                        Price = addProductViewModel.Price,
                        ProductColor = addProductViewModel.ProductColor,
                        Stock = addProductViewModel.Stock,
                        ProductCode = code,
                        Image = base64String
                    };
                    await _productWriteRepository.AddAsync(product);
                    await _productWriteRepository.SaveAsync();
                }
                // ViewModel'i Product modeline dönüştürme
                
                
                

                // Başarılı bir şekilde eklenirse yönlendirme yapabilirsiniz
                TempData["Success"] = "Başarılı";
                TempData["Message"] = "Ürün başarıyla eklendi";
                return RedirectToAction("AddProduct", "AllProducts");
            }

            // Eğer ModelState geçerli değilse, hata mesajlarıyla birlikte view'i tekrar gösterin
            TempData["Error"] = "Ürün eklenemedi";
            return View(addProductViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string ProductName)
        {
            var product = await _productReadRepository.GetSingleAsync(x => x.ProductName == ProductName);

            if (product != null)
            {
                _productWriteRepository.Remove(product);
                await _productWriteRepository.SaveAsync();

                // Silme işlemi başarılıysa yönlendirme yapabilirsiniz
                TempData["Success"] = "Başarılı";
                TempData["Message"] = "Ürün başarıyla silindi";
            }
            else
            {
                // Ürün bulunamazsa, hata mesajıyla birlikte yönlendirme yapabilirsiniz
                TempData["Error"] = "Ürün bulunamadı";
            }

            return RedirectToAction("Index", "AllProducts");
        }


        [HttpGet]
        public IActionResult OrderOrQuestion(int ProductCode)
        {
            ProductCodeDTO dto = new ProductCodeDTO
            {
                ProductCode = ProductCode
            };
            CombinedModelsForOrder model = new CombinedModelsForOrder()
            {
                Model2 = dto
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OrderOrQuestion(CombinedModelsForOrder OrderVM)
        {
            var name = User.Identity.Name;
            var userDetail = await _userManager.FindByNameAsync(name);
            var campaign = _dbContext.Campaigns.FirstOrDefault(c => c.discountCode == OrderVM.Model1.CampaignCode);
            if(OrderVM.Model1.CampaignCode != null)
            {
                Order order = new Order()
                {
                    CustomerId = userDetail.Id,
                    Address = OrderVM.Model1.Address,
                    Users = userDetail,
                    Phone = (int)OrderVM.Model1.Phone,
                    Products = _dbContext.Products.Where(x => x.ProductCode == OrderVM.Model1.ProductCode).ToList(),
                    ProductCode = OrderVM.Model1.ProductCode,
                    usedCampaignCode = OrderVM.Model1.CampaignCode

                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                TempData["Success"] = "Siparişiniz Alındı. Siparişinizin onaylanması için lütfen ödeme sayfasına geçiniz.";
                //TempData["OrderId"] = order.OrderId;
                return RedirectToAction("Payment", "AllProducts", new { OrderId = order.OrderId });
            }
            else
            {
                Order order = new Order()
                {
                    CustomerId = userDetail.Id,
                    Address = OrderVM.Model1.Address,
                    Users = userDetail,
                    Phone = (int)OrderVM.Model1.Phone,
                    Products = _dbContext.Products.Where(x => x.ProductCode == OrderVM.Model1.ProductCode).ToList(),
                    ProductCode = OrderVM.Model1.ProductCode
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                TempData["Success"] = "Siparişiniz Alındı. Siparişinizin onaylanması için lütfen ödeme sayfasına geçiniz.";
                ViewBag.OrderId = order.OrderId;
                return RedirectToAction("Payment", "AllProducts", new { OrderId = order.OrderId });
            }
            
            
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateQuestionAsync(OrderViewModelcs OrderVM)
        //{
        //    var name = User.Identity.Name;
        //    var userDetail = await _userManager.FindByNameAsync(name);
        //    Question question = new Question()
        //    {
        //        QuestionBody = OrderVM.QuestionBody,
        //        Users = userDetail
        //    };
        //    _dbContext.Questions.Add(question);

        //    return RedirectToAction("Index");
        //}



        [HttpGet]
        public IActionResult Payment(Guid OrderId, Product product)
        {
            //ViewBag.OrderId = OrderId;
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == OrderId);

            var haveInterest = _dbContext.Campaigns.Where(x => x.discountCode == order.usedCampaignCode).FirstOrDefault();
            var soldProduct = _dbContext.Products.Where(m => order.ProductCode == m.ProductCode).FirstOrDefault();
            if (haveInterest != null)
            {   
                var price = soldProduct.Price - (soldProduct.Price * haveInterest.discountRate);
                ViewBag.price = price;
                return View(OrderId);
            }
            else
            {
                ViewBag.price = soldProduct.Price;
                return View(OrderId);
            }
        }

        [HttpPost]
        public async  Task<IActionResult> PaymentApprove(Guid OrderId)
        {
            //Guid orderId = Guid.Parse(TempData["OrderId"].ToString());
            var currentOrder = _dbContext.Orders.Where(x => x.OrderId == OrderId).FirstOrDefault();
            var urun = _dbContext.Products.Where(x => x.ProductCode == currentOrder.ProductCode).FirstOrDefault();
            urun.Stock -= 1;
            _productWriteRepository.Update(urun);
            await _productWriteRepository.SaveAsync();
            currentOrder.confirmationRequest = true;
            _dbContext.SaveChanges();
            TempData["Mesaj"] = "Onay isteğiniz gönderildi";
            return RedirectToAction("Profile", "Account");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult PaymentConfirmation()     
        {
            var list = _dbContext.Orders.Where(x => x.confirmationRequest == true).ToList();
            var model = new PaymentConfirmationDTO
            {
                orders = list
            };
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PaymentConfirmation(PaymentConfirmationDTO pcDTO)
        {
            var currentOrder = await _dbContext.Orders.Where(o => o.OrderId == pcDTO.OrderId).FirstOrDefaultAsync();
            currentOrder.isConfirmed = true;
            currentOrder.confirmationRequest = false;
            _dbContext.SaveChanges();
            TempData["Message"] = "Onaylandı.";
            return RedirectToAction("PaymentConfirmation","AllProducts");
        }

        [HttpGet]
        public IActionResult AddCampaign()
        {
            return View();
        }
    }
}
