using Microsoft.EntityFrameworkCore;
using Music.Application.Interface.Entity;
using Music.Domain.Entities;
using Music.Infrastructure.Context;

namespace Music.Infrastructure.Services;

public class SubscriptionServices : GenericServices<Subscription>, ISubscriptionServices
{
	private readonly MusicDBContext context;

	public SubscriptionServices(MusicDBContext context) : base(context)
	{
		this.context = context;
	}

	public async Task<Subscription> GetSubscriptionByUserIdAsync(string userId)
	{
		var subscription =  await context.Subscriptions.Where(subscriptions => subscriptions.User.Id == userId).FirstOrDefaultAsync();
		return subscription;
	}

	public async Task SubscriptionExpirationCheckerAsync()
	{
		foreach (var subscription in context.Subscriptions)
		{
			if (subscription.IsActive)
			{
				if (subscription.Create != null && subscription.Expired >= subscription.Create)
				{
					subscription.IsActive = false;
				}
			}
		}
		await context.SaveChangesAsync();
        await Console.Out.WriteLineAsync("Subscription Expiration Checker is done successfully");
    }

}