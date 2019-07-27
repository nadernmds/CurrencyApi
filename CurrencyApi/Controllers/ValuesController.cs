using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private  IMemoryCache _Cache;

        public ValuesController(IMemoryCache cache)
        {
            _Cache = cache;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            string date;
            if (!_Cache.TryGetValue("pep",out date))
            {
                if (date==null)
                {
                    date = Cashe();
                }
                _Cache.Set("pep", date,new DateTimeOffset(DateTime.Now.AddSeconds(10)));
            }
            return date;
        }

        private  string Cashe()
        {
            return DateTime.Now.ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
