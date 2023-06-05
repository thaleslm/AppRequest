using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;
namespace AppRequest.Controllers;

public static class ProblemDetailsExtensions
{
    public static  Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications){
        return  notifications
                .GroupBy(g => g.Key)
                .ToDictionary(g => g.Key,g=> g.Select(x => x.Message).ToArray());
    }

    
    public static  Dictionary<string, string[]> ConvertToProblemDetails(this IEnumerable<IdentityError> error){
        var dictionary = new Dictionary<string, string[]>();
        dictionary.Add("error",error.Select(error => error.Description).ToArray());
        return  dictionary;
    }
}