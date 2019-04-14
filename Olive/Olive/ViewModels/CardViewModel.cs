using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using MLToolkit.Forms.SwipeCardView;
using Olive.SwipeFunctions;

namespace Olive.ViewModels
{
    public class CardViewModel : BasePageViewModel
    {
        private ObservableCollection<ProductModel> _profiles = new ObservableCollection<ProductModel>();

        private uint _threshold;
        private float _backCardScale;

        public CardViewModel()
        {
            InitializeProfiles();

            Threshold = (uint)(App.ScreenWidth / 3);
            
            SwipedLeftCommand = new Command<SwipedCardEventArgs>(OnSwipedLeftCommand);
            SwipedRightCommand = new Command<SwipedCardEventArgs>(OnSwipedRightCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);
            _backCardScale = 0.8f;
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

        private void InitializeProfiles()
        {
            this.Profiles.Add(new ProductModel { ProfileId = 1, Name = "Dark T Shirt", Size = "Medium", Photo = "tshirt1.jpg" });
            //this.Profiles.Add(new ProductModel { ProfileId = 2, Name = "Just Do It T Shirt", Size = "Small", Photo = "tshirt2.jpg" });
            this.Profiles.Add(new ProductModel { ProfileId = 3, Name = "Yellow T Shirt", Size = "Large", Photo = "tshirt3.jpg" });
            this.Profiles.Add(new ProductModel { ProfileId = 4, Name = "Kahoot T Shirt ", Size = "Small", Photo = "tshirt4.png" });
            //this.Profiles.Add(new ProductModel { ProfileId = 5, Name = "Abby ", Age = 25, Photo = "p589739.jpg" });
        }
    }
}
