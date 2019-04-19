using Firebase.Database;
using Firebase.Database.Query;
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
        //private tblProducts prodObject;
        private tblUser userObject;
        private tblProducts product1;

        public OrderPage (string productKey)
		{
            

            InitializeComponent ();

            productID = productKey;
            PopulateInfo();
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

            var products = dbData.Where(a => a.Key == productID).ToList();

            foreach (var e in products)
            {
                e.Object.productNo = e.Key;
                var json = JsonConvert.SerializeObject(e.Object);
                var prodObject = JsonConvert.DeserializeObject<tblProducts>(json);
                product1 = prodObject;
            }
           
            return product1;
        }

        public async Task<tblProducts> DeleteItem()
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

            var products = dbData.Where(a => a.Key == productID).ToList();

            foreach (var e in products)
            {
                e.Object.productNo = e.Key;
                var json = JsonConvert.SerializeObject(e.Object);
                var prodObject = JsonConvert.DeserializeObject<tblProducts>(json);
                product1 = prodObject;
            }

            await db.Child("Products").Child(productID).DeleteAsync();

            //product1.Add(prodObject);
            return product1;
        }

        public void closePage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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
                    .Child("Users")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblUser>();

            var user = dbData.Where(a => a.Key == Settings.UserKey).ToList();

            foreach (var e in user)
            {
                e.Object.userNo = e.Key;
                var json = JsonConvert.SerializeObject(e.Object);
                userObject = JsonConvert.DeserializeObject<tblUser>(json);
            }
                
            //userInfo.Add(prodObject);

            return userObject;
        }

        public async void PopulateInfo()
        {
            await GetItem();
            await GetUser();

            txt_FirstName.Text = userObject.userFirstName;
            txt_LastName.Text = userObject.userLastName;
            txtAddressLine1.Text = userObject.userAddressLine1;
            txtAddressLine2.Text = userObject.userAddressLine2;
            txtCity.Text = userObject.userCity;
            txtCounty.Text = userObject.userCounty;
            txtPostcode.Text = userObject.userPostcode;
            txtItemName.Text = product1.prodDescription;
            txtItemSize.Text = product1.prodSize;
            txtItemPrice.Text = product1.prodPrice.ToString();
        }

        public async void ConfirmOrder(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirmation", "To confirm your order you must now make payment", "Ok", "Cancel");
            if (answer)
            {
                await DeleteItem();
                //await Navigation.PopModalAsync();
                var paymentPage = new PaymentPage();
                await Navigation.PushModalAsync(paymentPage, false);

                
            }
        }
    }
}