using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace ActueelNS.Services
{
    public static class StringLoader
    {
        static ResourceLoader resourceLoader = null;

        public static string Get(string resourceName)
        {
            if (resourceLoader == null)
            {
                resourceLoader = new ResourceLoader("ActueelNS.Services/Resources");
            }

            return resourceLoader.GetString(resourceName);
        }

    }
}
