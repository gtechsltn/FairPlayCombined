﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayBlogsSchema;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.DataAccess.Models.FairPlayShopSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;


namespace FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;

[Table("VideoPlanThumbnail", Schema = "FairPlayTube")]
public partial class VideoPlanThumbnail
{
    [Key]
    public long VideoPlanThumbnailId { get; set; }

    public long VideoPlanId { get; set; }

    [Required]
    public byte[] ImageBytes { get; set; }

    [ForeignKey("VideoPlanId")]
    [InverseProperty("VideoPlanThumbnail")]
    public virtual VideoPlan VideoPlan { get; set; }
}