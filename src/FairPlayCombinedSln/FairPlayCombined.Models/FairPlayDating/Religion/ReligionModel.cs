﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.Religion
{
    public class ReligionModel : IListModel
    {
        public int? ReligionId { get; set; }
        public string? Name { get; set; }
    }
}
