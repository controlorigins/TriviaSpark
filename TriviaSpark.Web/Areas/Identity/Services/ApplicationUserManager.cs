using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TriviaSpark.Core.Match.Models;

namespace TriviaSpark.Web.Areas.Identity.Services;

public class ApplicationUserManager : UserManager<Core.Match.Entities.TriviaSparkWebUser>
{
    public ApplicationUserManager(
        IUserStore<Core.Match.Entities.TriviaSparkWebUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<Core.Match.Entities.TriviaSparkWebUser> passwordHasher,
        IEnumerable<IUserValidator<Core.Match.Entities.TriviaSparkWebUser>> userValidators,
        IEnumerable<IPasswordValidator<Core.Match.Entities.TriviaSparkWebUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<Core.Match.Entities.TriviaSparkWebUser>> logger) :
        base(store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
    {



    }

    /// <summary>
    /// Get User Model
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserModel?> GetUserModelById(string userId)
    {
        var user = await FindByIdAsync(userId);
        if (user == null) { return null; }

        return Create(user);
    }

    private static UserModel Create(Core.Match.Entities.TriviaSparkWebUser user)
    {
        return new UserModel
        {
            UserId = user.Id,
            Email = user.Email ?? user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
        };
    }
}
