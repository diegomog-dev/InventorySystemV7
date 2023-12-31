﻿using InventorySystemV7.DataAccess.Repository.IRepository;
using InventorySystemV7.Models;
using InventorySystemV7.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventorySystemV7.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitWork _unitWork;

        public HomeController(ILogger<HomeController> logger, IUnitWork unitWork)
        {
            _logger = logger;
            _unitWork = unitWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> productList = await _unitWork.Product.GetAll();
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}