using Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid.EntityContext;
using Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth.EntityContext;
using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.OAuth
{
    public static class ControladorOAuth
    {
        public static bool CrearNuevoOAuth(DB.Acid.Proyecto proyecto,EntityContextAcid entityContextAcid, EntityContextOauth entityContextOAuth, ConfigService configService)
        {
            DB.Acid.Usuario filaUsuarioAcid = entityContextAcid.Usuario.FirstOrDefault(x => x.Login == configService.ObtenerLoginAdmin());

            DB.OAuth.Usuario filaUsuarioOauth = entityContextOAuth.Usuario.FirstOrDefault(usuario => usuario.UsuarioID.Equals(filaUsuarioAcid.UsuarioID));

            DB.OAuth.UsuarioConsumer usuarioConsumer = entityContextOAuth.UsuarioConsumer.Include(item => item.OAuthConsumer).ThenInclude(item => item.ConsumerData).Include(item => item.OAuthConsumer).ThenInclude(item => item.OAuthToken).FirstOrDefault(x => x.ProyectoID == proyecto.ProyectoID && x.UsuarioID==filaUsuarioAcid.UsuarioID);
            if (usuarioConsumer != null)
            {
                usuarioConsumer.OAuthConsumer.ConsumerKey = configService.ObtenerAPIConsumerKey();
                usuarioConsumer.OAuthConsumer.ConsumerSecret = configService.ObtenerAPIConsumerSecret();
                entityContextOAuth.SaveChanges();

                usuarioConsumer.OAuthConsumer.ConsumerData.Nombre = "Hércules";
                usuarioConsumer.OAuthConsumer.ConsumerData.UrlOrigen = "-";
                entityContextOAuth.SaveChanges();

                usuarioConsumer.OAuthConsumer.OAuthToken.First().Token = configService.ObtenerAPITokenKey();
                usuarioConsumer.OAuthConsumer.OAuthToken.First().TokenSecret = configService.ObtenerAPITokenSecret();
                usuarioConsumer.OAuthConsumer.OAuthToken.First().State = 2;
                usuarioConsumer.OAuthConsumer.OAuthToken.First().UsuarioID = filaUsuarioAcid.UsuarioID;
                usuarioConsumer.OAuthConsumer.OAuthToken.First().ConsumerVersion = "1.0.1";
                entityContextOAuth.SaveChanges();
            }
            else
            {
                DB.OAuth.OAuthConsumer filaOAuthConsumer = new DB.OAuth.OAuthConsumer();
                filaOAuthConsumer.ConsumerKey = configService.ObtenerAPIConsumerKey();
                filaOAuthConsumer.ConsumerSecret = configService.ObtenerAPIConsumerSecret();
                filaOAuthConsumer.VerificationCodeFormat = 1;
                filaOAuthConsumer.VerificationCodeLength = 1;
                entityContextOAuth.OAuthConsumer.Add(filaOAuthConsumer);
                entityContextOAuth.SaveChanges();

                DB.OAuth.ConsumerData filaConsumer = new DB.OAuth.ConsumerData();
                filaConsumer.ConsumerId = filaOAuthConsumer.ConsumerId;
                filaConsumer.Nombre = "Hércules";
                filaConsumer.UrlOrigen = "-";
                filaConsumer.FechaAlta = DateTime.UtcNow;
                entityContextOAuth.ConsumerData.Add(filaConsumer);
                entityContextOAuth.SaveChanges();

                DB.OAuth.UsuarioConsumer filaUsuarioConsumer = new DB.OAuth.UsuarioConsumer();
                filaUsuarioConsumer.UsuarioID = filaUsuarioAcid.UsuarioID;
                filaUsuarioConsumer.ConsumerId = filaOAuthConsumer.ConsumerId;
                filaUsuarioConsumer.ProyectoID = proyecto.ProyectoID;
                entityContextOAuth.UsuarioConsumer.Add(filaUsuarioConsumer);
                entityContextOAuth.SaveChanges();

                DB.OAuth.OAuthToken filaToken = new DB.OAuth.OAuthToken();
                filaToken.Token = configService.ObtenerAPITokenKey();
                filaToken.TokenSecret = configService.ObtenerAPITokenSecret();
                filaToken.State = 2;
                filaToken.IssueDate = DateTime.UtcNow;
                filaToken.ConsumerId = filaOAuthConsumer.ConsumerId;
                filaToken.UsuarioID = filaUsuarioAcid.UsuarioID;
                filaToken.ConsumerVersion = "1.0.1";
                entityContextOAuth.OAuthToken.Add(filaToken);
                entityContextOAuth.SaveChanges();
            }
            return true;
        }
    }
}
