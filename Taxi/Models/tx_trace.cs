//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Taxi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tx_trace
    {
        public int Id { get; set; }
        public string header { get; set; }
        public string text { get; set; }
        public string code { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<int> itemID { get; set; }
    }
}
