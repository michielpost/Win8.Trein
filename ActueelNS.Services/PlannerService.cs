using System;
using System.Net;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using ActueelNS.Services.Constants;
using System.Net.Http;

namespace ActueelNS.Services
{
    public class PlannerService : IPlannerService
    {
        private ISearchHistoryService _searchHistoryService;

        public PlannerService(ISearchHistoryService sh)
        {
            _searchHistoryService = sh;
        }
        public async Task<List<ReisMogelijkheid>> GetSearchResult(PlannerSearch search)
        {

            string stringDateTime = search.DateTime.ToString("yyyy-MM-dd") + "T" + search.DateTime.ToString("HH:mm:ss");

            string query = string.Format("previousAdvices=5&nextAdvices=5&fromStation={0}&toStation={1}&dateTime={2}", search.VanStation.Code, search.NaarStation.Code, stringDateTime);

            if (search.ViaStation != null)
                query += "&viaStation=" + search.ViaStation.Code;

            if (search.Type != null && search.Type.ToLower() == "vertrek")
                query += "&departure=true";
            else
                query += "&departure=false";


            query += string.Format("&hslAllowed={0}&yearCard={1}", search.IsHogesnelheid, search.IsYearCard);


            Uri address = new Uri(string.Format(NSApi.BaseUrl + "/ns-api-treinplanner?{0}&a={1}", query, DateTime.Now.Ticks), UriKind.Absolute);
            HttpClient webclient = new HttpClient();
            webclient.MaxResponseContentBufferSize = 9000000;
            //webclient.Credentials = new NetworkCredential(NSApi.Login, NSApi.Password);

            string response = await webclient.GetStringAsync(address);

           return await Task.Run(async () =>
                {

                    XElement tijdenXmlElement = XElement.Parse(response);

                    List<ReisMogelijkheid> reismogelijkheidList = new List<ReisMogelijkheid>();

                    foreach (var element in tijdenXmlElement.Descendants("ReisMogelijkheid"))
                    {
                        ReisMogelijkheid mogelijkheid = new ReisMogelijkheid();
                        mogelijkheid.IsSearchVertrek = (search.Type == "vertrek");

                        if (element.Element("AantalOverstappen") != null)
                            mogelijkheid.AantalOverstappen = int.Parse(element.Element("AantalOverstappen").Value);

                        mogelijkheid.GeplandeVertrekTijd = GetDateTime(element, "GeplandeVertrekTijd");
                        mogelijkheid.ActueleVertrekTijd = GetDateTime(element, "ActueleVertrekTijd");

                        mogelijkheid.GeplandeAankomstTijd = GetDateTime(element, "GeplandeAankomstTijd");
                        mogelijkheid.ActueleAankomstTijd = GetDateTime(element, "ActueleAankomstTijd");

                        mogelijkheid.GeplandeReisTijd = GetElementText(element.Element("GeplandeReisTijd"));

                        if(element.Element("Optimaal") != null)
                            mogelijkheid.Optimaal = bool.Parse(element.Element("Optimaal").Value);

                        mogelijkheid.ReisDelen = new List<ReisDeel>();

                        foreach (var reisdeelElement in element.Descendants("ReisDeel"))
                        {
                            ReisDeel deel = new ReisDeel();

                            deel.VervoerType = GetElementText(reisdeelElement.Element("VervoerType"));

                            deel.ReisStops = new List<ReisStop>();
                            foreach (var stopElement in reisdeelElement.Descendants("ReisStop"))
                            {
                                ReisStop stop = new ReisStop();

                                stop.Naam = GetElementText(stopElement.Element("Naam"));
                                stop.Vertrekspoor = GetElementText(stopElement.Element("Spoor"));
                                stop.Tijd = GetDateTime(stopElement, "Tijd");


                                if (stopElement.Element("Spoor") != null
                                    && stopElement.Element("Spoor").Attribute("wijziging") != null)
                                {
                                    stop.IsVertrekspoorWijziging = bool.Parse(stopElement.Element("Spoor").Attribute("wijziging").Value);
                                }


                                deel.ReisStops.Add(stop);
                            }

                            //ReisStop endStation = null;
                            //if (deel.ReisStops.Count > 1)
                            //{
                            //    endStation = deel.ReisStops.Last();

                            //    deel.ReisStops.Remove(endStation);

                            //}

                            mogelijkheid.ReisDelen.Add(deel);

                            //if (endStation != null)
                            //{
                            //    ReisDeel endDeel = new ReisDeel();

                            //    endDeel.VervoerType = GetElementText(reisdeelElement.Element("VervoerType"));

                            //    endDeel.ReisStops = new List<ReisStop>();
                            //    endDeel.ReisStops.Add(endStation);
                            //    endDeel.IsAankomst = true;

                            //    mogelijkheid.ReisDelen.Add(endDeel);
                            //}
                        }

                        //Set vertrek vertraging
                        var first = mogelijkheid.ReisDelen.FirstOrDefault();
                        if (first != null && first.FirstStop != null)
                        {
                            first.FirstStop.VertragingTekst = mogelijkheid.VertrekVertraging;
                        }

                        //Set aankomst vertraging
                        var last = mogelijkheid.ReisDelen.LastOrDefault();
                        if (last != null && last.FirstStop != null)
                        {
                            last.LastStop.VertragingTekst = mogelijkheid.AankomstVertraging;
                        }

                      

                        reismogelijkheidList.Add(mogelijkheid);
                    }

                    await _searchHistoryService.Add(new SearchHistory() { PlannerSearch = search, Reismogelijkheden = reismogelijkheidList });

                    return reismogelijkheidList;
               });
        }

        private static DateTime GetDateTime(XElement element, string name)
        {
            DateTime dtime = DateTime.Now;
            if (element.Element(name) != null)
            {
                string time = element.Element(name).Value;
                int zoneIndex = time.LastIndexOf('+');
                if (zoneIndex > 0)
                {
                    time = time.Substring(0, zoneIndex);
                }

                dtime = DateTime.Parse(time);
            }

            return dtime;
        }


        private string GetElementText(XElement element)
        {
            if (element != null)
                return element.Value;


            return null;
        }
       
    }
}
