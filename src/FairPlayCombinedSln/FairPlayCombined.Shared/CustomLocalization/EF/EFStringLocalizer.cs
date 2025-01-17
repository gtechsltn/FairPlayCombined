﻿using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FairPlayCombined.Shared.CustomLocalization.EF
{
    /// <summary>
    /// Handles EF-based localization
    /// </summary>
    /// <remarks>
    /// Initializes <see cref="EFStringLocalizer"/>
    /// </remarks>
    /// <param name="dbContextFactory"></param>
    /// <param name="customCache"></param>
    /// <param name="logger"></param>
    public class EFStringLocalizer(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ICustomCache customCache, ILogger<EFStringLocalizer> logger) : IStringLocalizer
    {
        private readonly Lock _lock=new();
        /// <summary>
        /// Returns the value for the given key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        /// <summary>
        /// Returns the value for the given key using the supplied arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        /// <summary>
        /// Sets the Culture to use
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new EFStringLocalizer(dbContextFactory, customCache, logger);
        }

        /// <summary>
        /// Gets all of the strings
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            string cacheKey = $"{nameof(GetAllStrings)} -{CultureInfo.CurrentCulture.Name}";
            var db = dbContextFactory.CreateDbContext();
            var result = customCache!.GetOrCreateAsync<CustomLocalizedString[]>(cacheKey,
                retrieveDataTask:() =>
                {
                    logger.LogInformation("Executing method {MethodName}", nameof(GetAllStrings));
                    var data = 
                    db.Resource
                    .AsNoTracking()
                    .Include(r => r.Culture)
                    .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name)
                    .Select(r => new CustomLocalizedString
                    {
                        Name = r.Key,
                        Value = r.Value
                    })
                    .ToArray();
                    return Task.FromResult(data);
                },
                expiration: Constants.CacheConfiguration.LocalizationCacheDuration,
                cancellationToken: CancellationToken.None).Result;
            return result!.Select(r=>new LocalizedString(r.Name!, r.Value!));
        }

        private string? GetString(string name)
        {
            using (this._lock.EnterScope())
            {
                var cacheKey = $"{name}-{CultureInfo.CurrentCulture.Name}";
                var result = customCache!.GetOrCreateAsync<string?>(key: cacheKey,
                    retrieveDataTask: () =>
                {
                    logger.LogInformation("Executing method {MethodName} for resource {ResourceName}", nameof(GetString), name);
                    var allStrings = this.GetAllStrings(false);
                    var data = allStrings
                    .FirstOrDefault(r => r.Name == name)?.Value;
                    return Task.FromResult(data);
                }, expiration: Constants.CacheConfiguration.LocalizationCacheDuration,
                cancellationToken: CancellationToken.None)
                    .Result;
                return result;
            }
        }
    }
}
