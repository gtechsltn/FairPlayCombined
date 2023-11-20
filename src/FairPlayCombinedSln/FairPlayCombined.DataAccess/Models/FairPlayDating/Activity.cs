﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.DataAccess.Models.FairPlayShopSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;


namespace FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;

[Table("Activity", Schema = "FairPlayDating")]
public partial class Activity
{
    [Key]
    public int ActivityId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<UserActivity> UserActivity { get; set; } = new List<UserActivity>();
}