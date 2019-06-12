using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursachTRPO.Models;
using KursachTRPO.Models.bdModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private DataBaseContext dataBaseContext;

        public GroupsController(DataBaseContext context)
        {
            dataBaseContext = context;
        }

        [HttpGet]
        public IEnumerable<GroupModel> GetGroups()
        {
            return dataBaseContext.Group.Include(s=>s.Students).Select(e=>ConvertToModel(e)).ToList(); 
        }

        [HttpGet("{id}")]
        public IEnumerable<GroupModel> GetGroup(int id)
        {
                return dataBaseContext.Group.Include(s => s.Students).Where(e => e.Id == id).Select(e=>ConvertToModel(e)).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] GroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                Group group = dataBaseContext.Group.Where(G => G.Name == groupModel.Name).FirstOrDefault();

                if (group == null)
                {
                    dataBaseContext.Group.Add(new Group { Name = groupModel.Name, Specialty = groupModel.Specialty, CreateYear = groupModel.CreateYear });
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
        public async Task<IActionResult> PutGroup(int id, [FromBody] GroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                Group group = dataBaseContext.Group.Where(G => G.Id == id).FirstOrDefault();

                if (group == null)
                {
                    return BadRequest("группа не существует");
                }
                else
                {
                    group.Name = groupModel.Name;
                    group.Specialty = groupModel.Specialty;
                    group.CreateYear = groupModel.CreateYear;
                    dataBaseContext.Group.Update(group);

                    await dataBaseContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status201Created);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                dataBaseContext.Remove(await dataBaseContext.FindAsync<Group>(id));
                await dataBaseContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        private GroupModel ConvertToModel(Group group) =>
            new GroupModel { CreateYear = group.CreateYear, Name = group.Name, Id = group.Id, Specialty = group.Specialty, Count = group.Students.Count()};
    }
}