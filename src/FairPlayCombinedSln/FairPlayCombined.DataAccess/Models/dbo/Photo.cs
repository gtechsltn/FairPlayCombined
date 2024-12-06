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

public partial class Photo
{
    [Key]
    public long PhotoId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Filename { get; set; }

    [Required]
    public byte[] PhotoBytes { get; set; }

    [InverseProperty("HeaderPhoto")]
    public virtual ICollection<Blog> Blog { get; set; } = new List<Blog>();

    [InverseProperty("ThumbnailPhoto")]
    public virtual ICollection<BlogPost> BlogPost { get; set; } = new List<BlogPost>();

    [InverseProperty("Photo")]
    public virtual ICollection<Post> Post { get; set; } = new List<Post>();

    [InverseProperty("ThumbnailPhoto")]
    public virtual ICollection<Product> Product { get; set; } = new List<Product>();

    [InverseProperty("ProfilePhoto")]
    public virtual ICollection<UserProfile> UserProfile { get; set; } = new List<UserProfile>();

    [InverseProperty("Photo")]
    public virtual ICollection<VideoFaceThumbnail> VideoFaceThumbnail { get; set; } = new List<VideoFaceThumbnail>();

    [InverseProperty("VideoThumbnailPhoto")]
    public virtual ICollection<VideoInfo> VideoInfo { get; set; } = new List<VideoInfo>();

    [InverseProperty("Photo")]
    public virtual ICollection<VideoInfographic> VideoInfographic { get; set; } = new List<VideoInfographic>();

    [InverseProperty("Photo")]
    public virtual ICollection<VideoThumbnail> VideoThumbnail { get; set; } = new List<VideoThumbnail>();
}