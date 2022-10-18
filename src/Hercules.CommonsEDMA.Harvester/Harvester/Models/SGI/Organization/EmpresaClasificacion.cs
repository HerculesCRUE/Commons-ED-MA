﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Organization
{
    /// <summary>
    /// Empresa clasificación
    /// </summary>
    public class EmpresaClasificacion
    {
        /// <summary>
        /// Identificador de la clasificación.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Código de la clasificación.
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// Nombre de la clasificación.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Identificador de la entidad padre de la clasificación.
        /// </summary>
        public string PadreId { get; set; }
    }
}