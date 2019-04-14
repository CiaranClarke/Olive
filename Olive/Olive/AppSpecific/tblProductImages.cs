using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Olive.AppSpecific
{
    public class tblProductImages
    {
        [PrimaryKey]
        public string productNo
        { get; set; }
        [NotNull, MaxLength(20)]
        public string prodImageString
        { get; set; }
        
    }
}
