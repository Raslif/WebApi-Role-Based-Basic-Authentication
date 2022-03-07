using BasicAuthentication.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace BasicAuthentication.Controllers
{
    public class ValuesController : ApiController
    {
        [BasicAuthentication]
        [MyAuthorize(Roles = "Admin,Superadmin")]
        public IEnumerable<string> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return new string[] { identity.Claims.FirstOrDefault(c => c.Type == "ID").Value,
                                  identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value, 
                                  identity.Name };
        }

        [BasicAuthentication]
        [MyAuthorize(Roles = "Superadmin")]
        public string Get(int id)
        {
            return $"Value - {id}";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
