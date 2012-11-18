using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Interfaces;
using Windows.Storage;

namespace ActueelNS.Services
{
    public class SettingService : ISettingService
    {

        private const string OVCARD = "ovcard";
        private const string SHOWHSL = "showhsl";
        private const string SHOWHSLMSG = "showhslmessage";

        public bool GetOvCard()
        {
            try
            {
                var storage = ApplicationData.Current.RoamingSettings;

                if (storage.Values.ContainsKey(OVCARD))
                    return (bool)storage.Values[OVCARD];
                else
                    return false;
            }
            catch { }

            return false;
        }

        public void SaveOvCard(bool value)
        {
            try
            {
                var storage = ApplicationData.Current.RoamingSettings;

                if (storage.Values.ContainsKey(OVCARD))
                    storage.Values[OVCARD] = value;
                else
                    storage.Values.Add(OVCARD, value);


            }
            catch { }
        }

        public bool GetShowHsl()
        {
            try
            {
                var storage = ApplicationData.Current.RoamingSettings;

                if (storage.Values.ContainsKey(SHOWHSL))
                    return (bool)storage.Values[SHOWHSL];
                else
                    return false;
            }
            catch { }

            return true;
        }

        public void SaveShowHsl(bool value)
        {
            try
            {
                var storage = ApplicationData.Current.RoamingSettings;

                if (storage.Values.ContainsKey(SHOWHSL))
                    storage.Values[SHOWHSL] = value;
                else
                    storage.Values.Add(SHOWHSL, value);
            }
            catch { }
        }


        public bool GetShowHslMessage()
        {
            try
            {
                var storage = ApplicationData.Current.LocalSettings;

                if (storage.Values.ContainsKey(SHOWHSLMSG))
                    return (bool)storage.Values[SHOWHSLMSG];
                else
                    return true;
            }
            catch { }

            return true;
        }

        public void SaveShowHslMessage(bool value)
        {
            try
            {
                var storage = ApplicationData.Current.LocalSettings;

                if (storage.Values.ContainsKey(SHOWHSLMSG))
                    storage.Values[SHOWHSLMSG] = value;
                else
                    storage.Values.Add(SHOWHSLMSG, value);
            }
            catch { }
        }

    }
}
