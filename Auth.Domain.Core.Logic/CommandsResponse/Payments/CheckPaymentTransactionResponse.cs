namespace Auth.Domain.Core.Logic.CommandsResponse.Payments
{
    public class CheckPaymentTransactionResponse 
    {
        public CheckPaymentTransactionResponse(){}
        public CheckPaymentTransactionResponse(TimeSpan delay, bool isSuccess)
        {
            Delay = delay;
            IsSuccess = isSuccess;
        }
        public TimeSpan Delay { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsFromQueue { get; set; }
    }
}
