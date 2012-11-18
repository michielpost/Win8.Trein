using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Mocks.Single
{
    public class StoringMock : Storing
    {
        public StoringMock()
        {
            this.Bericht = "Abc lang bericht";
            this.Datum = DateTime.Now;
            this.Reden = "Lange reden";
            this.Traject = "Traject";
        }
    }
}
