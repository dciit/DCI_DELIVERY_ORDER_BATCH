using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DCI_DELIVERY_ORDER_BATCH.Models
{
    public partial class DoStockAlpha
    {
        public DateTime DatePd { get; set; }
        public string Partno { get; set; }
        public string Cm { get; set; }
        public string Vdcode { get; set; }
        public double Stock { get; set; }
        public int Rev { get; set; }
        public DateTime? InsertDt { get; set; }
    }
}
