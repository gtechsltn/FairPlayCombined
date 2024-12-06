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

[Table("VideoPlan", Schema = "FairPlayTube")]
[Index("VideoName", "ApplicationUserId", Name = "IX_VideoPlan_VideoName", IsUnique = true)]
public partial class VideoPlan
{
    [Key]
    public long VideoPlanId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    [Required]
    [StringLength(50)]
    public string VideoName { get; set; }

    [Required]
    [StringLength(500)]
    public string VideoDescription { get; set; }

    [Required]
    [StringLength(3000)]
    public string VideoScript { get; set; }

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("VideoPlan")]
    public virtual AspNetUsers ApplicationUser { get; set; }

    [InverseProperty("VideoPlan")]
    public virtual ICollection<VideoPlanThumbnail> VideoPlanThumbnail { get; set; } = new List<VideoPlanThumbnail>();
}