using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursachTRPO.Models;
using KursachTRPO.Models.bdModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin, user")]
    [ClaimRequirement]
    public class StudentController : ControllerBase
    {
        private DataBaseContext dataBaseContext;

        public StudentController (DataBaseContext context)
        {
            dataBaseContext = context;
        }

        [HttpGet]
        public IEnumerable<StudentsModel> GetStudent()
        {
            return dataBaseContext.Students.Include(g => g.Group).Select(e => ConvertToStudentsModel(e)).ToList();
        }

        [HttpGet("{id}")]
        public IEnumerable<StudentsModel> GetStudent(int id)
        {
            return dataBaseContext.Students.Include(g => g.Group).Where(e=>e.Id==id).Select(e => ConvertToStudentsModel(e)).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentsModel studentsModel)
        {
            if (ModelState.IsValid)
            {
                Student student = dataBaseContext.Students.Where(e => e.NumberOfBook == studentsModel.NumberOfBook).FirstOrDefault();

                if (student == null)
                {
                    dataBaseContext.Students.Add(new Student
                    {
                        Name = studentsModel.Name,
                        Address = studentsModel.Address,
                        Group = dataBaseContext.Group.Where(e => e.Name == studentsModel.GroupName).FirstOrDefault(),
                        LastName = studentsModel.LastName,
                        MidleName = studentsModel.MidleName,
                        NumberOfBook = studentsModel.NumberOfBook
                    });
                    await dataBaseContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] StudentsModel studentsModel)
        {
            if (ModelState.IsValid)
            {
                Student student = dataBaseContext.Students.Where(e => e.Id == id).FirstOrDefault();

                if (student == null)
                {
                    return BadRequest("группа не существует");
                }
                else
                {

                    student.Name = studentsModel.Name;
                    student.Address = studentsModel.Address;
                    student.Group = dataBaseContext.Group.Where(e => e.Name == studentsModel.GroupName).FirstOrDefault();
                    student.LastName = studentsModel.LastName;
                    student.MidleName = studentsModel.MidleName;
                    student.NumberOfBook = studentsModel.NumberOfBook;

                    dataBaseContext.Students.Update(student);

                    await dataBaseContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status201Created);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                dataBaseContext.Remove(await dataBaseContext.FindAsync<Student>(id));
                await dataBaseContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        private StudentsModel ConvertToStudentsModel(Student student) =>
            new StudentsModel
            {
                Address = student.Address,
                GroupName = student.Group == null ? "": student.Group.Name,
                Name = student.Name,
                LastName = student.LastName,
                Id = student.Id,
                MidleName = student.MidleName,
                NumberOfBook = student.NumberOfBook
            };
    }
}