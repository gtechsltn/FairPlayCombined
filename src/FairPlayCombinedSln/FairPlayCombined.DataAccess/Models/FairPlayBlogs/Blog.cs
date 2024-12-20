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


namespace FairPlayCombined.DataAccess.Models.FairPlayBlogsSchema;

[Table("Blog", Schema = "FairPlayBlogs")]
[Index("Name", Name = "UI_Blog_Name", IsUnique = true)]
public partial class Blog
{
    [Key]
    public long BlogId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    public long HeaderPhotoId { get; set; }

    [StringLength(100)]
    public string CustomDomain { get; set; }

    public bool IsCustomDomainVerified { get; set; }

    [Required]
    [StringLength(450)]
    public string OwnerApplicationUserId { get; set; }

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

    [InverseProperty("Blog")]
    public virtual ICollection<BlogPost> BlogPost { get; set; } = new List<BlogPost>();

    [InverseProperty("Blog")]
    public virtual ICollection<BlogSubscriber> BlogSubscriber { get; set; } = new List<BlogSubscriber>();

    [ForeignKey("HeaderPhotoId")]
    [InverseProperty("Blog")]
    public virtual Photo HeaderPhoto { get; set; }

    [ForeignKey("OwnerApplicationUserId")]
    [InverseProperty("Blog")]
    public virtual AspNetUsers OwnerApplicationUser { get; set; }
}