using System;
using System.Windows;
using Windows.UI.Xaml;

namespace ActueelNS.Services.Models
{
    public class ReisStop
    {
        public string Naam { get; set; }
        public DateTime? Tijd { get; set; }

        public string DisplayTijd
        {
            get
            {
                if (Tijd.HasValue)
                    return Tijd.Value.ToString("HH:mm");
                else
                    return null;
            }
        }

        public string VertragingTekst { get; set; }

        private string _vertrekSpoor;

        public string Vertrekspoor
        {
            get
            {
                if (_vertrekSpoor == null)
                    return _vertrekSpoor;

                if (_vertrekSpoor.Length <= 2)
                    return _vertrekSpoor;
                else
                    return _vertrekSpoor.Substring(0, 2);
            }
            set { _vertrekSpoor = value; }
        }

        public bool IsVertrekspoorWijziging { get; set; }


    }
}
