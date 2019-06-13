using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursachTRPO.Models;
using KursachTRPO.Models.bdModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("api/student/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private DataBaseContext dataBaseContext;

        public HistoryController(DataBaseContext context)
        {
            dataBaseContext = context;
        }

        [HttpGet("{id}")]
        public IEnumerable<HistoryModel> GetHistory(int id)
        {
            return dataBaseContext.Histories.Where(e => e.StudentId == id).Select(e => new HistoryModel
            {
                Id = e.Id,
                DateTime = e.DateTime,
                IdStudent = e.StudentId,
                Type = e.Type
            });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostHistory(int id, [FromBody] HistoryModel historyModel)
        {
            if (ModelState.IsValid)
            {
                dataBaseContext.Add(new History { DateTime = historyModel.DateTime, StudentId = id, Type = historyModel.Type });
                await dataBaseContext.SaveChangesAsync();

                return StatusCode(201);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            try
            {
                dataBaseContext.Remove(await dataBaseContext.FindAsync<History>(id));
                await dataBaseContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}