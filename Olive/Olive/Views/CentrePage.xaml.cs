using Firebase.Database;
using Firebase.Database.Query;
using MLToolkit.Forms.SwipeCardView;
using MLToolkit.Forms.SwipeCardView.Core;
using Newtonsoft.Json;
using Olive.AppSpecific;
using Olive.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Olive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CentrePage : ContentPage
	{
        FirebaseClient firebase = new FirebaseClient("https://olive-4a870.firebaseio.com/"); //Firebase Database URL  
        string name;

        public CentrePage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent ();
            this.BindingContext = new CardViewModel();
            
            SwipeCardView.Dragging += OnDragging;

            
        }

        public async void OnSwipedRightCommand()
        {
            System.Reflection.PropertyInfo pi = SwipeCardView.TopItem.GetType().GetProperty("ProfileId");
            string name = (string)(pi.GetValue(SwipeCardView.TopItem, null));

            await CreateWishlist(Settings.UserKey, name);

        }
        private void OnDislikeClicked(object sender, EventArgs e)
        {
            this.SwipeCardView.InvokeSwipe(SwipeCardDirection.Left);
        }

        private async void OnSuperLikeClicked(object sender, EventArgs e)
        {
            await this.SwipeCardView.InvokeSwipe(SwipeCardDirection.Up);
            
            //var wishlistPage = new Views.ViewItemPage(profileID_lbl.Text);
            //await Navigation.PushModalAsync(wishlistPage, false);
        }

        private async void OnLikeClicked(object sender, EventArgs e)
        {
            await this.SwipeCardView.InvokeSwipe(SwipeCardDirection.Right);

            System.Reflection.PropertyInfo pi = SwipeCardView.TopItem.GetType().GetProperty("ProfileId");
            string name = (string)(pi.GetValue(SwipeCardView.TopItem, null));

            await CreateWishlist(Settings.UserKey, name);
        }

        private async void MoreInfoClicked(object sender, EventArgs e)
        {
            //var json = JsonConvert.SerializeObject(SwipeCardView.TopItem);
            //var prodObject = JsonConvert.DeserializeObject<ProductModel>(json);
            System.Reflection.PropertyInfo pi = SwipeCardView.TopItem.GetType().GetProperty("ProfileId");
            name = (string)(pi.GetValue(SwipeCardView.TopItem, null));

            var viewItemPage = new Views.ViewItemPage(name);
            await Navigation.PushModalAsync(viewItemPage, false);
        }

        public async Task CreateWishlist(string userNo, string productNo)
        {
            var wish = await firebase
              .Child("Wishlist")
              .PostAsync(new tblWishlist()
              {
                  userNo = userNo,
                  productNo = productNo
              });
            var imageKey = wish.Key;

            //Settings.UserKey = token;
        }

        private async void OnDragging(object sender, DraggingCardEventArgs e)
        {
            var view = (Xamarin.Forms.View)sender;
            var nopeFrame = view.FindByName<Frame>("NopeFrame");
            var likeFrame = view.FindByName<Frame>("LikeFrame");
            var superLikeFrame = view.FindByName<Frame>("SuperLikeFrame");
            var threshold = (this.BindingContext as CardViewModel).Threshold;

            var draggedXPercent = e.DistanceDraggedX / threshold;

            var draggedYPercent = e.DistanceDraggedY / threshold;

            switch (e.Position)
            {
                case DraggingCardPosition.Start:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    nopeButton.Scale = 1;
                    likeButton.Scale = 1;
                    superLikeButton.Scale = 1;
                    break;
                case DraggingCardPosition.UnderThreshold:
                    if (e.Direction == SwipeCardDirection.Left)
                    {
                        nopeFrame.Opacity = (-1) * draggedXPercent;
                        nopeButton.Scale = 1 + draggedXPercent / 2;
                        superLikeFrame.Opacity = 0;
                        superLikeButton.Scale = 1;

                    }
                    else if (e.Direction == SwipeCardDirection.Right)
                    {
                        likeFrame.Opacity = draggedXPercent;
                        likeButton.Scale = 1 - draggedXPercent / 2;
                        superLikeFrame.Opacity = 0;
                        superLikeButton.Scale = 1;

                    }
                    else if (e.Direction == SwipeCardDirection.Up)
                    {
                        nopeFrame.Opacity = 0;
                        likeFrame.Opacity = 0;
                        nopeButton.Scale = 1;
                        likeButton.Scale = 1;
                        superLikeFrame.Opacity = (-1) * draggedYPercent;
                        superLikeButton.Scale = 1 + draggedYPercent / 2;
                    }
                    break;
                case DraggingCardPosition.OverThreshold:
                    if (e.Direction == SwipeCardDirection.Left)
                    {
                        nopeFrame.Opacity = 1;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Right)
                    {
                        likeFrame.Opacity = 1;
                        superLikeFrame.Opacity = 0;
                    }
                    else if (e.Direction == SwipeCardDirection.Up)
                    {
                        nopeFrame.Opacity = 0;
                        likeFrame.Opacity = 0;
                        superLikeFrame.Opacity = 1;
                    }
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    nopeButton.Scale = 1;
                    likeButton.Scale = 1;
                    superLikeButton.Scale = 1;
                    break;
                case DraggingCardPosition.FinishedOverThreshold:
                    nopeFrame.Opacity = 0;
                    likeFrame.Opacity = 0;
                    superLikeFrame.Opacity = 0;
                    nopeButton.Scale = 1;
                    likeButton.Scale = 1;
                    superLikeButton.Scale = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (e.Position == DraggingCardPosition.FinishedOverThreshold && e.Direction == SwipeCardDirection.Right)
            {
                OnSwipedRightCommand();
            }
        }

        public void PurchaseButton_Clicked(object sender, System.EventArgs e)
        {
            
        }

        void SwipeLeftButton_Clicked(object sender, System.EventArgs e)
        {
            
        }

        void SwipeRightButton_Clicked(object sender, System.EventArgs e)
        {
           
        }
    }
}