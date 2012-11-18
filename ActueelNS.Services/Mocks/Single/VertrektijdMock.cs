using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Mocks.Single
{
    public class VertrektijdMock : Vertrektijd
    {
        public VertrektijdMock()
        {
            this.Eindbestemming = "Groningen";
            this.IsVertrekspoorWijziging = true;
            this.Opmerkingen = "Opmerkingen 123";

            this.ReisTip = "Reistip";

            this.Ritnummer = 123;

            this.Route = "Amsterdam CS, Delft, Den Haag";

            this.Tijd = DateTime.Now;

            this.TreinSoort = "Intercity";

            this.Vertraging = "+5 min";

            this.VertragingTekst = "+5 min";

            this.Vertrekspoor = "22a";
        }


    }
}
