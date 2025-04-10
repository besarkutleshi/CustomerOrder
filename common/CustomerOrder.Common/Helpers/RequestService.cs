namespace CustomerOrder.Common.Helpers;

public class RequestService : IRequestService
{
    public int CompanyId { get; private set; } = 2;
    public string UserId { get; private set; } = null!;
    public string CompanyName { get; private set; } = null!;

    public void SetCompanyId(int companyId) => CompanyId = companyId;

    public void SetCompanyName(string companyName) => CompanyName = companyName;

    public void SetUserId(string userId) => UserId = userId;
}