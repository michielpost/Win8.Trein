using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Mocks.Single
{
    public class ReisDeelMock : ReisDeel
    {
        public ReisDeelMock()
        {
            this.VervoerType = "Trein";

            this.ReisStops = new List<ReisStop>() { 
                new ReisStop() { Naam = "Delft", Vertrekspoor = "1" }, 
                new ReisStop() { Naam = "Groningen"},
                new ReisStop() { Naam = "Blah"},
                new ReisStop() { Naam = "Amsterdam", Vertrekspoor = "22"}
                };
        }
    }
}
