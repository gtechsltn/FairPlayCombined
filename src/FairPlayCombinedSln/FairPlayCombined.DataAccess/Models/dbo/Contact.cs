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

[Index("EmailAddress", Name = "UI_Contact_EmailAddress", IsUnique = true)]
public partial class Contact
{
    [Key]
    public long ContactId { get; set; }

    [Required]
    [StringLength(450)]
    public string OwnerApplicationUserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Lastname { get; set; }

    [Required]
    [StringLength(50)]
    public string EmailAddress { get; set; }

    [StringLength(50)]
    public string LinkedInProfileUrl { get; set; }

    [StringLength(50)]
    public string YouTubeChannelUrl { get; set; }

    public string InstagramUrl { get; set; }

    [Column("XFormerlyTwitterUrl")]
    public string XformerlyTwitterUrl { get; set; }

    [StringLength(50)]
    public string BusinessPhoneNumber { get; set; }

    [StringLength(50)]
    public string MobilePhoneNumber { get; set; }

    public DateTimeOffset? BirthDate { get; set; }

    [InverseProperty("PrimaryContact")]
    public virtual ICollection<Company> Company { get; set; } = new List<Company>();

    [InverseProperty("Contact")]
    public virtual ICollection<ContactCompany> ContactCompany { get; set; } = new List<ContactCompany>();

    [ForeignKey("OwnerApplicationUserId")]
    [InverseProperty("Contact")]
    public virtual AspNetUsers OwnerApplicationUser { get; set; }
}