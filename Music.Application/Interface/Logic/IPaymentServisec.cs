namespace Music.Application.Interface.Logic;
public interface IPaymentServisec
{
    public Task<HttpResponseMessage> PaymentAsync(string amount, string CallbackURL, string description=null, string phoneNumber=null);

}
