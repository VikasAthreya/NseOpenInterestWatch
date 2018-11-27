using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using NseOpenInterestWatch.EntityFramework;

namespace NseOpenInterestWatch.Code
{
    public interface INseOiDataScraper
    {
        void ScrapNseBankNiftyData();
    }

    public class NseOiDataScraper : INseOiDataScraper
    {
        const string NSE_BULK_DEALS_DIALY = "https://www.nseindia.com/products/dynaContent/equities/equities/bulkdeals.jsp?symbol=&segmentLink=13&symbolCount=&dateRange=day&fromDate=&toDate=&dataType=DEALS";

        public void ScrapNseBankNiftyData()
        {
            string[] bnfRange = ConfigurationManager.AppSettings["BNfStrikePriceRange"].Split(',');

            var web = new HtmlWeb();
            HtmlDocument doc =
                web.Load(
                    "https://www.nseindia.com/live_market/dynaContent/live_watch/option_chain/optionKeys.jsp?symbolCode=-9999&symbol=BANKNIFTY&symbol=BANKNIFTY&instrument=OPTIDX&date=-&segmentLink=17&segmentLink=17");

            //            doc

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                Console.WriteLine("Found: " + table.Id);
                if (table.Id == "octable")
                {
                    using (var context = new NseToolsContext())
                    {
                        var records = new List<BankNiftyOiHistory>();
                        foreach (HtmlNode row in table.SelectNodes("tr"))
                        {
                            Console.WriteLine("row");
                            string innerHtml = row.InnerHtml;
                            HtmlNodeCollection allCells = row.SelectNodes("td");
                            if (allCells.Count > 10 &&
                                !string.IsNullOrWhiteSpace(allCells[11].InnerText) &&
                                Convert.ToDecimal(allCells[11].InnerText) >= Convert.ToInt16(bnfRange[0]) &&
                                Convert.ToDecimal(allCells[11].InnerText) <= Convert.ToInt16(bnfRange[1]))
                            {
                                var record = new BankNiftyOiHistory();
                                //                        Chart
                                //                        OI	                         YES
                                record.CeOi = Convert.ToInt32(allCells[1].InnerText.Replace(",", ""));
                                //                        Chng in OI                         YES
                                //TODO - fix this
//                                record.CeChange = Convert.ToInt32(allCells[2].InnerText.Replace(",", ""));
                                //                        Volume
                                //                        IV
                                //                        LTP
                                //                        Net Chng	
                                //                        Bid Qty
                                //                        Bid Price	
                                //                        Ask Price	
                                //                        Ask Qty
                                //                        Strike Price                         YES
                                record.StrikePrice =
                                    Convert.ToInt32(Convert.ToDecimal(allCells[11].InnerText.Replace(",", "")));
                                //                        Bid Qty
                                //                        Bid Price
                                //                        Ask Price 
                                //                        Ask Qty 
                                //                        Net Chng
                                //                        LTP
                                //                        IV
                                //                        Volume
                                //                        Chng in OI                          YES
                               
                                //TODO - fix this
                                // record.PeChange = Convert.ToInt32(allCells[20].InnerText.Replace(",", ""));
                                //                        OI                          YES
                                if (allCells[21].InnerText.Contains(","))
                                {
                                    record.PeOi = Convert.ToInt32(allCells[21].InnerText.Replace(",", ""));    
                                }
                                
                                //                        Chart


                                record.ModifiedDateTime = DateTime.Now;
                                records.Add(record);

                                //                            innerHtml = innerHtml.Replace(",", "");
                                ////                        string replacement = Regex.Replace(innerText, @"\t|\n|\r", ",");
                                //                            string replacement = Regex.Replace(innerHtml, @"\n", ",");
                                //                            var csvRow = replacement.Split(',');
                                //                            csvRow.ForEach(x => x = x.Trim());
                                //
                                //                            foreach (HtmlNode cell in row.SelectNodes("th|td"))
                                //                            {
                                //                                Console.WriteLine("cell: " + cell.InnerText);
                                //                            }
                            }
                        }

                        //add the row
                        context.BankNiftyOiHistories.AddRange(records);
                        context.SaveChanges();
                    }
                }
            }
        }

//        private bool isAlreadyDownloaded(DateTime date)
//        {
//            using (var context = new db_Entities())
//            {
//                var previousData = context.NseBulkDeals.OrderByDescending(x => x.Date).ToList();
//                if (previousData[0].Date == date)
//                    return true;
//                return false;
//            }
//        }
    }

}