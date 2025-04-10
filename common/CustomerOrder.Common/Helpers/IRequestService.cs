namespace CustomerOrder.Common.Helpers;
public interface IRequestService
{
    public string UserId { get; }
    public int CompanyId { get; }
    public string CompanyName { get; }

    void SetCompanyId(int companyId);
    void SetUserId(string userId);
    void SetCompanyName(string companyName);
}
