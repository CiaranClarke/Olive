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

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyItems : ContentPage
	{
        int pNo = SellItemPage.pNo;

        public MyItems ()
		{
            
			InitializeComponent ();

            myItems();
        }

        public void closePage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        public void myItems()
        {
            using (var db = DependencyService.Get<IConnectionSQLite>().CreateConnection())
            {
                var product = db.Get<tblProducts>(pNo);

                byte[] imageByte = Convert.FromBase64String(product.prodImageString);
                ImageSource imgSrc = ImageSource.FromStream(() => new MemoryStream(imageByte));
                Image im = new Image();
                im.HeightRequest = 250;
                im.WidthRequest = 250;
                im.Source = imgSrc;

                imagesStack.Children.Add(im);
            }
        }
	}
}