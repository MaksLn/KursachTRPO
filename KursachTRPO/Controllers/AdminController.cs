using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachTRPO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KursachTRPO.Models.bdModel;

namespace KursachTRPO.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private AutorizationContext _context;
        public AdminController(AutorizationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x,i)=>i==2).FirstOrDefault().Value;
            return View();
        }

        [HttpGet]
        public IActionResult Group()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            List<Group> groups = new List<Group>(_context.Group.Include(s=>s.Students));
            return View(groups);
        }

        [HttpPost]
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
        public IActionResult AddGroup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGroup(GroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                Group group = _context.Group.Where(G => G.Name == groupModel.Name).FirstOrDefault();

                if (group == null)
                {
                    _context.Group.Add(new Group { Name = groupModel.Name, Specialty = groupModel.Specialty, CreateYear = groupModel.CreateYear });
                    await _context.SaveChangesAsync();

                    return View();
                }
                else
                {
                    ModelState.AddModelError("G", "Группа уже существует");
                }
            }
            return View(groupModel);
        }

        [HttpGet]
        public IActionResult PutGroup(int Id)
        {
            Group group = _context.Group.Where(G => G.Id == Id).FirstOrDefault();
            if (group != null)
            {
                return View(new GroupModel {Id=group.Id, Name = group.Name, Specialty = group.Specialty, CreateYear = group.CreateYear });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    return View();
                }
                else
                {
                    ModelState.AddModelError("G", "Группа уже существует");
                }
            }
            return View(groupModel);
        }

        [HttpGet]
        public IActionResult Users()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            List<User> users = new List<User>(_context.Users);
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            TempData["UserName"] = HttpContext.User.Claims.Where((x, i) => i == 2).FirstOrDefault().Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> AddUser(RegisterModel userModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userModel.Login);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { Email = userModel.Email, Login=userModel.Login, Password = userModel.Password };
                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == userModel.Role.ToLower());
                    if (userRole != null)
                        user.Role = userRole;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return View();
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(userModel);
        }
    }
}