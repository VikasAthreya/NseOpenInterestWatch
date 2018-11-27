using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace NseOpenInterestWatch
{
    public static class Helper
    {
        public static List<int> GetConfiguredBNfStrikePrices()
        {
            var strikes = new List<int>();

            var bNfStrikePriceRange = ConfigurationManager.AppSettings["BNfStrikePriceRange"].Split(',');
            var lowerRange = Convert.ToInt32(bNfStrikePriceRange.First());
            var upperRange = Convert.ToInt32(bNfStrikePriceRange.Last());

            for (int i = lowerRange; i <= upperRange; i+=100)
            {
                strikes.Add(i);
            }

            return strikes;
        }

//        public static int GetBankNiftySpotPrice()
//        {
//            var web = new HtmlWeb();
//            HtmlDocument doc =
//                web.Load("https://kite.zerodha.com/ext/chart/INDICES/NIFTY%20BANK/260105");
//
//            int bnfSpotPrice = doc.DocumentNode.SelectNodes("//table");
//
//            return bnfSpotPrice;
//
//        }
    }
}