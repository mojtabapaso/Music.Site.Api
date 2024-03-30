using Music.Domain.Entities;

namespace Music.Application.Interface.Entity;

public interface IWalletServices : IGenericServices<Wallet>
{
    public Task<Wallet> GetWalletByUserIdAsync(string userId);
}
