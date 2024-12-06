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

[Table("VideoComment", Schema = "FairPlayTube")]
public partial class VideoComment
{
    [Key]
    public long VideoCommentId { get; set; }

    public long VideoInfoId { get; set; }

    [Required]
    [StringLength(450)]
    public string ApplicationUserId { get; set; }

    [Required]
    [StringLength(500)]
    public string Comment { get; set; }

    public DateTimeOffset RowCreationDateTime { get; set; }

    [StringLength(256)]
    public string RowCreationUser { get; set; }

    [StringLength(250)]
    public string SourceApplication { get; set; }

    [Column("OriginatorIPAddress")]
    [StringLength(100)]
    public string OriginatorIpaddress { get; set; }

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("VideoComment")]
    public virtual AspNetUsers ApplicationUser { get; set; }

    [ForeignKey("VideoInfoId")]
    [InverseProperty("VideoComment")]
    public virtual VideoInfo VideoInfo { get; set; }
}