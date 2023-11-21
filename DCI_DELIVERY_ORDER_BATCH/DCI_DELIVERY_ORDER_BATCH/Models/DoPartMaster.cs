using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DCI_DELIVERY_ORDER_BATCH.Models
{
    public partial class DoPartMaster
    {
        public int PartId { get; set; }
        public string Partno { get; set; }
        public string Cm { get; set; }
        public string VdCode { get; set; }
        public string Description { get; set; }
        public int? Pdlt { get; set; }
        public string Unit { get; set; }
        public int? BoxMin { get; set; }
        public int? BoxMax { get; set; }
        public int? BoxQty { get; set; }
    }
}
