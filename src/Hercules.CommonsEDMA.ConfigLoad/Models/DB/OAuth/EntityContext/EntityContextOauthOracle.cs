using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth.EntityContext
{
    public class EntityContextOauthOracle : EntityContextOauth
    {
        public EntityContextOauthOracle(DbContextOptions<EntityContextOauth> dbContextOptions, ConfigService configService)
            : base(dbContextOptions, configService)
        {

        }
    }
}
