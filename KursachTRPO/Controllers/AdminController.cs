using KursachTRPO.Models;
using KursachTRPO.Models.bdModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Controllers
{
    [Authorize(Roles = "admin,user")]
    public class AdminController : Controller
    {
        private AutorizationContext _context;
        public AdminController(AutorizationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            return View();
        }

        [HttpGet]
        public IActionResult Group(string groupSerch)
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            List<Group> groups;

            if (groupSerch != null)
            {
                groups = new List<Group>(_context.Group.Include(s => s.Students).Where(e => e.Name == groupSerch));
            }
            else
            {
                groups = new List<Group>(_context.Group.Include(s => s.Students));
            }

            return View(groups);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Group(int Id)
        {
            Group group = _context.Group.Where(e => e.Id == Id).FirstOrDefault();

            if (group != null)
            {
                _context.Group.Remove(group);
                await _context.SaveChangesAsync();
            }

            List<Group> groups = new List<Group>(_context.Group.Include(s => s.Students));

            return View(groups);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AddGroup()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddGroup(GroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                Group group = _context.Group.Where(G => G.Name == groupModel.Name).FirstOrDefault();

                if (group == null)
                {
                    _context.Group.Add(new Group { Name = groupModel.Name, Specialty = groupModel.Specialty, CreateYear = groupModel.CreateYear });
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Group", "Admin");
                }
                else
                {
                    ModelState.AddModelError("G", "Группа уже существует");
                }
            }
            return View(groupModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult PutGroup(int Id)
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            Group group = _context.Group.Where(G => G.Id == Id).FirstOrDefault();
            if (group != null)
            {
                return View(new GroupModel { Id = group.Id, Name = group.Name, Specialty = group.Specialty, CreateYear = group.CreateYear });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutGroup(GroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                Group group = _context.Group.Where(G => G.Id == groupModel.Id).FirstOrDefault();

                if (group != null)
                {
                    group.Name = groupModel.Name;
                    group.Specialty = groupModel.Specialty;
                    group.CreateYear = groupModel.CreateYear;
                    _context.Group.Update(group);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Group", "Admin");
                }
                else
                {
                    ModelState.AddModelError("G", "Группа уже существует");
                }
            }
            return View(groupModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Users()
        {

            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            List<User> users = new List<User>(_context.Users);
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AddUser()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Users(string Login)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == Login);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            List<User> users = new List<User>(_context.Users);

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(RegisterModel userModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userModel.Login);
                if (user == null)
                {
                    user = new User { Email = userModel.Email, Login = userModel.Login, Password = userModel.Password };
                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == userModel.Role.ToLower());
                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Users", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(userModel);
        }

        [HttpGet]
        public IActionResult Students(string Name = "", string studentSerch = "")
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            List<StudentsModel> studentsModels;

            if (Name != "")
            {
                var ls = _context.Students.Include(e => e.Group).Where(e => e.Group.Name == Name);

                if (ls != null)
                {
                    studentsModels = new List<StudentsModel>();

                    foreach (var i in ls)
                    {
                        studentsModels.Add(new StudentsModel
                        {
                            Name = i.Name,
                            Address = i.Address,
                            GroupName = i.Group.Name,
                            LastName = i.LastName,
                            Id = i.Id,
                            MidleName = i.MidleName,
                            NumberOfBook = i.NumberOfBook
                        });
                    }

                    if (studentSerch == "" || studentSerch == null)
                    {
                        return View(studentsModels.OrderBy(e => e.GroupName).ToList());
                    }
                    else
                    {
                        return View(studentsModels.OrderBy(e => e.GroupName).Where(e => e.NumberOfBook == studentSerch).ToList());
                    }
                }
            }

            studentsModels = new List<StudentsModel>();

            foreach (var i in _context.Students.Include(e => e.Group))
            {
                try
                {
                    studentsModels.Add(new StudentsModel
                    {
                        Name = i.Name,
                        Address = i.Address,
                        GroupName = i.Group.Name,
                        LastName = i.LastName,
                        Id = i.Id,
                        MidleName = i.MidleName,
                        NumberOfBook = i.NumberOfBook
                    });
                }
                catch (Exception exception)
                {
                    studentsModels.Add(new StudentsModel
                    {
                        Name = i.Name,
                        Address = i.Address,
                        LastName = i.LastName,
                        Id = i.Id,
                        MidleName = i.MidleName,
                        NumberOfBook = i.NumberOfBook
                    });
                }
            }

            if (studentSerch == ""|| studentSerch==null)
            {
                return View(studentsModels.OrderBy(e => e.GroupName).ToList());
            }
            else
            {
                return View(studentsModels.OrderBy(e => e.GroupName).Where(e=>e.NumberOfBook==studentSerch).ToList());
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AddStudents()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddStudents(StudentsModel studentsModel)
        {
            if (ModelState.IsValid)
            {
                Student student = _context.Students.Where(e => e.NumberOfBook == studentsModel.NumberOfBook).FirstOrDefault();

                if (student == null)
                {
                    _context.Students.Add(new Student
                    {
                        Name = studentsModel.Name,
                        Address = studentsModel.Address,
                        Group = _context.Group.Where(e => e.Name == studentsModel.GroupName).FirstOrDefault(),
                        LastName = studentsModel.LastName,
                        MidleName = studentsModel.MidleName,
                        NumberOfBook = studentsModel.NumberOfBook
                    });
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Students", "Admin");
                }
                else
                {
                }
            }
            return View(studentsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Students(int Id)
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            Student student = await _context.Students.FirstOrDefaultAsync(u => u.Id == Id);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            List<StudentsModel> studentsModels = new List<StudentsModel>();

            foreach (var i in _context.Students.Include(e => e.Group))
            {
                try
                {
                    studentsModels.Add(new StudentsModel
                    {
                        Name = i.Name,
                        Address = i.Address,
                        GroupName = i.Group.Name,
                        LastName = i.LastName,
                        Id = i.Id,
                        MidleName = i.MidleName,
                        NumberOfBook = i.NumberOfBook
                    });
                }
                catch
                {
                    studentsModels.Add(new StudentsModel
                    {
                        Name = i.Name,
                        Address = i.Address,
                        LastName = i.LastName,
                        Id = i.Id,
                        MidleName = i.MidleName,
                        NumberOfBook = i.NumberOfBook
                    });
                }
            }
            return View(studentsModels.OrderBy(e => e.GroupName).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult PutStudent(int Id)
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            Student student = _context.Students.Include(e => e.Group).Where(G => G.Id == Id).FirstOrDefault();
            if (student != null)
            {
                if (student.Group == null)
                {
                    return View(new StudentsModel
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Address = student.Address,
                        LastName = student.LastName,
                        MidleName = student.MidleName,
                        NumberOfBook = student.NumberOfBook
                    });
                }

                return View(new StudentsModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    Address = student.Address,
                    GroupName = student.Group.Name,
                    LastName = student.LastName,
                    MidleName = student.MidleName,
                    NumberOfBook = student.NumberOfBook
                });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutStudent(StudentsModel studentModel)
        {
            if (ModelState.IsValid)
            {
                Student student = _context.Students.Include(e => e.Group).Where(e => e.Id == studentModel.Id).FirstOrDefault();

                if (student != null)
                {
                    if (studentModel.GroupName != "")
                    {
                        var group = _context.Group.Where(e => e.Name == studentModel.GroupName).FirstOrDefault();

                        if (group != null)
                        {
                            student.Id = student.Id;
                            student.Name = student.Name;
                            student.Address = student.Address;
                            student.Group = group;
                            student.LastName = student.LastName;
                            student.MidleName = student.MidleName;
                            student.NumberOfBook = student.NumberOfBook;

                            _context.Students.Update(student);
                        }

                        student.Id = student.Id;
                        student.Name = student.Name;
                        student.Address = student.Address;
                        student.LastName = student.LastName;
                        student.MidleName = student.MidleName;
                        student.NumberOfBook = student.NumberOfBook;

                        _context.Students.Update(student);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Students", "Admin");
                }
                else
                {
                    ModelState.AddModelError("G", "Группа уже существует");
                }
            }
            return View(studentModel);
        }


        [HttpGet]
        public IActionResult StudentInfo(int? Id)
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            var temp = _context.Students.Include(e => e.Histories).Include(e => e.HistorySkips).Where(e => e.Id == Id).FirstOrDefault();

            HistorysModel historysModel = new HistorysModel
            {
                historySkipsModels = HistorySkips.Convert(temp.HistorySkips),
                historyModels = History.Convert(temp.Histories),
                StudentId = temp.Id
            };

            return View(historysModel);
        }

        [HttpGet]
        public IActionResult AddSkips(int? Id)
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            return View(new HistorySkipsModel { IdStudent = Id ?? 0, StartSkips = DateTime.Now, EndSkips = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSkips(HistorySkipsModel historySkipsModel)
        {
            if (ModelState.IsValid)
            {
                _context.HistorySkips.Add(new HistorySkips
                {
                    Cause = historySkipsModel.Cause,
                    EndSkips = historySkipsModel.EndSkips,
                    StartSkips = historySkipsModel.StartSkips,
                    Type = historySkipsModel.TypeSkips,
                    StudentId = historySkipsModel.IdStudent
                });

                await _context.SaveChangesAsync();

                return Redirect($"/Admin/StudentInfo?Id={historySkipsModel.IdStudent}");
            }

            return View(historySkipsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentInfoDelete(int? Id, int? IdStudent)
        {
            if (Id != null && IdStudent != null)
            {
                try
                {
                    _context.Remove(_context
                        .Students
                        .Include(e => e.HistorySkips)
                        .Where(e => e.Id == IdStudent)
                        .FirstOrDefault()
                        .HistorySkips
                        .Where(e => e.Id == Id).FirstOrDefault());
                }
                catch
                {

                }

                await _context.SaveChangesAsync();
            }

            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            var temp = _context.Students.Include(e => e.Histories).Include(e => e.HistorySkips).Where(e => e.Id == IdStudent).FirstOrDefault();

            HistorysModel historysModel = new HistorysModel
            {
                historySkipsModels = HistorySkips.Convert(temp.HistorySkips),
                historyModels = History.Convert(temp.Histories),
                StudentId = temp.Id
            };

            return View("StudentInfo", historysModel);
        }

        [HttpGet]
        public IActionResult AddHistorys(int? Id)
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            return View(new HistoryModel { IdStudent = Id ?? 0, DateTime = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHistorys(HistoryModel historyModel)
        {
            if (ModelState.IsValid)
            {
                _context.Histories.Add(new History
                {
                    DateTime = historyModel.DateTime,
                    Type = historyModel.Type,
                    StudentId = historyModel.IdStudent
                });

                await _context.SaveChangesAsync();

                return Redirect($"/Admin/StudentInfo?Id={historyModel.IdStudent}");
            }

            return View(historyModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentInfoDeleteHistory(int? Id, int? IdStudent)
        {
            if (Id != null && IdStudent != null)
            {
                try
                {
                    _context.Remove(_context
                        .Students
                        .Include(e => e.Histories)
                        .Where(e => e.Id == IdStudent)
                        .FirstOrDefault()
                        .Histories
                        .Where(e => e.Id == Id).FirstOrDefault());
                }
                catch
                {

                }

                await _context.SaveChangesAsync();
            }

            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;

            var temp = _context.Students.Include(e => e.Histories).Include(e => e.HistorySkips).Where(e => e.Id == IdStudent).FirstOrDefault();

            HistorysModel historysModel = new HistorysModel
            {
                historySkipsModels = HistorySkips.Convert(temp.HistorySkips),
                historyModels = History.Convert(temp.Histories),
                StudentId = temp.Id
            };

            return View("StudentInfo", historysModel);
        }
    }
}