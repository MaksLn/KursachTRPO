using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models.bdModel
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public DateTime CreateYear { get; set; }

        public List<Student> Students { get; set; }
        public Group()
        {
            Students = new List<Student>();
        }
    }
}
