using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Olive.AppSpecific
{
    [Table("tblProducts")]
    public class tblProducts
    {
        [PrimaryKey, AutoIncrement]
        public int productNo
        { get; set; }
        [NotNull, MaxLength(20)]
        public string prodCategory
        { get; set; }
        [NotNull, MaxLength(20)]
        public string prodSubCategory
        { get; set; }
        [NotNull]
        public decimal prodPrice
        { get; set; }
        [NotNull, MaxLength(100)]
        public string prodDescription
        { get; set; }
        [MaxLength(6)]
        public string prodSize
        { get; set; }
        [MaxLength(20)]
        public string prodColour
        { get; set; }
        [NotNull]
        public string prodImageString
        { get; set; }
        [MaxLength(20)]
        public string prodBrand
        { get; set; }
        [NotNull, MaxLength(50)]
        public string prodLocation
        { get; set; }
        public bool prodSold
        { get; set; }
        public int prodQuantity
        { get; set; }
    }
}
