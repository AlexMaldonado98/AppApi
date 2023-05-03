using AppApi.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

public class CustomOAuthProvider : OAuthAuthorizationServerProvider
{
    public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        context.Validated();
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        var userName = context.UserName;
        var password = context.Password;

        var user = GetUserFromDatabase(userName, password);

        if (user == null)
        {
            context.SetError("invalid_grant", "Nombre de usuario o contraseña incorrectos.");
            return;
        }

        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
        identity.AddClaim(new Claim(ClaimTypes.Name, user.usuario));
        identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

        var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
        context.Validated(ticket);
    }

    private usuarios GetUserFromDatabase(string userName, string password)
    {
        using (var db = new ticketsEntities1())
        {
            var user = db.usuarios.FirstOrDefault(u => u.usuario == userName && u.pass == password);
            return user;
        }
    }
}