using Olive.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
	{
        object locker = new object(); // class level private field
        private TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

        public SignUpPage ()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public async void OnAlreadyRegisterTapped(object sender, EventArgs e)
        {
            var signUpPage = new LoginPage();
            await Navigation.PushAsync(signUpPage, false);
        }

        private async void RegisterClicked(object sender, EventArgs e)
        {
            try
            {
                var sqliteFilename = "olive.db3";
                string dbPath = DependencyService.Get<IDbFilePath>().GetLocalFilePath();
                dbPath = Path.Combine(dbPath, sqliteFilename);

                if (!String.IsNullOrEmpty(txt_Email.Text) & !String.IsNullOrEmpty(txt_Password.Text) & !String.IsNullOrEmpty(txt_FirstName.Text)
                       & !String.IsNullOrEmpty(txt_LastName.Text) & !String.IsNullOrEmpty(txt_Address1.Text) & !String.IsNullOrEmpty(txt_Town.Text)
                       & !String.IsNullOrEmpty(txt_Telephone.Text) & txt_Password.Text == txt_PasswordConfirm.Text)
                {
                    //bool TableExists = DependencyService.Get<ISQLiteFunctions>().TableExists("tblIphoneContacts", dbPath);

                    //if (TableExists == true)
                    //{
                    //    using (var db = DependencyService.Get<IConnectionSQLite>().CreateConnection())
                    //    {
                    //        var list = db.Query<tblUser>("select userNo, userFirstName, userLastName, userEmail from tblUser where userEmail=?", txt_Email.Text);

                    //        if (list.Count != 0)
                    //        {
                    //            await DisplayAlert("Login", "An account using this email address already exists, please try and log in. If you have forgotten your password use the forgotten password feature.", "Ok");

                    //            var LoginPage = new LoginPage();
                    //            await Navigation.PushAsync(LoginPage, false);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    using (var db = DependencyService.Get<IConnectionSQLite>().CreateConnection())
                    //    {
                    //        lock (locker)
                    //        {
                    //            db.CreateTable<tblUser>();
                    //        }

                    //        lock (locker)
                    //        {
                    //            db.CreateTable<tblAddressUser>();
                    //        }
                    //    }
                    //};

                    var main = new MainPage();
                    await Navigation.PushAsync(main, false);
                }
                else
                {
                    await DisplayAlert("Register", "Please ensure all required fields are completed and try again", "Ok");
                }
            }
            catch(Exception)
            {

            }
        }
    }
}