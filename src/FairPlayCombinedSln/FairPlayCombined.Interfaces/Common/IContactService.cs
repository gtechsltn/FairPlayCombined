using FairPlayCombined.Models.Common.Contact;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Interfaces.Common;
public interface IContactService
{
    Task<long> CreateContactAsync(
    CreateContactModel createModel,
    CancellationToken cancellationToken
    );
    Task<ContactModel[]> GetAllContactAsync(
    CancellationToken cancellationToken);
    Task<ContactModel> GetContactByIdAsync(
    long id,
    CancellationToken cancellationToken);
    Task DeleteContactByIdAsync(
    long id,
    CancellationToken cancellationToken);
    Task<PaginationOfT<ContactModel>> GetPaginatedContactAsync(
    PaginationRequest paginationRequest,
    CancellationToken cancellationToken);
}