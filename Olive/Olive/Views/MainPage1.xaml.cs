using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage1 : Xamarin.Forms.TabbedPage
    {
        public MainPage1()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
            var accountPage = new NavigationPage(new MyAccountPage());
            accountPage.Icon = "accountIcon.png";

            var navigationPage = new NavigationPage(new CentrePage());
            navigationPage.Icon = "logoIcon.png";

            var settingsPage = new NavigationPage(new SettingsPage());
            settingsPage.Icon = "settingsIcon.png";

            
            Children.Add(accountPage);
            Children.Add(navigationPage);
            Children.Add(settingsPage);

            InitializeComponent();
        }
    }
}