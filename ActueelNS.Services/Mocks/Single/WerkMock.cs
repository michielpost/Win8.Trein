using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Mocks.Single
{
    public class WerkMock : Werkzaamheden
    {
        public WerkMock()
        {
            this.Advies = "Abc lang bericht";
            this.Periode = DateTime.Now.ToString();
            this.Reden = "Lange reden";
            this.Traject = "Traject";
            this.Vertraging = "veel vertraging";
        }
    }
}
