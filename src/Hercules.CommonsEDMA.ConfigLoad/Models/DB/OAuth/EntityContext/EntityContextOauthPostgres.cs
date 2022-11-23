using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth.EntityContext
{
    public class EntityContextOauthPostgres : EntityContextOauth
    {
        public EntityContextOauthPostgres(DbContextOptions<EntityContextOauth> dbContextOptions, ConfigService configService)
            : base(dbContextOptions, configService)
        {

        }
    }
}
