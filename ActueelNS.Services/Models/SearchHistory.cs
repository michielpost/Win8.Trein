using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActueelNS.Services.Models
{
    public class SearchHistory
    {
        public PlannerSearch PlannerSearch { get; set; }
        public List<ReisMogelijkheid> Reismogelijkheden { get; set; }

        public string UniqueId
        {
            get
            {
                return PlannerSearch.GetUniqueId() + PlannerSearch.DateTime.ToString("yyyyddMM_hhmm");
            }
        }
    }
}
