using Olive.ViewModels;
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
	public partial class ViewItemPage : ContentPage
	{
        public static int productID;
        public static ViewItemPage ViewItemPageCS;

        public ViewItemPage (int productId)
        {
            ViewItemPageCS = this;
            InitializeComponent();

            productID = productId;

            CreateItem();
        }

        public void CreateItem()
        {

        }
    }
}