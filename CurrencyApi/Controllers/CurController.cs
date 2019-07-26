using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurController : ControllerBase
    {
        List<Url> urls;
        private int i = 1;
        public CurController()
        {
            urls = new List<Url>()
            {
        new Url()   {id=i++, name="gold_18" ,url="http://www.tgju.org/chart/geram18"},
        new Url()   {id=i++, name= "gold_24" ,url="http://www.tgju.org/chart/geram24" },
        new Url()   {id=i++, name= "gold_melted " ,url="http://www.tgju.org/chart/gold_futures" },

        new Url()   {id=i++,name= "coin_imam" ,url="http://www.tgju.org/chart/sekee" },
        new Url()   {id=i++,name= "coin_bahareAzadi" ,url="http://www.tgju.org/chart/sekeb" },
        new Url()   {id=i++,name= "dollar_nim" ,url="http://www.tgju.org/chart/nim" },
        new Url()   {id=i++,name= "dollar_rob" ,url="http://www.tgju.org/chart/rob" },


        new Url()   {id=i++,name= "euro" ,url="http://www.tgju.org/chart/price_eur" },
        new Url()   {id=i++,name= "dollar" ,url="http://www.tgju.org/chart/price_dollar_rl"},
        new Url()   {id=i++,name= "lir" ,url="http://www.tgju.org/chart/price_try" },
        new Url()   {id=i++,name= "pond" ,url="http://www.tgju.org/chart/price_gbp" },
        new Url()   {id=i++,name= "dinar" ,url="http://www.tgju.org/chart/price_iqd" },
        new Url()   {id=i++,name= "yoan" ,url="http://www.tgju.org/chart/price_cny" },
        new Url()   {id=i++,name= "menat" ,url="http://www.tgju.org/chart/price_azn" }
            };
        }
        [ResponseCache(Duration = 60)]
        public IActionResult GetCur()
        {

            var currencies = FillData();
            return Ok(currencies);
        }

        private List<currency> FillData(int? item = 0)
        {
            List<currency> currencies = new List<currency>();
            if (!(item == 0))
            {
                urls = urls.Where(c => c.id == item).ToList();
            }
            foreach (var url in urls)
            {
                List<info> info = new List<info>();
                {
                    var web = new HtmlWeb();
                    var doc = web.Load(url.url);

                    var ul = doc.DocumentNode.SelectNodes("//ul[@class='data-line float-right float-half']");

                    var lis = ul.Descendants("li")
                     .Select(y => y.Descendants())
                     .ToList();

                    foreach (var li in lis)
                    {
                        var strong = li.First().InnerHtml;
                        var span = li.Last().InnerHtml;
                        bool? high = null;
                        try
                        {
                            var items = li.ToList();
                            string spanClass = items[2].Attributes["class"].Value;
                            if (spanClass == "high")
                            {
                                high = true;
                            }
                            else
                            {
                                high = false;
                            }
                        }
                        catch
                        {
                        }
                        info infos = new info
                        {
                            name = strong,
                            value = span,
                            high = high
                        };
                        info.Add(infos);
                    }
                }
                currency currency = new currency { currencyName = url.name, url = url.url, infos = info };
                currencies.Add(currency);
            }
            return currencies;
        }
        [HttpGet("{id}")]
        [ResponseCache(Duration =60)]
        public IActionResult getCur(int id)
        {

            var s = FillData(id);
            return Ok(s);
        }
    }
    class Url
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }
    class info
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool? high { get; set; }
    }

    class currency
    {
        public string url { get; set; }
        public string currencyName { get; set; }
        public IEnumerable<info> infos { get; set; }

    }
}