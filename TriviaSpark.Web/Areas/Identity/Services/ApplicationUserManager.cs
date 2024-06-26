using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TriviaSpark.Core.Match.Models;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Areas.Identity.Services;

public class ApplicationUserManager : UserManager<TriviaSparkWebUser>
{
    public ApplicationUserManager(
        IUserStore<TriviaSparkWebUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TriviaSparkWebUser> passwordHasher,
        IEnumerable<IUserValidator<TriviaSparkWebUser>> userValidators,
        IEnumerable<IPasswordValidator<TriviaSparkWebUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TriviaSparkWebUser>> logger) :
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

    private static UserModel Create(TriviaSparkWebUser user)
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
