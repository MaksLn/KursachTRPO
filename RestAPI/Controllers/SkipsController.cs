using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursachTRPO.Models;
using KursachTRPO.Models.bdModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin,user")]
    [ClaimRequirement]
    public class SkipsController : ControllerBase
    {
        private DataBaseContext dataBaseContext;

        public SkipsController(DataBaseContext context)
        {
            dataBaseContext = context;
        }

        [HttpGet("{id}")]
        public IEnumerable<HistorySkipsModel> GetSkips(int id)
        {
            return dataBaseContext.HistorySkips.Where(e => e.StudentId == id).Select(e => new HistorySkipsModel
            {
                Cause = e.Cause,
                EndSkips = e.EndSkips,
                IdStudent = id,
                IdSkips = e.Id,
                StartSkips = e.StartSkips,
                TypeSkips = e.Type
            });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostSkips(int id, [FromBody] HistorySkipsModel historySkipsModel)
        {
            if (ModelState.IsValid)
            {
                dataBaseContext.Add(new HistorySkips
                {
                    Cause = historySkipsModel.Cause,
                    EndSkips = historySkipsModel.EndSkips,
                    StartSkips = historySkipsModel.StartSkips,
                    StudentId = id,
                    Type = historySkipsModel.TypeSkips
                });
                await dataBaseContext.SaveChangesAsync();

                return StatusCode(201);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkips(int id)
        {
            try
            {
                dataBaseContext.Remove(await dataBaseContext.FindAsync<HistorySkips>(id));
                dataBaseContext.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}