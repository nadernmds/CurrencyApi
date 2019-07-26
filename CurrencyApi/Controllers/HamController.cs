using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HamController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<List<CarName>> GetCarAsync(int id)
        {
            return await Data(id);

        }
        //private async Task<List<CarName>> ddd(int id)
        //{
        //    var s = new HttpClient();
        //    var pairs = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("pageindex", id.ToString())
        //    };
        //    var content = new FormUrlEncodedContent(pairs);

        //    var sss = await s.PostAsync("https://www.hamrah-mechanic.com/handler/exhibitionlist2.ashx", content);
        //    var html = await sss.Content.ReadAsStringAsync();
        //    var web = new HtmlWeb();
        //    HtmlDocument doc = new HtmlDocument();
        //    doc.LoadHtml(html);

        //    var div = doc.DocumentNode.SelectNodes("//div[@class='w100f']");
        //    var list = new List<CarName>();

        //    foreach (var item in div)
        //    {
        //        var ssss = item.Descendants("h3").ToList()[0].InnerText;
        //        list.Add(new CarName {
        //            carName= item.Descendants("h3").ToList()[0].InnerText
        //            ,price= item.Descendants("span").ToList()[0].InnerText

        //        });

        //    }
        //    return list;
        //}
        private async Task<List<CarName>> Data(int id)
        {
            var s = new HttpClient();
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pageindex", id.ToString())
            };
            var content = new FormUrlEncodedContent(pairs);

            var sss = await s.PostAsync("https://www.hamrah-mechanic.com/handler/exhibitionlist2.ashx", content);
            var html = await sss.Content.ReadAsStringAsync();
            var web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var div = doc.DocumentNode.SelectNodes("//div[@class='data-container']");
            var list = new List<CarName>();

            foreach (var item in div)
            {
                var ssss = item.Descendants("h3").ToList()[0].InnerText;
                list.Add(new CarName
                {
                    carName = item.Descendants("h3").ToList()[0].InnerText
                    ,
                    price = item.Descendants("span").ToList()[0].InnerText
,
                    url = "https://www.hamrah-mechanic.com/" + item.Descendants("img").ToList()[0].Attributes["src"].Value.ToString()
                });

            }
            return list;
        }
        public class CarName
        {
            public string carName { get; set; }
            public string price { get; set; }
            public string url { get; set; }

        }
    }
}