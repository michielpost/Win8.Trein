using ActueelNS.Services.Interfaces;
using System.Xml.Linq;
using System.Linq;
using ActueelNS.Services.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Windows.Storage;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using System;

namespace ActueelNS.Services
{
    public class StationService : IStationService
    {
        private List<Station> _stationList = null;

        public async Task<List<Station>> GetStations(string country)
        {
            if (_stationList != null)
                return _stationList;
            else
            {
                var sf = await Package.Current.InstalledLocation.GetFileAsync(@"Data\stations.xml");
                var file = await sf.OpenAsync(FileAccessMode.Read);
                Stream inStream = file.AsStreamForRead();

                XElement stationXmlElement = XDocument.Load(inStream).Elements().First();

                var list = (from x in stationXmlElement.Descendants("s")
                            select new Station
                            {
                                Name = x.Element("name").Value,
                                Code = x.Element("id").Value,
                                NamesExtra = x.Descendants("Sy").Select(s => s.Value).ToArray(),
                                //Country = x.Element("country").Value,
                                //Alias = bool.Parse(x.Element("alias").Value),
                                Lat = double.Parse(x.Element("lat").Value, CultureInfo.InvariantCulture),
                                Long = double.Parse(x.Element("lon").Value, CultureInfo.InvariantCulture)
                            });

                _stationList = list.ToList();

                return _stationList;

            }

        }

        
        public void Test()
        {
            //var start = DateTime.Now;
            //for (int i = 0; i < 20; i++)
            //{
            //    StreamResourceInfo xml = Application.GetResourceStream(new Uri("/ActueelNS.Services;component/Data/stations_all.xml", System.UriKind.Relative));

            //    XElement stationXmlElement = XElement.Load(xml.Stream);

            //    var list = (from x in stationXmlElement.Descendants("station")
            //                where x.Element("country").Value == "NL"
            //                && x.Element("alias").Value == "false"
            //                select new Station
            //                {
            //                    Name = x.Element("name").Value,
            //                    Code = x.Element("code").Value,
            //                    //Country = x.Element("country").Value,
            //                    //Alias = bool.Parse(x.Element("alias").Value),
            //                    //Lat = x.Element("lat").Value,
            //                    //Long = x.Element("long").Value
            //                });

            //    _stationList = list.ToList();
                
            //}
            //var stop = DateTime.Now;

            //MessageBox.Show((stop - start).ToString());

            //var startA = DateTime.Now;
            //for (int i = 0; i < 20; i++)
            //{
            //    StreamResourceInfo xml = Application.GetResourceStream(new Uri("/ActueelNS.Services;component/Data/stations.xml", System.UriKind.Relative));

            //    XElement stationXmlElement = XElement.Load(xml.Stream);
                             

            //    var list = (from x in stationXmlElement.Descendants("station")
            //                where x.Element("country").Value == "NL"
            //                 && x.Element("alias").Value == "false"
            //                select new Station
            //                {
            //                    Name = x.Element("name").Value,
            //                    Code = x.Element("code").Value,
            //                    //Country = x.Element("country").Value,
            //                    //Alias = bool.Parse(x.Element("alias").Value),
            //                    //Lat = x.Element("lat").Value,
            //                    //Long = x.Element("long").Value
            //                });

            //    _stationList = list.ToList();

            //}
            //var stopA = DateTime.Now;

            //MessageBox.Show((stopA - startA).ToString());
        }

        public async Task<Station> GetStationByName(string name)
        {
          
                var allStations = await GetStations("NL");

                return allStations.Where(x => x.Name == name).FirstOrDefault();
        }

        public async Task<Station> GetStationByCode(string code)
        {
            code = code.ToLower();

            var allStations = await GetStations("NL");

            return allStations.Where(x => x.Code.ToLower() == code).FirstOrDefault();
        }

        
    }
}
