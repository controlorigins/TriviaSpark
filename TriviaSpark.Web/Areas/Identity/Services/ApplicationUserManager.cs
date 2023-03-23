using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
}
