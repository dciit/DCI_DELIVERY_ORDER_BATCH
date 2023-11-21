using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DCI_DELIVERY_ORDER_BATCH.Models
{
    public partial class ViDoPlan
    {
        public int? Wcno { get; set; }
        public string Prdym { get; set; }
        public string Prdymd { get; set; }
        public int? PdLeadTime { get; set; }
        public string Prdltymd { get; set; }
        public string Model { get; set; }
        public decimal? Qty { get; set; }
        public string Partno { get; set; }
        public string Cm { get; set; }
        public string Partname { get; set; }
        public string Route { get; set; }
        public string Catmat { get; set; }
        public string Exp { get; set; }
        public decimal? Reqqty { get; set; }
        public string Whunit { get; set; }
        public string Vender { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? Consumption { get; set; }
    }
}
