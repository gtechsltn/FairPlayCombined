﻿using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Models.FairPlaySocial.Post;
using FairPlayCombined.Services.FairPlaySocial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace FairPlaySocial.MinimalApiEndpoints
{
    public static class MinimalApiEndpointsExtensions
    {
        public static WebApplication MapClientAppsEndpoints(this WebApplication app,
            string clientAppsAuthPolicy)
        {
            app.MapGet("/api/authtest", () =>
            {
                return "Auth worked!!!";
            }).RequireAuthorization(policyNames: clientAppsAuthPolicy);
            app.MapGet("api/photoimage/{photoId}", async (
                [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
                CancellationToken cancellationToken,
                long photoId) =>
            {
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var photoEntity = await dbContext.Photo.AsNoTracking().SingleAsync(p => p.PhotoId == photoId);
                var mimeType = MediaTypeNames.Image.Png;
                return Results.File(photoEntity.PhotoBytes, contentType: mimeType);
            });
            var postsGroup = app.MapGroup("/api/posts");
            postsGroup.MapPost("createPost", async ([FromServices] PostService postService,
                CreatePostModel createPostModel,
                CancellationToken cancellationToken) =>
                {
                    var postId = await postService.CreatePostAsync(createPostModel,
                        cancellationToken: cancellationToken);
                    return postId;
                })
                .RequireAuthorization(policyNames: clientAppsAuthPolicy);
            return app;
        }
    }
}
