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

public partial class Company
{
    [Key]
    public long CompanyId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public string WebsiteUrl { get; set; }

    [StringLength(50)]
    public string Phone { get; set; }

    public long? PrimaryContactId { get; set; }

    public string YouTubeChannelUrl { get; set; }

    [Required]
    [StringLength(450)]
    public string OwnerApplicationUserId { get; set; }

    [InverseProperty("Company")]
    public virtual ICollection<ContactCompany> ContactCompany { get; set; } = new List<ContactCompany>();

    [ForeignKey("OwnerApplicationUserId")]
    [InverseProperty("Company")]
    public virtual AspNetUsers OwnerApplicationUser { get; set; }

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("Company")]
    public virtual Contact PrimaryContact { get; set; }
}