using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DCI_DELIVERY_ORDER_BATCH.Models
{
    public partial class DoDictMstr
    {
        public int DictId { get; set; }
        public string DictType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string RefCode { get; set; }
        public string Note { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string DictStatus { get; set; }
    }
}
