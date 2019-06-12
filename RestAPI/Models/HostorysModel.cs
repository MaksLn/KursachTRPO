using KursachTRPO.Models.bdModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KursachTRPO.Models
{
    public class HistorysModel
    {
        public List<HistorySkipsModel> historySkipsModels { get; set; }
        public List<HistoryModel> historyModels { get; set; }
        public int? StudentId { get; set; }

        public HistorysModel()
        {
            historyModels = new List<HistoryModel>();
            historySkipsModels = new List<HistorySkipsModel>();
        }
    }

    public class HistoryModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Введите тип")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Введите дату")]
        public DateTime DateTime { get; set; }
        public int? IdStudent { get; set; }
    }

    public class HistorySkipsModel
    {
        public int? IdSkips { get; set; }
        [Required(ErrorMessage = "Введите тип")]
        public string TypeSkips { get; set; }
        [Required(ErrorMessage = "Введите причину")]
        public string Cause { get; set; }
        public DateTime StartSkips { get; set; }
        [Required(ErrorMessage = "Введите дату")]
        public DateTime EndSkips { get; set; }
        [Required(ErrorMessage = "Введите дату")]
        public int? IdStudent { get; set; }
    }
}
