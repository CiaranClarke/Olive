using Firebase.Database;
using Newtonsoft.Json;
using Olive.AppSpecific;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderPage : ContentPage
	{
        string productID;
        private tblProducts prodObject;
        private tblUser userObject;

        public OrderPage (string productKey)
		{
            productID = productKey;
            PopulateInfo();

            InitializeComponent ();
		}

        public async Task<tblProducts> GetItem()
        {
            //var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCpxDUBeaHiEKaNUEyBPgJxjRDAlGtxW1U"));
            //var data = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            var db = new FirebaseClient(
              "https://olive-4a870.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(Settings.authToken)
              });

            var dbData = await db
                    .Child("Products")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblProducts>();

            var product = dbData.Where(a => a.Key == productID).ToList();

            var json = JsonConvert.SerializeObject(product);
            prodObject = JsonConvert.DeserializeObject<tblProducts>(json);

            //product1.Add(prodObject);
            return prodObject;
        }

        public async Task<tblUser> GetUser()
        {
            //var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCpxDUBeaHiEKaNUEyBPgJxjRDAlGtxW1U"));
            //var data = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            var db = new FirebaseClient(
              "https://olive-4a870.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(Settings.authToken)
              });

            var dbData = await db
                    .Child("Products")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblUser>();

            var user = dbData.Where(a => a.Key == Settings.UserKey).ToList();

            var json = JsonConvert.SerializeObject(user);
            userObject = JsonConvert.DeserializeObject<tblUser>(json);
                
            //userInfo.Add(prodObject);

            return userObject;
        }

        public async void PopulateInfo()
        {
            await GetItem();
            await GetUser();


        }

        public async void ConfirmOrder(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "To confirm your order you must now make payment through Paypal", "Ok", "Cancel");
            if (answer)
            {
                var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Test Product", new Decimal(12.50), "GBP"), new Decimal(0));
                if (result.Status == PayPalStatus.Cancelled)
                {
                    Debug.WriteLine("Cancelled");
                }
                else if (result.Status == PayPalStatus.Error)
                {
                    Debug.WriteLine(result.ErrorMessage);
                }
                else if (result.Status == PayPalStatus.Successful)
                {
                    Debug.WriteLine(result.ServerResponse.Response.Id);
                }
            }
        }
    }
}