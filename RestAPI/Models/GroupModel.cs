using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите название группы")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите специальность группы")]
        public string Specialty { get; set; }
        [Required(ErrorMessage = "Укажите дату создания группы")]
        public DateTime CreateYear { get; set; }
    }
}
