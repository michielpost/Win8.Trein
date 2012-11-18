using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Mocks.Single
{
    public class ReisMogelijkheidMock : ReisMogelijkheid
    {
        public ReisMogelijkheidMock()
        {
            this.ActueleVertrekTijd = DateTime.Now;
            this.ActueleAankomstTijd = DateTime.Now;

            this.AantalOverstappen = 2;

            this.GeplandeReisTijd = "10:00";

            this.GeplandeVertrekTijd = DateTime.Now;
        }
    }
}
