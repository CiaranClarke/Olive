using Olive.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Olive
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            var page = new CentrePage();
            Placeholder.Content = page.Content;
        }

        public void MyAccountClicked(object sender, EventArgs e)
        {
            var page = new MyAccountPage();
            Placeholder.Content = page.Content;
        }

        public void SettingsClicked(object sender, EventArgs e)
        {
            var page = new SettingsPage();
            Placeholder.Content = page.Content;
        }

        public void OliveClicked(object sender, EventArgs e)
        {
            var page = new CentrePage();
            Placeholder.Content = page.Content;
        }
    }
}
