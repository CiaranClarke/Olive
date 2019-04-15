using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Database;
using Firebase.Database.Query;
using Olive.AppSpecific;
using Newtonsoft.Json;
using Firebase.Auth;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        FirebaseClient firebase; //Firebase Database URL  
        FirebaseAuth auth;
        FirebaseAuthLink token = null;
        public LoginPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            firebase = new FirebaseClient("https://olive-4a870.firebaseio.com/"); //Firebase Database URL    

            InitializeComponent();

            txt_Email.Completed += (s, e) => txt_Password.Focus();
            txt_Password.Completed += (s, e) => SignInClicked(null, null);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //txt_Email.Text = "";
            //txt_Password.Text = "";

            txt_Email.Text = "john@test.com";
            txt_Password.Text = "Music4Life!";
        }

        public async void OnPasswordRecoveryTapped(object sender, EventArgs e)
        {
            var forgotPasswordPage = new ForgotPasswordPage();
            await Navigation.PushAsync(forgotPasswordPage, false);
        }

        private async void RegisterClicked(object sender, EventArgs e)
        {
            var signUpPage = new Views.SignUpPage();
            await Navigation.PushAsync(signUpPage, false);
        }

        public async Task<List<tblUser>> GetAllUsers()
        {

            var list = (await firebase
              .Child("Users")
              .OrderByKey()
              .OnceAsync<tblUser>())
              .Select(item => 
              new tblUser
              {
                  userNo = item.Key,
                  userEmail = item.Object.userEmail,
                  userPassword = item.Object.userPassword
              }).ToList();

            return list;
        }

        public async Task<tblUser> GetUser(string userEmail)
        {
            var allUsers = await GetAllUsers();
            var user = await firebase
              .Child("Users")
              .OnceAsync<tblUser>();

            return allUsers.Where(a => a.userEmail == userEmail).FirstOrDefault();
        }

        private async Task<bool> AuthEmail(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCpxDUBeaHiEKaNUEyBPgJxjRDAlGtxW1U"));
            token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            
            var db = new FirebaseClient(
              "https://olive-4a870.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken)
              });



            var dbData = (await db
                    .Child("Users")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblUser>()).Select(item => new tblUser
                    {
                        userNo = item.Key,
                        userEmail = item.Object.userEmail
                    }).ToList();

            var some = dbData.Where(a => a.userEmail == email).FirstOrDefault();
            Settings.UserKey = some.userNo;
            //Settings.UserLocation = userCity;

            return true;
        }

        public async void SignInClicked(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txt_Email.Text) & !String.IsNullOrEmpty(txt_Password.Text))
                {
                    //var test = GetUser(txt_Email.Text);
                    //if (test.Result.userPassword == txt_Password.Text)
                    //{
                    

                     // this is one option.
                    if (await AuthEmail(txt_Email.Text, txt_Password.Text) == true)
                    {
                        Settings.authToken = token.FirebaseToken;

                        //var test = GetUser(txt_Email.Text);
                        //Settings.UserKey = test.Result.userNo;
                        Settings.email = txt_Email.Text;
                        Settings.password = txt_Password.Text;

                        var main = new MainPage1();
                        await Navigation.PushAsync(main, false);
                    }
                    else
                    {
                        await DisplayAlert("Login", "Email address or password are incorrect, please try again.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Login", "Email address or password are not valid, please try again.", "Ok");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Login", ex.ToString(), "Ok");
            }
        }
    }
}