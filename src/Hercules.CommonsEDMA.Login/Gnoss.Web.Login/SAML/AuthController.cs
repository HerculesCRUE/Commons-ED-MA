extern alias ApiWrapper;

using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.MvcCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System;
using ApiWrapper::Gnoss.ApiWrapper;
using Gnoss.Web.Login.Open;
using System.IO;

namespace Gnoss.Web.Login.SAML
{
    [AllowAnonymous]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private static readonly ResourceApi mResourceApi = new ResourceApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config");
        const string relayStateReturnUrl = "ReturnUrl";
        private Saml2Configuration config;
        readonly ConfigServiceLogin mConfigServiceSAML;

        public AuthController(IOptions<Saml2Configuration> configAccessor, ConfigServiceLogin configServiceSAML)
        {
            config = configAccessor.Value;
            mConfigServiceSAML = configServiceSAML;
        }

        [HttpGet, HttpPost]
        [Route("Login")]
        public IActionResult Login(string returnUrl = null, string token = null)
        {
            mResourceApi.Log.Info($"9.-AuthController Login");
            var binding = new Saml2RedirectBinding();
            binding.SetRelayStateQuery(new Dictionary<string, string> { { relayStateReturnUrl, returnUrl ?? Url.Content("~/") }, { "token", token } });

            return binding.Bind(new Saml2AuthnRequest(config)).ToActionResult();
        }

        [HttpGet, HttpPost]
        [Route("AssertionConsumerService")]
        public async Task<IActionResult> AssertionConsumerService()
        {
            try
            {
                mResourceApi.Log.Info($"10.-AuthController AssertionConsumerService");
                var binding = new Saml2PostBinding();
                var saml2AuthnResponse = new Saml2AuthnResponse(config);

                mResourceApi.Log.Info("saml status0: " + saml2AuthnResponse.Status);

                //ITfoxtec.Identity.Saml2.Http.HttpRequest request = Request.ToGenericHttpRequest();
                //request.Method = "POST";
                //request.QueryString = Request.HttpContext.Request.QueryString.Value;
                Request.Method = "POST";

                binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);
                mResourceApi.Log.Info("saml status1: " + saml2AuthnResponse.Status);
                if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
                {
                    throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
                }


                //binding.Unbind(Request.ToGenericHttpRequest(), saml2AuthnResponse);
                await saml2AuthnResponse.CreateSession(HttpContext, lifetime: new TimeSpan(0, 5, 0), claimsTransform: (claimsPrincipal) => ClaimsTransform.Transform(claimsPrincipal));

                mResourceApi.Log.Info("saml status2 postsession: " + saml2AuthnResponse.Status);


                var relayStateQuery = binding.GetRelayStateQuery();
                mResourceApi.Log.Info("saml status3 postrealystatequery: " + saml2AuthnResponse.Status);

                string token = relayStateQuery["token"];
                string returnUrl = relayStateQuery["ReturnUrl"];


                mResourceApi.Log.Info("content: " + mConfigServiceSAML.GetUrlServiceInDomain() + "?returnUrl=" + returnUrl + "&token=" + token);

                return Redirect(Url.Content(@$"~/{mConfigServiceSAML.GetUrlServiceInDomain()}LoginSAML") + "?returnUrl=" + returnUrl + "&token=" + token);
            }
            catch (Exception ex)
            {
                mResourceApi.Log.Error("Excepcion en AssertionConsumerService: " + ex.Message);
                mResourceApi.Log.Error("Traza en AssertionConsumerService: " + ex.StackTrace);
                return null;
            }
        }

        [HttpGet, HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            //return Redirect(config.SingleLogoutDestination.Scheme + "://" + config.SingleLogoutDestination.Host + "/cas/logout?service="+ config.Issuer);
            var binding = new Saml2PostBinding();
            var saml2LogoutRequest = await new Saml2LogoutRequest(config, User).DeleteSession(HttpContext);
            return binding.Bind(saml2LogoutRequest).ToActionResult();
        }

    }
}
