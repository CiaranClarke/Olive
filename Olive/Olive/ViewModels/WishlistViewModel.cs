using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Olive.ViewModels
{
    public class WishlistViewModel
    {
        private List<ItemProduct> _myWishlist;
        //private static List<tblIphoneIncidents> incidentsList;
        public static bool hasWishlist = true;
        public event PropertyChangedEventHandler PropertyChanged;

        public WishlistViewModel()   
        {
            LoadData();
        }

        protected bool SetProperty<T>(
            ref T backingStore,
            T value,
            [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnCultureChanged(CultureInfo culture)
        {
        }

        public List<ItemProduct> WishlistItems
        {
            get { return _myWishlist; }
            set { SetProperty(ref _myWishlist, value); }
        }

        private void LoadData()
        {
            //var cases = CreateCases();
            //foreach(var case1 in cases)
            //{
            //    _myCases.Add(case1);
            //}

            _myWishlist = CreateWishlist();
        }

        public static List<ItemProduct> CreateWishlist()
        {
            var result = new List<ItemProduct>();

            try
            {
                //string str_JSONincident =  AppServiceLiveToPhone.GetLiveDBIncidents(Convert.ToInt32(Settings.LiveIpCoID)).Result;
                //string str_JSONincident = AppServiceLiveToPhone.getLiveDBIncidentsSync(Convert.ToInt32(Settings.LiveIpCoID)).Result;

                //if (str_JSONincident != "0")
                //{
                //    foreach (LiveIncidents item in JsonConvert.DeserializeObject<List<LiveIncidents>>(str_JSONincident))
                //    {
                //        result.Add(
                //           new MyCases
                //           {
                //               Id = item.IpInID,
                //               Name = "Case No. " + item.IpInID.ToString(),
                //               BackgroundColor = Color.FromHex("#00162E"),
                //               BackgroundImage = "",
                //               Icon = GrialShapesFont.Event,
                //               Badge = 2,
                //               CaseInfo = item.IpInDateAdded.ToString()
                //           });
                //    }
                    //hasWishlist = true;
                //}
                //else
                //{
                //    hasWishlist = false;
                //}
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        //public async static void GetIncidents()
        //{
        //    await AppServiceLiveToPhone.GetLiveDBIncidents(Convert.ToInt32(Settings.LiveIpCoID));
        //}

    }
}
