using System;
using System.Net;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ActueelNS.Services.Constants;
using System.Net.Http;

namespace ActueelNS.Services
{
    public class StoringenService : IStoringenService
    {
        private List<Storing> _storingen;
        private List<Werkzaamheden> _werkzaamheden;
        private DateTime? _syncDate;

        public async Task<List<Storing>> GetStoringen(string station)
        {
            if (_syncDate.HasValue
                && ((DateTime.Now - _syncDate.Value) < new TimeSpan(1, 0, 0))
                && _storingen != null)
                return _storingen;

            //ignore station
            station = string.Empty;

            string url = NSApi.BaseUrl + "/ns-api-storingen?actual=true&unplanned=true";

            if(!string.IsNullOrEmpty(station))
                url = string.Format(NSApi.BaseUrl + "/ns-api-storingen?actual=true&planned=false&station={0}", station);

            Uri address = new Uri(url, UriKind.Absolute);
            HttpClient webclient = new HttpClient();
            webclient.MaxResponseContentBufferSize = 9000000;

            //webclient.Credentials = new NetworkCredential(NSApi.Login, NSApi.Password);

            string response = await webclient.GetStringAsync(address);

            return await Task.Run(() =>
            {
                //System.Threading.Thread.Sleep(5000);

                XElement storingenXmlElement = XElement.Parse(response);

                List<Storing> storingLijst = ParseStoringen(storingenXmlElement);

                List<Werkzaamheden> werkzaamhedenLijst = ParseWerkzaamheden(storingenXmlElement);

                //await TaskEx.Delay(TimeSpan.FromSeconds(5));  

                _syncDate = DateTime.Now;
                _storingen = storingLijst;
                _werkzaamheden = werkzaamhedenLijst;

                return storingLijst;
            });

        }

        public List<Werkzaamheden> GetWerkzaamheden()
        {
                return _werkzaamheden;
        }

        private List<Storing> ParseStoringen(XElement storingenXmlElement)
        {
            List<Storing> storingLijst = new List<Storing>();

            foreach (var element in storingenXmlElement.Element("Ongepland").Descendants("Storing"))
            {
                Storing storing = new Storing();

                if (element.Element("Datum") != null)
                {
                    string time = element.Element("Datum").Value;
                    int zoneIndex = time.LastIndexOf('+');
                    if (zoneIndex > 0)
                    {
                        time = time.Substring(0, zoneIndex);
                    }

                    storing.Datum = DateTime.Parse(time);
                }

                storing.Id = GetElementText(element.Element("id"));
                storing.Traject = GetElementText(element.Element("Traject"));
                storing.Reden = GetElementText(element.Element("Reden"));
                storing.Bericht = GetElementText(element.Element("Bericht"));

#if DEBUG
                //for (int i = 0; i < 15; i++)
                //{
                //    storingLijst.Add(storing);

                //}
#endif

                storingLijst.Add(storing);
            }


            //for (int i = 0; i < 1; i++)
            //{

            //    Storing a = new Storing() { Traject = "A", Reden = "a" };
            //    Storing b = new Storing() { Traject = "Bbbbb bbbb ", Reden = "aaaa aaa" };
            //    Storing c = new Storing() { Traject = "Bbbbbbbbbbbbbbbbbbbbbbbbbb bbbb bbbbbbbbbbbbbbbbbb", Reden = "a aaaaaaaaaaaaa aaaaa aaaaaa" };

            //    storingLijst.Add(c);

            //    storingLijst.Add(a);
            //    storingLijst.Add(b);



            //}

            return storingLijst;
        }

        private List<Werkzaamheden> ParseWerkzaamheden(XElement storingenXmlElement)
        {
            List<Werkzaamheden> werkzaamhedenLijst = new List<Werkzaamheden>();

            foreach (var element in storingenXmlElement.Element("Gepland").Descendants("Storing"))
            {
                Werkzaamheden werk = new Werkzaamheden();

                werk.Id = GetElementText(element.Element("id"));
                werk.Traject = GetElementText(element.Element("Traject"));
                werk.Periode = GetElementText(element.Element("Periode"));
                werk.Reden = "Oorzaak: " + GetElementText(element.Element("Reden"));
                werk.Advies = GetElementText(element.Element("Advies"));
                werk.Vertraging = "Vertraging: " + GetElementText(element.Element("Vertraging"));

                if (!werkzaamhedenLijst.Where(x => x.Id == werk.Id).Any())
                {
                    werkzaamhedenLijst.Add(werk);
                }
            }
            return werkzaamhedenLijst;
        }


        private string GetElementText(XElement element)
        {
            if (element != null)
                return element.Value;


            return null;
        }
    }
}
