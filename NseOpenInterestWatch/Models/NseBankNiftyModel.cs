using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NseOpenInterestWatch.Models
{
    public class NseBankNiftyModel
    {
        public NseBankNiftyModel()
        {
            NseBankNiftyOiDataForStrikePrice = new List<NseBankNiftyOiDataForStrikePrice>();
        }
        public string AllStrikes { get; set; }
        public List<NseBankNiftyOiDataForStrikePrice> NseBankNiftyOiDataForStrikePrice { get; set; }
    }

    public class NseBankNiftyOiDataForStrikePrice
    {
        public string CeOi { get; set; }
        public string PeOi { get; set; }
        public string DtTm { get; set; }
        public string StrikePrice { get; set; }
    }
}