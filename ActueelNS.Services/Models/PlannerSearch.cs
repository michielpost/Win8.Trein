using System;
using System.Runtime.Serialization;
using System.Text;

namespace ActueelNS.Services.Models
{
    [KnownType(typeof(Station))]
    public class PlannerSearch
    {
        [DataMember]
        public Station VanStation { get; set; }
        [DataMember]
        public Station NaarStation { get; set; }
        [DataMember]
        public Station ViaStation { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public bool IsHogesnelheid { get; set; }
        [DataMember]
        public bool IsYearCard { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public DateTime SearchDateTime { get; set; }

        public string DisplayTijd
        {
            get
            {
                return DateTime.ToString("dd-MM-yyyy") + " " + DateTime.ToString("HH:mm");
            }
        }

        public string DisplayDag
        {
            get
            {
                return DateTime.ToString("dd-MM-yyyy");
            }
        }

        public string DisplayVia
        {
            get
            {
                if (ViaStation != null)
                    return "via " + ViaStation.Name;
                else
                    return string.Empty;
            }
        }

        public string DisplayFull
        {
            get
            {
                if (this.Type == "aankomst")
                {
                    return string.Format(StringLoader.Get("PlannerSearchAankomstTitle"), VanStation.Name, NaarStation.Name, DisplayVia);

                }
                else
                {
                    return string.Format(StringLoader.Get("PlannerSearchVertrekTitle"), VanStation.Name, NaarStation.Name, DisplayVia);

                }
            }
        }

        public string FullName
        {
            get
            {
                var id = string.Format("{0} - {1}", VanStation.Name, NaarStation.Name);
                if (ViaStation != null)
                    id += " via " + ViaStation.Name;

                return id;
            }
        }


        public string GetUniqueId()
        {
            StringBuilder sb = new StringBuilder();

            if (VanStation != null)
                sb.AppendFormat("_{0}_", VanStation.Code);

            if (NaarStation != null)
                sb.AppendFormat("_{0}_", NaarStation.Code);

            if (ViaStation != null)
                sb.AppendFormat("_{0}_", ViaStation.Code);

            return sb.ToString();

        }

        public string GetUniquePriceId()
        {
            StringBuilder sb = new StringBuilder();

            if (VanStation != null)
                sb.AppendFormat("_{0}_", VanStation.Code);

            if (NaarStation != null)
                sb.AppendFormat("_{0}_", NaarStation.Code);

            if (ViaStation != null)
                sb.AppendFormat("_{0}_", ViaStation.Code);

            if (this.DateTime != null)
                sb.AppendFormat("_{0}_", DateTime.Year);

            return sb.ToString();

        }




    }
}
