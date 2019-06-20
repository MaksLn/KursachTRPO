using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models
{
    public class StudentsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Введите имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите фамилию")]
        public string MidleName { get; set; }
        [Required(ErrorMessage = "Введите отчество")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Введите адрес")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Введите номер зачетной книжки")]
        public string NumberOfBook { get; set; }
        public string GroupName { get; set; }
    }
}
