﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NseOpenInterestWatch.EntityFramework;
using NseOpenInterestWatch.Models;

namespace NseOpenInterestWatch.Hubs
{
    public class NseOiWatchHub : Hub
    {

        public static void Refresh()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NseOiWatchHub>();
            hubContext.Clients.All.refreshPage();
        }

        public NseBankNiftyModel Refresh(string name, string message)
        {
            try
            {
                return GetNseBankNiftyData();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private NseBankNiftyModel GetNseBankNiftyData()
        {
            dynamic ce = new JObject();
            var finalss = new NseBankNiftyModel();
            var final = new List<BankNiftyOiHistory>();
            using (var context = new NseToolsContext())
            {
                var result = context.BankNiftyOiHistories.OrderBy(x => x.StrikePrice)
                    .GroupBy(x => x.StrikePrice).Select(grp => grp.ToList()).ToList();
                var temp = new List<string>();

                finalss.AllStrikes = JsonConvert.SerializeObject(result.Select(x => x.First().StrikePrice).ToList());

                //                var result1 = context.BankNiftyOiHistories.GroupBy(x => x.StrikePrice).Select(grp => grp.ToList()[0]).ToList();
                foreach (var res in result)
                {
                    var sorted = res.OrderBy(x => x.ModifiedDateTime).ToList();
                    var allCes = sorted.Select(x => x.CeOi).ToList();
                    var allPes = sorted.Select(x => x.PeOi).ToList();
                    var allDateTime = sorted.Select(x => x.ModifiedDateTime != null ? x.ModifiedDateTime.Value.ToLocalTime() : new DateTime()).ToList();

                    finalss.NseBankNiftyOiDataForStrikePrice.Add(new NseBankNiftyOiDataForStrikePrice
                    {
                        CeOi = JsonConvert.SerializeObject(allCes),
                        PeOi = JsonConvert.SerializeObject(allPes),
                        DtTm = JsonConvert.SerializeObject(allDateTime)
                    });
                }
                //                var res = result.First();
                //
                //                var sorted = res.OrderBy(x => x.ModifiedDateTime).ToList();
                //                var allCes = sorted.Select(x => x.CeOi).ToList();
                //                var allPes = sorted.Select(x => x.PeOi).ToList();
                //                var allDateTime = sorted.Select(x => x.ModifiedDateTime).ToList();
                //
                //                finalss.NseBankNiftyOiDataForStrikePrice.CeOi = JsonConvert.SerializeObject(allCes);
                //                finalss.NseBankNiftyOiDataForStrikePrice.PeOi = JsonConvert.SerializeObject(allPes);
                //                finalss.NseBankNiftyOiDataForStrikePrice.DtTm = JsonConvert.SerializeObject(allDateTime);
                return finalss;
            }
        }

    }
}