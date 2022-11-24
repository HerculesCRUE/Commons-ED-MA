using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid.EntityContext
{
    public class EntityContextAcidOracle : EntityContextAcid
    {
        public EntityContextAcidOracle(DbContextOptions<EntityContextAcid> dbContextOptions, ConfigService configService)
            : base(dbContextOptions, configService)
        {

        }
    }
}
