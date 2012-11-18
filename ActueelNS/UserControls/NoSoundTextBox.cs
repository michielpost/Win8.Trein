using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ActueelNS.UserControls
{
    public class NoSoundTextBox : TextBox
    {
        public event EventHandler EnterPressed;

        protected override void OnKeyDown(Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //do nothing, just override the original one to avoid the waring sound.
            if (e.Key != Windows.System.VirtualKey.Enter)
                base.OnKeyDown(e);
            else
            {
                if (EnterPressed != null)
                {
                    EnterPressed(this, null);
                }
            }
        }


        protected override void OnKeyUp(Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //do nothing, just override the original one to avoid the waring sound.
            if (e.Key != Windows.System.VirtualKey.Enter)
                base.OnKeyUp(e);
        }
    }
}
