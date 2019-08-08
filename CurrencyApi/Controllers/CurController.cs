using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurController : ControllerBase
    {
        private  IMemoryCache _Cache;

        public CurController(IMemoryCache cache)
        {
            this._Cache = cache;
            urls = new List<Url>()
            {
        new Url()   {id=i++, name="طلای 18 عیار" ,url="http://www.tgju.org/chart/geram18"},
        new Url()   {id=i++, name= "طلای 24 عیار" ,url="http://www.tgju.org/chart/geram24" },
        new Url()   {id=i++, name= "طلای آب شده " ,url="http://www.tgju.org/chart/gold_futures" },

        new Url()   {id=i++,name= "سکه طرح امام" ,url="http://www.tgju.org/chart/sekee" },
        new Url()   {id=i++,name= "سکه تمام بهار آزادی" ,url="http://www.tgju.org/chart/sekeb" },
        new Url()   {id=i++,name= "سکه نیم بهار آزادی" ,url="http://www.tgju.org/chart/nim" },
        new Url()   {id=i++,name= "سکه ربع بهار آزادی" ,url="http://www.tgju.org/chart/rob" },


        new Url()   {id=i++,name= "یورو" ,url="http://www.tgju.org/chart/price_eur" },
        new Url()   {id=i++,name= "دلار" ,url="http://www.tgju.org/chart/price_dollar_rl"},
        new Url()   {id=i++,name= "لیر" ,url="http://www.tgju.org/chart/price_try" },
        new Url()   {id=i++,name= "پوند" ,url="http://www.tgju.org/chart/price_gbp" },
        new Url()   {id=i++,name= "دینار" ,url="http://www.tgju.org/chart/price_iqd" },
        new Url()   {id=i++,name= "یوان" ,url="http://www.tgju.org/chart/price_cny" },
        new Url()   {id=i++,name= "مانات" ,url="http://www.tgju.org/chart/price_azn" }
            };
        }
        List<Url> urls;
        private int i = 1;

        public IActionResult GetCur()
        {
            List<currency> currencies=new List<currency>();
            if (!_Cache.TryGetValue("pep",out currencies))
            {
                if (currencies == null)
                {
                    currencies = FillData();
                }
                _Cache.Set("pep", currencies, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }

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

            List<currency> currencies = new List<currency>();
            if (!_Cache.TryGetValue("luis", out currencies))
            {
                if (currencies == null)
                {
                    currencies = FillData(id);
                }
                _Cache.Set("luis", currencies, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }
            return Ok(currencies);
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