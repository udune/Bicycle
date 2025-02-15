using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bicycle.Data.Repositories;
using Bicycle.Models;
using Bicycle.ViewModels;

namespace Bicycle.Controllers;

public class HomeController : Controller
{
    private readonly ITeacherRepository _TeacherRepository;
    private readonly IStudentRepository _StudentRepository;
    private readonly IReviewRepository _ReviewRepository;

    public HomeController(ITeacherRepository teacherRepository, IStudentRepository studentRepository, IReviewRepository reviewRepository)
    {
        _TeacherRepository = teacherRepository;
        _StudentRepository = studentRepository;
        _ReviewRepository = reviewRepository;
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
    public IActionResult Create(ReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
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
    public IActionResult Edit(ReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
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
            _ReviewRepository.Delete(result);
            _ReviewRepository.Save();
        }
        
        return RedirectToAction("Review");
    }

    public IActionResult Student()
    {
        var viewModel = new StudentTeacherViewModel()
        {
            Teachers = _TeacherRepository.GetAllTeachers(),
            Students = _StudentRepository.GetAllStudents()
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Student(StudentTeacherViewModel model)
    {
        if (ModelState.IsValid)
        {
            // model 데이터를 Student 테이블에 저장
            _StudentRepository.AddStudent(model.Student);
            _StudentRepository.Save();
            ModelState.Clear();
        }
        else
        {
            // 에러를 보여준다
        }

        var viewModel = new StudentTeacherViewModel()
        {
            Teachers = _TeacherRepository.GetAllTeachers(),
            Students = _StudentRepository.GetAllStudents()
        };
        
        return View(viewModel);
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