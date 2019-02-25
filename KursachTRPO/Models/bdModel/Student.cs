using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models.bdModel
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string NumberOfBook { get; set; }

        public List<HistorySkips> HistorySkips { get; set; }
        public List<History> Histories { get; set; }

        public Student()
        {
            HistorySkips = new List<HistorySkips>();
            Histories = new List<History>();
        }

        public int? GroupId { get; set; }
        public Group Group { get; set; }  
    }
}
