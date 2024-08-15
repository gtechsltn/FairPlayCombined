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

[Index("ApplicationUserId", Name = "UI_UserMonetizationProfile_ApplicationUserId", IsUnique = true)]
public partial class UserMonetizationProfile
{
    [Key]
    public long UserMonetizationProfileId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    public string GitHubSponsors { get; set; }

    [Column("BuyMeACoffee")]
    public string BuyMeAcoffee { get; set; }

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("UserMonetizationProfile")]
    public virtual AspNetUsers ApplicationUser { get; set; }
}