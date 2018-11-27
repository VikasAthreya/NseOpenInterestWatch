﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NseOpenInterestWatch.EntityFramework;
using NseOpenInterestWatch.Models;

namespace NseOpenInterestWatch.Controllers
{
    public class HomeController : Controller
    {
//        private NseToolsContext dbContext;
//        public HomeController()
//        {
//            dbContext = new DbContext();
//        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult NewChart()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error("ss", "Something bad happened");


            var model = GetNseBankNiftyData();
            var json = JsonConvert.SerializeObject(model);
            //Source data returned as JSON  
            return Json(json, JsonRequestBehavior.AllowGet);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        

        private NseBankNiftyModel GetNseBankNiftyData()
        {
            dynamic ce = new JObject();
            var finalss = new NseBankNiftyModel();
            var final = new List<BankNiftyOiHistory>();
            using (var context = new NseToolsContext())
            {
                var result = context.BankNiftyOiHistories.OrderBy(x=>x.StrikePrice)
                    .GroupBy(x => x.StrikePrice).Select(grp => grp.ToList()).ToList();
                var temp = new List<string>();

                finalss.AllStrikes = JsonConvert.SerializeObject(result.Select(x => x.First().StrikePrice).ToList());
                
//                var result1 = context.BankNiftyOiHistories.GroupBy(x => x.StrikePrice).Select(grp => grp.ToList()[0]).ToList();
                foreach (var res in result)
                {
                    var sorted = res.OrderBy(x => x.ModifiedDateTime).ToList();

                    var allCes = sorted.Select(x => x.CeOi).ToList();
                    var allPes = sorted.Select(x => x.PeOi).ToList();
                    var allDateTime = sorted.Select(x => x.ModifiedDateTime != null ? x.ModifiedDateTime.Value.ToShortTimeString() : "").ToList();
                    var allStrikes = sorted.Select(x => x.StrikePrice).Distinct().ToList();

                    finalss.NseBankNiftyOiDataForStrikePrice.Add(new NseBankNiftyOiDataForStrikePrice
                    {
                        StrikePrice = JsonConvert.SerializeObject(allStrikes),
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