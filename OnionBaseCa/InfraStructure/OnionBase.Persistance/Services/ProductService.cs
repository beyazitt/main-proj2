//using OnionBase.Application.Repositories;
//using OnionBase.Application.Services;
//using OnionBase.Domain.Entities;
//using OnionBase.Persistance.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OnionBase.Persistance.Services
//{
//    public class ProductService : IProductService
//    {
//        private readonly IRepository _productWriteRepository;

//        public ProductService(IRepository productWriteRepository)
//        {
//            _productWriteRepository = productWriteRepository;
//        }
//        public void AddProduct(Product product)
//        {
//            _productWriteRepository.AddAsync(product);
//        }

//        public void DeleteProduct(Product product)
//        {
//            _productWriteRepository.Remove(product);
//        }
//    }
//}
