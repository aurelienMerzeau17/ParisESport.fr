using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using ParisESport.fr.Models;

namespace ParisESport.fr
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // Pour permettre aux utilisateurs de ce site de se connecter à l’aide de leur compte à partir d’autres sites tels que Microsoft, Facebook et Twitter,
            // vous devez mettre à jour ce site. Pour plus d’informations, consultez la page http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "5VkYUoEhwl2ca4s4qOrdsSqEc",
                consumerSecret: "JLF1Gk4rnzBwHVUz5IO6FTMTo722xmVPN65gm78fmwv3TQodwK");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "469281396540222",
                appSecret: "f635207b2ec89b7c9a7e1579c3cd89f5");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
