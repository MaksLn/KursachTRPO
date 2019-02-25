using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models.bdModel
{
    public class HistorySkips
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartSkips { get; set; }
        public DateTime EndSkips { get; set; }

        public int? StudentId { get; set; }
        public Student Student { get; set; }
    }
}
