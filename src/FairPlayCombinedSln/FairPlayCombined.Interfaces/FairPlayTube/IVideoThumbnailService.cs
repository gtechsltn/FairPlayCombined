﻿using FairPlayCombined.Models.FairPlayTube.VideoThumbnail;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoThumbnailService
    {
        Task<PaginationOfT<VideoThumbnailModel>>
            GetPaginatedVideoThumbnailByVideoInfoIdAsync(long videoInfoId, PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
        Task<long> CreateVideoThumbnailAsync(CreateVideoThumbnailModel createModel,
            CancellationToken cancellationToken);
        Task<VideoThumbnailModel[]> GetAllVideoThumbnailAsync(CancellationToken cancellationToken);
        Task<VideoThumbnailModel> GetVideoThumbnailByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteVideoThumbnailByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoThumbnailModel>> GetPaginatedVideoThumbnailAsync(
            PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}