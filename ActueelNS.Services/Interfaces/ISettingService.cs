using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActueelNS.Services.Interfaces
{
    public interface ISettingService
    {
        bool GetOvCard();
        void SaveOvCard(bool value);

        bool GetShowHsl();
        void SaveShowHsl(bool value);

        bool GetShowHslMessage();
        void SaveShowHslMessage(bool value);

    }
}
