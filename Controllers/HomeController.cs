using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bicycle.Data.Repositories;
using Bicycle.Models;
using Bicycle.Services;
using Bicycle.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bicycle.Controllers;

public class HomeController : Controller
{
    private readonly IReviewRepository _ReviewRepository;
    private readonly AmazonS3Service _amazonS3Service;

    public HomeController(IReviewRepository reviewRepository, AmazonS3Service amazonS3Service)
    {
        _ReviewRepository = reviewRepository;
        _amazonS3Service = amazonS3Service;
    }

    public IActionResult Index()
    {
        var viewModel = new ReviewViewModel();
        return View(viewModel);
    }

    public IActionResult Review(int pageNumber = 1, string Search = "")
    {
        IEnumerable<Review> reviews;
        
        if (!string.IsNullOrEmpty(Search) && Search.Length == 5)
            reviews = _ReviewRepository.GetFindReviews(Search);
        else
            reviews = _ReviewRepository.GetAllReviews();
        
        var reviewTable = new ReviewTable(
            reviews.OrderByDescending(review => review.Id).Skip((pageNumber - 1) * 10).Take(10).ToList(), 
            new ReviewTablePage(reviews.Count(), pageNumber, 10));
        
        var viewModel = new ReviewViewModel()
        {
            Reviews = reviews,
            ReviewTable = reviewTable
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
    public async Task<IActionResult> Create(IFormFile? uploadFile, ReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (uploadFile != null)
            {
                string originalName = uploadFile.FileName;
                string originalType = Path.GetExtension(originalName);
                string key = Guid.NewGuid() + originalType;
                
                var fileUrl = await _amazonS3Service.UploadFileAsync(uploadFile, key);
                
                FileModel file = new FileModel()
                {
                    OriginalName = originalName,
                    OriginalType = originalType,
                    FileName = key,
                    FileUrl = fileUrl
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
    
    [Authorize]
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
                    await _amazonS3Service.DeleteFileAsync(existing.File.FileName);
                    _ReviewRepository.DeleteFile(existing.File);
                }
                
                string originalName = uploadFile.FileName;
                string originalType = Path.GetExtension(originalName);
                string key = Guid.NewGuid() + originalType;
                
                var fileUrl = await _amazonS3Service.UploadFileAsync(uploadFile, key);
                
                FileModel file = new FileModel()
                {
                    OriginalName = originalName,
                    OriginalType = originalType,
                    FileName = key,
                    FileUrl = fileUrl
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
    
    public async Task<IActionResult> Delete(int id)
    {
        var result = _ReviewRepository.GetReview(id);
        if (result != null)
        {
            if (result.File != null)
            {
                await _amazonS3Service.DeleteFileAsync(result.File.FileName);
                _ReviewRepository.DeleteFile(result.File);
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