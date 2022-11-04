namespace Tryitter.Services;

using System.Security.Claims;

public class AuthorizationService
{
    public bool VerifyIdentity(string id, ClaimsPrincipal User)
    {      
        var currentUser = User.FindFirst("Id")?.Value;
       
        var isAdmin = User.IsInRole("Admin");

        if(currentUser != id && !isAdmin)
        {
            return false;
        }

        return true;
    }
}