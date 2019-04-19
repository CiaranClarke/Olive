using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentPage : ContentPage
	{
		public PaymentPage ()
		{
			InitializeComponent ();
		}

        public void makePaymentClicked(object sender, EventArgs e)
        {
            DisplayAlert("Payment", "Payment has been made!", "Ok");
            Navigation.PopModalAsync();
            Navigation.PopModalAsync();
        }

    }
}