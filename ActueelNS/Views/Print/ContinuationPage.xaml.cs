using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ActueelNS.Views.Print
{
    /// <summary>
    /// A paged used to flow text from a given text container
    /// Usage: Outputscenarios 1-5 might not fit entirely on a given "printer page"
    /// In that case simply add new continuation pages of the given size until all the content can be displayed
    /// </summary>
    public sealed partial class ContinuationPage : Page
    {
        /// <summary>
        /// Creates a continuation page and links text-flow to a text flow container
        /// </summary>
        /// <param name="textLinkContainer">Text link container which will flow text into this page</param>
        public ContinuationPage(RichTextBlockOverflow textLinkContainer)
        {
            InitializeComponent();
            textLinkContainer.OverflowContentTarget = continuationPageLinkedContainer;
        }

    }
}
