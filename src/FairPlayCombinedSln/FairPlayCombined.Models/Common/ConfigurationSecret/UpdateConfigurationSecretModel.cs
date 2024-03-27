﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.ConfigurationSecret
{
    public class UpdateConfigurationSecretModel : IUpdateModel
    {
        public int ConfigurationSecretId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        public string? Value { get; set; }
    }
}
