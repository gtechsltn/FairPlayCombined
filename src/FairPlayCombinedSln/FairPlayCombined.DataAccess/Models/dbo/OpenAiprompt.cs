﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.DataAccess.Models.FairPlayShopSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;


namespace FairPlayCombined.DataAccess.Models.dboSchema;

[Table("OpenAIPrompt")]
public partial class OpenAiprompt
{
    [Key]
    [Column("OpenAIPromptId")]
    public long OpenAipromptId { get; set; }

    [Required]
    [StringLength(450)]
    public string OwnerApplicationUserId { get; set; }

    [Column(TypeName = "money")]
    public decimal OperationCost { get; set; }

    [Required]
    public string OriginalPrompt { get; set; }

    [Required]
    public string RevisedPrompt { get; set; }

    [Required]
    [StringLength(50)]
    public string Model { get; set; }

    [Required]
    public byte[] GeneratedImageBytes { get; set; }

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

    [ForeignKey("OwnerApplicationUserId")]
    [InverseProperty("OpenAiprompt")]
    public virtual AspNetUsers OwnerApplicationUser { get; set; }
}