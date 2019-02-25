using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models.bdModel
{
    public class History
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Cause { get; set; }

        public int? StudentId { get; set; }
        public Student Student { get; set; }
    }
}
