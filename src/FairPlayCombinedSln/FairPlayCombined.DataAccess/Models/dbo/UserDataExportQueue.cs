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


namespace FairPlayCombined.DataAccess.Models.dboSchema;

[Index("ApplicationUserId", Name = "IX_UserDataExportQueue_ApplicationUserId")]
public partial class UserDataExportQueue
{
    [Key]
    public long UserDataExportQueueId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    public bool IsCompleted { get; set; }

    public string FileUrl { get; set; }

    public DateTimeOffset RowCreationDateTime { get; set; }

    [Required]
    [StringLength(256)]
    public string RowCreationUser { get; set; }

    [Required]
    [StringLength(250)]
    public string SourceApplication { get; set; }

    [Required]
    [Column("OriginatorIPAddress")]
    [StringLength(100)]
    public string OriginatorIpaddress { get; set; }

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("UserDataExportQueue")]
    public virtual AspNetUsers ApplicationUser { get; set; }
}