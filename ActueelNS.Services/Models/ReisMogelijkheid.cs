using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;
using System.Runtime.Serialization;
using System.Globalization;

namespace ActueelNS.Services.Models
{
    [DataContract]
    public class ReisMogelijkheid
    {
        public PlannerSearch PlannerSearch { get; set; }

        [DataMember]
        public int AantalOverstappen { get; set; }
        [DataMember]
        public string GeplandeReisTijd { get; set; }

        [DataMember]
        public bool Optimaal { get; set; }

        [DataMember]
        public DateTime GeplandeVertrekTijd { get; set; }
        [DataMember]
        public DateTime ActueleVertrekTijd { get; set; }

        [DataMember]
        public DateTime GeplandeAankomstTijd { get; set; }
        [DataMember]
        public DateTime ActueleAankomstTijd { get; set; }

        [DataMember]
        public List<ReisDeel> ReisDelen { get; set; }


        [DataMember]
        public bool IsSearchVertrek { get; set; }


        public string ContextDisplayTijd
        {
            get
            {
                if (IsSearchVertrek)
                    return VertrekDisplayTijd;
                else
                    return AankomstDisplayTijd;
            }
        }


        public string VertrekDisplayTijd
        {
            get
            {
                return GeplandeVertrekTijd.ToString("HH:mm") + VertrekVertraging;
            }
        }

        public string AankomstDisplayTijd
        {
            get
            {
                return GeplandeAankomstTijd.ToString("HH:mm") + AankomstVertraging;
            }
        }

        public string VertrekVertraging
        {
            get
            {
                return GetVertragingTekst(GeplandeVertrekTijd, ActueleVertrekTijd);
            }
        }

        public string AankomstVertraging
        {
            get
            {
                return GetVertragingTekst(GeplandeAankomstTijd, ActueleAankomstTijd);
            }
        }

        public string VertrekTypeEnSpoor
        {
            get
            {
                var first = this.ReisDelen.First();

                string text = string.Format(StringLoader.Get("ReisDeelSpoorRegel"), first.VervoerType, first.FirstStop.Vertrekspoor);

                return text;

            }
        }

        public string VertrekTreinType
        {
            get
            {
                return this.ReisDelen.First().VervoerType;
            }
        }


        public string AankomstStation
        {
            get
            {
                return this.ReisDelen.Last().LastStop.Naam;
            }
        }

        private static string GetVertragingTekst(DateTime gepland, DateTime actueel)
        {
            string result = string.Empty;

            if (gepland != null && actueel != null)
            {
                var verschil = actueel - gepland;

                if (verschil > new TimeSpan(0))
                {
                   if (verschil.Hours > 0 || verschil.Minutes > 0)
                    {
                        result = string.Format(" (+{0})", ((verschil.Hours * 60) + verschil.Minutes));
                    }
                }
            }

            return result;
        }

        public string DisplayOverstappen
        {
            get
            {
                if (this.AantalOverstappen == 0)
                    return StringLoader.Get("OverstapGeen");
                else if (this.AantalOverstappen == 1)
                    return StringLoader.Get("Overstap1");
                else
                    return string.Format(StringLoader.Get("OverstapN"), this.AantalOverstappen);
            }
        }

        public string DisplayReisduur
        {
            get
            {
                try
                {
                    TimeSpan ts = TimeSpan.ParseExact(GeplandeReisTijd, @"h\:mm", CultureInfo.InvariantCulture);

                    if (ts.Hours == 0)
                    {
                        return string.Format(StringLoader.Get("ReisduurMinutes"), ts.Minutes);
                    }
                    else if (ts.Minutes == 0)
                    {
                        if (ts.Hours == 1)
                            return string.Format(StringLoader.Get("ReisduurHour"), ts.Hours);
                        else
                            return string.Format(StringLoader.Get("ReisduurHours"), ts.Hours);


                    }
                    else
                    {
                        if(ts.Hours == 1 && ts.Minutes == 1)
                            return string.Format(StringLoader.Get("ReisduurFull_1"), ts.Hours, ts.Minutes);
                        else if (ts.Hours == 1)
                            return string.Format(StringLoader.Get("ReisduurFull"), ts.Hours, ts.Minutes);
                        else if (ts.Minutes == 1)
                            return string.Format(StringLoader.Get("ReisduurFull_S1"), ts.Hours, ts.Minutes);
                        else
                            return string.Format(StringLoader.Get("ReisduurFull_S"), ts.Hours, ts.Minutes);

                    }

                }
                catch
                {
                }

                return StringLoader.Get("Reisduur") + GeplandeReisTijd;
            }
        }


        public string VanNaar
        {
            get
            {
                return this.ReisDelen.First().FirstStop.Naam + " › " + this.ReisDelen.Last().LastStop.Naam;
            }
        }

        public string VanTot
        {
            get
            {
                return VertrekDisplayTijd + " › " + AankomstDisplayTijd;
            }
        }


        public string GetAsText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(VanNaar);
            sb.Append(Environment.NewLine);

            sb.AppendLine(VanTot);
            sb.Append(Environment.NewLine);
            
            sb.AppendLine(DisplayReisduur);
            sb.Append(Environment.NewLine);
            
            sb.AppendLine(DisplayOverstappen);
            sb.Append(Environment.NewLine);

            sb.Append(Environment.NewLine);

            foreach (var deel in this.ReisDelen)
            {
                sb.AppendLine(deel.FirstStop.DisplayTijd + "\t" + deel.FirstStop.Naam + "\t\t spoor " + deel.FirstStop.Vertrekspoor);
                sb.Append(Environment.NewLine);
                sb.AppendLine(deel.LastStop.DisplayTijd + "\t" + deel.LastStop.Naam + "\t\t spoor " + deel.LastStop.Vertrekspoor);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

            }

            return sb.ToString();
        }

        public string GetAsHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(VanNaar);
            sb.Append("<br />");

            sb.AppendLine(VanTot);
            sb.Append("<br />");

            sb.AppendLine(DisplayReisduur);
            sb.Append("<br />");

            sb.AppendLine(DisplayOverstappen);
            sb.Append("<br />");

            sb.Append("<table>");

            foreach (var deel in this.ReisDelen)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", deel.FirstStop.DisplayTijd);
                sb.AppendFormat("<td>{0}</td>", deel.FirstStop.Naam);
                sb.AppendFormat("<td>{0}</td>", deel.FirstStop.Vertrekspoor);
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", deel.LastStop.DisplayTijd);
                sb.AppendFormat("<td>{0}</td>", deel.LastStop.Naam);
                sb.AppendFormat("<td>{0}</td>", deel.LastStop.Vertrekspoor);
                sb.Append("</tr>");

                sb.Append("<tr><td></td><td></td><td></td></tr>");

            }
            sb.Append("</table>");


            return sb.ToString();
        }
    }
}
