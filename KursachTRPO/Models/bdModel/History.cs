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
        public DateTime DateTime { get; set; }

        public int? StudentId { get; set; }
        public Student Student { get; set; }

        public static List<HistoryModel> Convert(List<History> histories)
        {
            List<HistoryModel> historyModels = new List<HistoryModel>();

            foreach (var i in histories)
            {
                historyModels.Add(i);
            }

            return historyModels;
        }

        public static implicit operator HistoryModel(History history)
        {
            return new HistoryModel
            {
                DateTime = history.DateTime,
                Id = history.Id,
                Type = history.Type
            };
        }
    }
}
