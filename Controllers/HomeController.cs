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

    public HomeController(ITeacherRepository teacherRepository, IStudentRepository studentRepository)
    {
        _TeacherRepository = teacherRepository;
        _StudentRepository = studentRepository;
    }

    public IActionResult Index()
    {
        return View();
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

    public IActionResult Detail(int id)
    {
        var viewModel = new StudentTeacherViewModel()
        {
            Student = _StudentRepository.GetStudent(id)
        };

        return View(viewModel);
    }

    public IActionResult Edit(int id)
    {
        var viewModel = new StudentTeacherViewModel()
        {
            Student = _StudentRepository.GetStudent(id)
        };
        
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(StudentTeacherViewModel model)
    {
        if (ModelState.IsValid)
        {
            _StudentRepository.Edit(model.Student);
            _StudentRepository.Save();
            
            return RedirectToAction("Student");
        }

        var viewModel = new StudentTeacherViewModel()
        {
            Student = model.Student,
        };
        
        return View(viewModel);
    }
    
    public IActionResult Delete(int id)
    {
        var result = _StudentRepository.GetStudent(id);
        if (result != null)
        {
            _StudentRepository.Delete(result);
            _StudentRepository.Save();
        }
        
        return RedirectToAction("Student");
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