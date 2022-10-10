﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Gnoss.Web.Login.SAML.Models.Entities
{
    [ExcludeFromCodeCoverage]
    public class TokenSAML
    {
        public TokenSAML()
        {            
        }

        [Key]
        public Guid Token { get; set; }
    }
}
