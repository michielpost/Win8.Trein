using System;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;

namespace ActueelNS.Services.Models
{
    public class Vertrektijd
    {
        public int Ritnummer { get; set; }

        public DateTime Tijd { get; set; }

        public string DisplayTijd
        {
            get
            {
                return Tijd.ToString("HH:mm");
            }
        }

        public string Vertraging { get; set; }
        public string VertragingTekst { get; set; }

        public string Eindbestemming { get; set; }

        public string TreinSoort { get; set; }

        public string Route { get; set; }
        public string ReisTip { get; set; }

        private string _vertrekSpoor;

        public string Vertrekspoor
        {
            get {
                return _vertrekSpoor;
                
            }
            set { _vertrekSpoor = value; }
        }
        
        public bool IsVertrekspoorWijziging { get; set; }

        public string Opmerkingen { get; set; }

        [XmlIgnore]
        public SolidColorBrush BackgroundColor { get; set; }


        public string RegelTwee
        {
            get 
            {
                //return string.Format("{0} perron {1}", TreinSoort, Vertrekspoor);
                return string.Format(StringLoader.Get("VertrektijdRegelTwee"), TreinSoort, Vertrekspoor);
               
            }
           
        }

        public string RegelTweeBig
        {
            get
            {
                if (Route != null && Route.Length > 0)
                    return string.Format("via {0}", Route);
                else
                    return null;
            }

        }


        public bool IsWarningVisible
        {
            get { return (VertragingTekst != null && VertragingTekst.Length > 0); }
        }


        public bool IsOpmerkingVisible
        {
            get { return (Opmerkingen != null && Opmerkingen.Length > 0); }
        }

        public bool IsReisTipVisible
        {
            get { return (ReisTip != null && ReisTip.Length > 0); }
        }
        

    }
}
