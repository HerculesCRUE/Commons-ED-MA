using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid.EntityContext
{
    public class EntityContextAcidPostgres : EntityContextAcid
    {
        /// <summary>
        /// Constructor internal, para obtener un objeto EntityContext, llamar al m�todo ObtenerEntityContext del BaseAD
        /// </summary>
        public EntityContextAcidPostgres(DbContextOptions<EntityContextAcid> dbContextOptions, ConfigService configService)
            : base(dbContextOptions, configService)
        {

        }
    }
}
