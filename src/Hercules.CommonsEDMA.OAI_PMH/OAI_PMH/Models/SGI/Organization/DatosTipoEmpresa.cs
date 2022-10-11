using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Organization
{
    /// <summary>
    /// Datos tipo empresa
    /// </summary>
    public class DatosTipoEmpresa : SGI_Base
    {
        /// <summary>
        /// Tipo de tercero.
        /// </summary>
        public TipoTercero TipoTercero { get; set; }
        /// <summary>
        /// Tipo de empresa. 
        /// </summary>
        public TipoEmpresa TipoEmpresa { get; set; }
        /// <summary>
        /// Tipo de empresa de contabilidad.
        /// </summary>
        public TipoEmpresaContabilidad TipoEmpresaContabilidad { get; set; }
        /// <summary>
        /// Tipo de tercero para personas físicas con dirección en el Reino Unido.
        /// </summary>
        public TipoTerceroReinoUnido TipoTerceroReinoUnido { get; set; }
    }
}
