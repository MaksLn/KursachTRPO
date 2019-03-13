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
        public string Type { get; set; }
        public string Cause { get; set; }
        public DateTime StartSkips { get; set; }
        public DateTime EndSkips { get; set; }

        public int? StudentId { get; set; }
        public Student Student { get; set; }

        public static List<HistorySkipsModel> Convert(List<HistorySkips> historySkips)
        {
            List<HistorySkipsModel> historySkipsModels = new List<HistorySkipsModel>();

            foreach(var i in historySkips)
            {
                historySkipsModels.Add(i);
            }

            return historySkipsModels;
        }

        public static implicit operator HistorySkipsModel(HistorySkips historySkips)
        {
            return new HistorySkipsModel
            {
                Cause = historySkips.Cause,
                EndSkips = historySkips.EndSkips,
                IdSkips = historySkips.Id,
                StartSkips = historySkips.StartSkips,
                TypeSkips = historySkips.Type
            };
        }
    }
}
