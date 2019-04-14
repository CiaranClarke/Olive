using MySql.Data.MySqlClient;
using Olive.AppSpecific;
using Olive.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
	{
        object locker = new object(); // class level private field
        private TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        private const string FirebaseURL = "https://olive-4a870.firebaseio.com/"; //Firebase Database URL  
        FirebaseClient firebase = new FirebaseClient("https://olive-4a870.firebaseio.com/"); //Firebase Database URL  
        FirebaseAuth auth;

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

        public async Task CreateUser(string userFirstName, string userLastName, string userEmail, string userPassword, string userTelephoneNo,
            string userAddressLine1, string userAddressLine2, string userCity, string userCounty, string userPostcode, DateTime userDateAdded)
        {
            var user = await firebase
              .Child("Users")
              .PostAsync(new tblUser() { userFirstName = userFirstName, userLastName = userLastName, userEmail = userEmail,
                  userPassword = userPassword, userTelephoneNo = userTelephoneNo, userAddressLine1 = userAddressLine1,
              userAddressLine2 = userAddressLine2, userCity = userCity, userCounty = userCounty, userPostcode = userPostcode, userDateAdded = userDateAdded});
            var token = user.Key;

            Settings.UserKey = token;
            Settings.UserLocation = userCity;
        }

        private async Task<bool> AuthSignUpEmail(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCpxDUBeaHiEKaNUEyBPgJxjRDAlGtxW1U"));
            var data = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            var db = new FirebaseClient(
              "https://olive-4a870.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
              });

            var dbData = await db
                    .Child("Users")
                    .Child(data.User.LocalId)
                    .OnceAsync<object>();

            return true;
        }


        private async void RegisterClicked(object sender, EventArgs e)
        {
            try
            {
                //var sqliteFilename = "olive.db3";
                //string dbPath = DependencyService.Get<IDbFilePath>().GetLocalFilePath();
                //dbPath = Path.Combine(dbPath, sqliteFilename);

                if (!String.IsNullOrEmpty(txt_Email.Text) & !String.IsNullOrEmpty(txt_Password.Text) & !String.IsNullOrEmpty(txt_FirstName.Text)
                       & !String.IsNullOrEmpty(txt_LastName.Text) & !String.IsNullOrEmpty(txt_Address1.Text) & !String.IsNullOrEmpty(txt_Town.Text)
                       & !String.IsNullOrEmpty(txt_Telephone.Text) & txt_Password.Text == txt_PasswordConfirm.Text)
                {
                    ////bool TableExists = DependencyService.Get<ISQLiteFunctions>().TableExists("tblIphoneContacts", dbPath);
                    //bool TableExists = true;
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

                    //        //lock (locker)
                    //        //{
                    //        //    db.CreateTable<tblAddressUser>();
                    //        //}
                    //    }
                    //};

                    //lock (locker)
                    //{
                    //    using (var db = DependencyService.Get<IConnectionSQLite>().CreateConnection())
                    //    {
                    //        //If table exists code - check to see if email already present - if so say that it already exists use password resending
                    //        //If forgot password - on click of button to reset - check if internet connection - chnage on phone db to new temp, send temp to webservice. Email out temp. Mark temp flag on phone db - when login again force reset, remove temp flag and upload to webservice/change isDirtyFlag.

                    //        var newCustomer = new tblUser
                    //        {
                    //            userFirstName = txt_FirstName.Text,
                    //            userLastName = txt_LastName.Text,
                    //            userEmail = txt_Email.Text,
                    //            userPassword = txt_Password.Text
                    //        };

                    //        db.Insert(newCustomer);
                    //        //Settings.userNo = newCustomer.userNo.ToString();
                    //    }
                    //}

                    if (await AuthSignUpEmail(txt_Email.Text, txt_Password.Text) == true)
                    {
                        await CreateUser(txt_FirstName.Text, txt_LastName.Text, txt_Email.Text, txt_Password.Text, txt_Telephone.Text, txt_Address1.Text, txt_Address2.Text, txt_Town.Text, txt_County.Text, txt_PostCode.Text, DateTime.Today);
                        
                        var main = new MainPage1();
                        await Navigation.PushAsync(main, false);
                    }
                    else
                    {
                        await DisplayAlert("Register", "An error occurred, please try again later", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Register", "Please ensure all required fields are completed and try again", "Ok");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}