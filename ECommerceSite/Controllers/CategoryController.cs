﻿using ECommereceSiteData.Data;
using ECommereceSiteData.Repository;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ECommerceSite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController( IWebHostEnvironment webHostEnvironment,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Category> data = _unitOfWork.Category.GetAll().ToList();
            var checkData = data;
            return View(checkData);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(Category modeldata, IFormFile img)
        {

            if (img != null && img.Length > 0)
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", img.FileName);
                using (var filestream = System.IO.File.Create(filepath))
                {
                    img.CopyTo(filestream);
                }
            }


            if (ModelState.IsValid)
            {
                modeldata.ImageUrl = "/Photos/" + img.FileName;
                _unitOfWork.Category.Add(modeldata);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Category");
            }
            return View(modeldata);


        }

        [HttpGet]

        public IActionResult Edit(int Id)
        {
            var data = _unitOfWork.Category.Get( u => u.Id == Id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return View(data);
            }
        }

        [HttpPost]

        public IActionResult Edit(Category modeldata, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if (img != null && img.Length > 0)
                {
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", img.FileName);
                    using (var filestream = System.IO.File.Create(filepath))
                    {
                        img.CopyTo(filestream);
                    }
                    modeldata.ImageUrl = "/Photos/" + img.FileName;
                }
                else
                {
                    // Retain the old image URL if no new image is uploaded
                    var existingCategory = _db.Categories.AsNoTracking().FirstOrDefault(c => c.Id == modeldata.Id);
                    if (existingCategory != null)
                    {
                        modeldata.ImageUrl = existingCategory.ImageUrl;
                    }
                }
                _unitOfWork.Category.Update(modeldata);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View(modeldata);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _unitOfWork.Category.Get(u => u.Id == id);

            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]

        public IActionResult Delete(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(obj);
            
        }

    }

}