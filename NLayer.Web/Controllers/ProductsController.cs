using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productservice;
        private readonly ICategoryService _categoryservice;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productservice, ICategoryService categoryservice, IMapper mapper)
        {
            _productservice = productservice;
            _categoryservice = categoryservice;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productservice.GetProductsWithCategory());
        }


        public async Task<IActionResult> Save()
        {
            var categories = await _categoryservice.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productservice.AddAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }


            var categories = await _categoryservice.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }
    }
}
