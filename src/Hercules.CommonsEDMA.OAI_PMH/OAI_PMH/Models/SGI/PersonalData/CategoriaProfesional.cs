﻿namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Categoría profesional
    /// </summary>
    public class CategoriaProfesional : SGI_Base
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Nombre de la categoría profesional de la persona dentro de la universidad
        /// </summary>
        public string nombre { get; set; }
    }
}