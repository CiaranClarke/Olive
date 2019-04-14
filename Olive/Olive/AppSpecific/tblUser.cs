using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLite.Net.Attributes;

namespace Olive.AppSpecific
{
    [Table("tblUser")]
    public class tblUser
    {
        [PrimaryKey]
        public string userNo
        { get; set; }
        [NotNull, MaxLength(20)]
        public string userFirstName
        { get; set; }
        [NotNull, MaxLength(20)]
        public string userLastName
        { get; set; }
        [NotNull, MaxLength(100)]
        public string userEmail
        { get; set; }
        [NotNull, MaxLength(300)]
        public string userPassword
        { get; set; }
        [NotNull, MaxLength(20)]
        public string userTelephoneNo
        { get; set; }
        [NotNull, MaxLength(50)]
        public string userAddressLine1
        { get; set; }
        [MaxLength(50)]
        public string userAddressLine2
        { get; set; }
        [NotNull, MaxLength(25)]
        public string userCity
        { get; set; }
        [MaxLength(25)]
        public string userCounty
        { get; set; }
        [MaxLength(10)]
        public string userPostcode
        { get; set; }
        [NotNull]
        public DateTime userDateAdded
        { get; set; }
    }
}
