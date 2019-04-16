using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using MLToolkit.Forms.SwipeCardView;
using Olive.SwipeFunctions;
using System.Threading.Tasks;
using Olive.AppSpecific;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Olive.Views;

namespace Olive.ViewModels
{
    public class CardViewModel : BasePageViewModel
    {
        private ObservableCollection<ProductModel> _profiles = new ObservableCollection<ProductModel>();

        private uint _threshold;
        private float _backCardScale;
        FirebaseClient firebase = new FirebaseClient("https://olive-4a870.firebaseio.com/"); //Firebase Database URL  
        List<tblProducts> productList = new List<tblProducts>();
        List<tblProductImages> imageList = new List<tblProductImages>();
        private string _topItem;


        public CardViewModel()
        {
            InitializeProfiles();

            Threshold = (uint)(App.ScreenWidth / 4);
            _threshold = (uint)(App.ScreenWidth / 4);

            SwipedLeftCommand = new Command<SwipedCardEventArgs>(OnSwipedLeftCommand);
            SwipedRightCommand = new Command<SwipedCardEventArgs>(OnSwipedRightCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);
            ClearItemsCommand = new Command(OnClearItemsCommand);
            AddItemsCommand = new Command(OnAddItemsCommand);
        }

        public ObservableCollection<ProductModel> Profiles
        {
            get => _profiles;
            set
            {
                _profiles = value;
                RaisePropertyChanged();
            }
        }

        public string TopItem
        {
            get => _topItem;
            set
            {
                _topItem = value;
                RaisePropertyChanged();
            }
        }

        public uint Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                RaisePropertyChanged();
            }
        }

        public float BackCardScale
        {
            get => _backCardScale;
            set
            {
                _backCardScale = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SwipedLeftCommand { get; }

        public ICommand SwipedRightCommand { get; }

        public ICommand DraggingCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }

        private void OnSwipedLeftCommand(SwipedCardEventArgs eventArgs)
        {
        }

        private void OnSwipedRightCommand(SwipedCardEventArgs eventArgs)
        {
            //CentrePage c = new CentrePage();
            //c.OnSwipedRightCommand();
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

        private void OnDraggingCommand(DraggingCardEventArgs eventArgs)
        {
            switch (eventArgs.Position)
            {
                case DraggingCardPosition.Start:
                    return;
                case DraggingCardPosition.UnderThreshold:
                    break;
                case DraggingCardPosition.OverThreshold:
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    return;
                case DraggingCardPosition.FinishedOverThreshold:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnClearItemsCommand()
        {
            Profiles.Clear();
        }

        private void OnAddItemsCommand()
        {
        }

        public async Task<List<tblProducts>> CreateProducts()
        {
            var db = new FirebaseClient(
              "https://olive-4a870.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(Settings.authToken)
              });

            var dbData = (await db
                    .Child("Products")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblProducts>()).Select(item => new tblProducts
                     {
                         prodDescription = item.Object.prodDescription,
                         prodSize = item.Object.prodSize,
                         productNo = item.Key
                     }).ToList();
            
            foreach (object e in dbData)
            {
                var json = JsonConvert.SerializeObject(e);
                var prodObject = JsonConvert.DeserializeObject<tblProducts>(json);
                productList.Add(prodObject);
            }

            //prodNoForImages = some.Object.productNo;
            return productList;
        }

        public async Task<List<tblProductImages>> CreateProductImages()
        {

            var db = new FirebaseClient(
             "https://olive-4a870.firebaseio.com/",
             new FirebaseOptions
             {
                 AuthTokenAsyncFactory = () => Task.FromResult(Settings.authToken)
             });

            var dbData = (await db
                    .Child("ProductImages")
                    //.Child(data.User.LocalId)
                    .OnceAsync<tblProductImages>()).Select(item => new tblProductImages
                    {
                        prodImageString = item.Object.prodImageString,
                        productNo = item.Object.productNo
                    }).ToList();

            foreach (object e in dbData)
            {
                var json = JsonConvert.SerializeObject(e);
                var prodImageObject = JsonConvert.DeserializeObject<tblProductImages>(json);
                imageList.Add(prodImageObject);
            }

            //prodNoForImages = some.Object.productNo;
            return imageList;
        }

        private async void InitializeProfiles()
        {
            try 
            { 
                await CreateProducts();
                await CreateProductImages();

                var result = from f in productList
                             join s in imageList on f.productNo equals s.productNo into g
                             select new
                             {
                                 f.productNo,
                                 f.prodDescription,
                                 f.prodSize,
                                 g.First().prodImageString
                             };

                foreach (var e in result)
                {
                    byte[] imageByte = Convert.FromBase64String(e.prodImageString);
                    ImageSource imgSrc = ImageSource.FromStream(() => new MemoryStream(imageByte));
                    Image im = new Image();
                    im.HeightRequest = 500;
                    im.WidthRequest = 400;
                    im.Source = imgSrc;

                    this.Profiles.Add(new ProductModel { ProfileId = e.productNo, Name = e.prodDescription, Size = e.prodSize, Photo = im.Source });
                    //prodNo = e.productNo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
