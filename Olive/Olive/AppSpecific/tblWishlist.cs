using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Olive.AppSpecific
{
    public class tblWishlist
    {
        [NotNull, MaxLength(20)]
        public string userNo
        { get; set; }
        [NotNull, MaxLength(20)]
        public string productNo
        { get; set; }
    }
}
