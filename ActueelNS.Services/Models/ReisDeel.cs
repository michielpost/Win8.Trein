using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ActueelNS.Services.Models
{
    public class ReisDeel
    {
        public string VervoerType { get; set; }
        public bool IsAankomst { get; set; }

        public List<ReisStop> ReisStops { get; set; }

        public List<ReisStop> TussenStops
        {
            get
            {
                if (ReisStops != null && ReisStops.Count > 1)
                    return ReisStops.Skip(1).ToList();
                else
                    return null;
            }
        }

        public string Richting
        {
            get
            {
                if (ReisStops != null)
                {
                    if (ReisStops.Count <= 1)
                        return null;

                    string text = ReisStops.Skip(1).FirstOrDefault().Naam;

                    return text;

                }
                else
                    return null;
            }

        }

        public string RegelEen
        {
            get
            {
                if (ReisStops != null)
                {
                    string text = string.Format(StringLoader.Get("ReisDeelSpoorRegel"), FirstStop.Naam, FirstStop.Vertrekspoor);

                    return text;

                }
                else
                    return VervoerType;
            }

        }

        public string RegelAankomst
        {
            get
            {
                if (ReisStops != null)
                {
                    string text = string.Format(StringLoader.Get("ReisDeelSpoorRegel"), LastStop.Naam, LastStop.Vertrekspoor);

                    return text;

                }
                else
                    return null;
            }

        }


        public string RegelTwee
        {
            get
            {
                if (ReisStops != null)
                {
                    if (ReisStops.Count <= 1)
                        return VervoerType;

                    //string text = string.Format("{0} richting", VervoerType);
                    string text = string.Format(StringLoader.Get("ReisDeelRegelTwee"), VervoerType);

                    return text;

                }
                else
                    return VervoerType;
            }

        }

        public string RegelDrie
        {
            get
            {
                if (ReisStops != null && ReisStops.Count > 1)
                {
                    string text = StringLoader.Get("tussenstation1");
                    if (ReisStops.Count > 2)
                        text = string.Format(StringLoader.Get("tussenstationN"), (ReisStops.Count - 1));

                    return text;

                }
                else
                    return string.Empty;
            }

        }

        public ReisStop FirstStop
        {
            get
            {
                if (ReisStops != null && ReisStops.Count >= 0)
                    return ReisStops.First();
                else
                    return null;
            }
           
        }

        public ReisStop LastStop
        {
            get
            {
                if (ReisStops != null && ReisStops.Count >= 0)
                    return ReisStops.Last();
                else
                    return null;
            }

        }


        public bool HasSingleMessage
        {
            get
            {
                return IsAankomst;
                //if (ReisStops != null && ReisStops.Count > 1)
                //    return false;
                //else
                //    return true;
            }
           
        }


        

    }
}
