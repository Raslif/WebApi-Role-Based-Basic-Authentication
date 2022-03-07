using BasicAuthentication.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BasicAuthentication.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
                SetUnAuthorizedResponse(actionContext);

            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string[] usrCredentials = decodedAuthToken.Split(':');
                string usrname = usrCredentials[0];
                string usrpass = usrCredentials[1];

                // We can do a DB call to check the availability of data
                if (UserValidate.Login(usrname, usrpass))
                {
                    var userDetails = UserValidate.GetUserDetails(usrname, usrpass);

                    var identity = new GenericIdentity(usrname, actionContext.Request.Headers.Authorization.Scheme);
                    identity.AddClaim(new Claim(ClaimTypes.Email, userDetails.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Name, userDetails.UserName));
                    identity.AddClaim(new Claim("ID", Convert.ToString(userDetails.ID)));

                    IPrincipal principal = new GenericPrincipal(identity, userDetails.Roles.Split(','));
                    Thread.CurrentPrincipal = HttpContext.Current.User = principal;

                    //HttpContext.Current.User = new UserDetails(usrname, principal.Identity.IsAuthenticated, userDetails.Roles.Split(','));
                }
                
                else
                    SetUnAuthorizedResponse(actionContext);
            }
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        private static HttpActionContext SetUnAuthorizedResponse(HttpActionContext httpActionContext)
        {
            httpActionContext.Response = httpActionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Authorization-denied",
                                                httpActionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
            return httpActionContext;
        }
    }

    public class UserDetails : IPrincipal
    {
        public UserDetails(string userName, bool isAuthenticated, string[] roles)
        {
            UserName = userName;
            IsAuthenticated = isAuthenticated;
            Roles = roles;
        }
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public IIdentity Identity { get; private set; }
        public string[] Roles { get; set; }
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}