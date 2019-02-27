using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models
{
    public class HostorysModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }

        public int IdSkips { get; set; }
        public string TypeSkips { get; set; }
        public string Cause { get; set; }
        public DateTime StartSkips { get; set; }
        public DateTime EndSkips { get; set; }

    }
}
