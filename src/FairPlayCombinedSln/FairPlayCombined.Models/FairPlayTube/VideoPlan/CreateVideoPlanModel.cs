﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.VideoPlan
{
    public class CreateVideoPlanModel: ICreateModel
    {
        [Required]
        public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? VideoName { get; set; }

        [Required]
        [StringLength(500)]
        public string? VideoDescription { get; set; }

        [Required]
        [StringLength(3000)]
        public string? VideoScript { get; set; }
    }
}
