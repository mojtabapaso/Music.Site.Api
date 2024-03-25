using Music.Domain.Entities;

namespace Music.Application.Interface.Entity;

public interface ISubscriptionServices : IGenericServices<Subscription>
{
    public Task<Subscription> GetSubscriptionByUserIdAsync(string userId);
    public Task SubscriptionExpirationCheckerAsync();
}