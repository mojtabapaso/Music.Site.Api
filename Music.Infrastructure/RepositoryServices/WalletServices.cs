using Microsoft.EntityFrameworkCore;
using Music.Application.Interface.Entity;
using Music.Domain.Entities;
using Music.Infrastructure.Context;

namespace Music.Infrastructure.Services;

public class WalletServices : GenericServices<Wallet>, IWalletServices
{
	private MusicDBContext context;
	public WalletServices(MusicDBContext context) : base(context)
	{
		this.context = context;
	}
	public async Task<Wallet> GetUserWalletAsync(string userId)
	{
		return  await context.Wallets.Where(w=>w.User.Id == userId).FirstOrDefaultAsync();
	}
}