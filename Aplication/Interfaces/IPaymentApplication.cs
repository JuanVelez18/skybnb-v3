using application.DTOs;

namespace application.Interfaces
{
    public interface IPaymentApplication
    {
        Task CreatePaymentAsync(Guid userId, PaymentCreationDto paymentCreationDto);
    }
}