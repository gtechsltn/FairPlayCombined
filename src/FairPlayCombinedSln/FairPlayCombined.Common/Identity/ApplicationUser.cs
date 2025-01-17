using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Common.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class
#pragma warning disable S2094 // Classes should not be empty
public class ApplicationUser : IdentityUser
#pragma warning restore S2094 // Classes should not be empty
{
    [ProtectedPersonalData]
    public string? Name { get; set; }
    [ProtectedPersonalData]
    public string? Lastname { get; set; }
    [ProtectedPersonalData]
    public string? LinkedInProfileUrl { get; set; }
    [ProtectedPersonalData]
    public string? InstagramProfileUrl { get; set; }
    [ProtectedPersonalData]
    public string? XformerlyTwitterUrl { get; set; }
    [ProtectedPersonalData]
    public string? WebsiteUrl { get; set; }
    [ProtectedPersonalData]
    public string? ReasonToCreateAccount { get; set; }
    [ProtectedPersonalData]
    public DateTimeOffset? RowCreationDateTime { get; set; }

    [ProtectedPersonalData]
    public string? RowCreationUser { get; set; }

    [ProtectedPersonalData]
    public string? SourceApplication { get; set; }
    [ProtectedPersonalData]
    public string? OriginatorIpaddress { get; set; }
}

