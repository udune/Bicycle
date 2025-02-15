using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bicycle.Data.Repositories;
using Bicycle.Models;
using Bicycle.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bicycle.Controllers;

public class HomeController : Controller
{
    private readonly IReviewRepository _ReviewRepository;
    private readonly IWebHostEnvironment _Environment;

    public HomeController(IReviewRepository reviewRepository, IWebHostEnvironment environment)
    {
        _ReviewRepository = reviewRepository;
        _Environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Review()
    {
        var viewModel = new ReviewViewModel()
        {
            Reviews = _ReviewRepository.GetAllReviews()
        };
        return View(viewModel);
    }

    public IActionResult Create()
    {
        var viewModel = new ReviewViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormFile uploadFile, ReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (uploadFile != null)
            {
                string originalFile = uploadFile.FileName;
                string fileType = Path.GetExtension(originalFile);
                string fileName = Guid.NewGuid().ToString();
                string path = _Environment.WebRootPath + "/files/" + fileName + fileType;
                await using var fileStream = new FileStream(path, FileMode.Create);
                await uploadFile.CopyToAsync(fileStream);
                
                FileModel file = new FileModel()
                {
                    OriginalName = originalFile,
                    OriginalType = fileType,
                    FileName = fileName,
                    FileUrl = "/files/" + fileName + fileType
                };

                model.Review.File = file;
            }
            
            _ReviewRepository.AddReview(model.Review);
            _ReviewRepository.Save();
            ModelState.Clear();
        }
        else
        {
            ModelState.AddModelError("Create", "Invalid data");
        }

        return RedirectToAction("Review");
    }

    public IActionResult Detail(int id)
    {
        var viewModel = new ReviewViewModel()
        {
            Review = _ReviewRepository.GetReview(id)
        };

        return View(viewModel);
    }
    
    public IActionResult Edit(int id)
    {
        var viewModel = new ReviewViewModel()
        {
            Review = _ReviewRepository.GetReview(id)
        };
        
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(IFormFile? uploadFile, ReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existing = _ReviewRepository.GetReview(model.Review.Id);
            
            _ReviewRepository.Detach(existing);
            
            if (uploadFile != null)
            {
                if (existing.File != null)
                {
                    string existingPath = Path.Combine(_Environment.WebRootPath, existing.File.FileUrl.TrimStart('/'));
                    if (System.IO.File.Exists(existingPath))
                        System.IO.File.Delete(existingPath);
                    _ReviewRepository.DeleteFile(existing.File);
                }
                
                string originalFile = uploadFile.FileName;
                string fileType = Path.GetExtension(originalFile);
                string fileName = Guid.NewGuid().ToString();
                string path = _Environment.WebRootPath + "/files/" + fileName + fileType;
                await using var fileStream = new FileStream(path, FileMode.Create);
                await uploadFile.CopyToAsync(fileStream);
                
                FileModel file = new FileModel()
                {
                    OriginalName = originalFile,
                    OriginalType = fileType,
                    FileName = fileName,
                    FileUrl = "/files/" + fileName + fileType
                };

                model.Review.File = file;
            }
            else
            {
                model.Review.File = existing.File;
            }
            
            _ReviewRepository.Edit(model.Review);
            _ReviewRepository.Save();
            
            return RedirectToAction("Review");
        }

        var viewModel = new ReviewViewModel()
        {
            Review = model.Review,
        };
        
        return View(viewModel);
    }
    
    public IActionResult Delete(int id)
    {
        var result = _ReviewRepository.GetReview(id);
        if (result != null)
        {
            if (result.File != null)
            {
                string filePath = Path.Combine(_Environment.WebRootPath, result.File.FileUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            
            _ReviewRepository.Delete(result);
            _ReviewRepository.Save();
        }
        
        return RedirectToAction("Review");
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