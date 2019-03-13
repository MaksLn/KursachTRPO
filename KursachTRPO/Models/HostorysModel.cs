using KursachTRPO.Models.bdModel;
using System;
using System.Collections.Generic;

namespace KursachTRPO.Models
{
    public class HistorysModel
    {
        public List<HistorySkipsModel> historySkipsModels { get; set; }
        public List<HistoryModel> historyModels { get; set; }

        public HistorysModel()
        {
            historyModels = new List<HistoryModel>();
            historySkipsModels = new List<HistorySkipsModel>();
        }
    }

    public class HistoryModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class HistorySkipsModel
    {
        public int IdSkips { get; set; }
        public string TypeSkips { get; set; }
        public string Cause { get; set; }
        public DateTime StartSkips { get; set; }
        public DateTime EndSkips { get; set; }
    }
}
