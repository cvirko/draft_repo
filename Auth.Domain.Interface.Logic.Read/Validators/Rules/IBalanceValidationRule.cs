namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IBalanceValidationRule : IValidationRule<decimal>
    {
        bool IsPositive(decimal amount);
        Task<bool> IsEnoughFundsAsync(decimal amount, Guid userId, Func<Guid, decimal, Task<decimal?>> getExpectedBalanceAsync);
    }
}
