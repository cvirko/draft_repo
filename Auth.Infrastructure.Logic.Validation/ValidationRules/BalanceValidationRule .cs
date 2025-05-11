namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class BalanceValidationRule(IRegexService regex, Action<ErrorStatus, object[]> action) 
        : ValidationRule<decimal>(regex, action), IBalanceValidationRule
    {
        public override bool IsLengthFormatValid(decimal value)
        {
            if (value == 0)
            {
                AddError(ErrorStatus.AccessDenied);
                return false;
            }
            return true;
        }
        public bool IsPositive(decimal amount)
        {
            if (amount > 0 ) return true;
            AddError(ErrorStatus.Format, amount);
            return false;
        }
        public async Task<bool> IsEnoughFundsAsync(decimal amount,Guid userId, Func<Guid,decimal,Task<decimal?>> getExpectedBalanceAsync)
        {
            if (amount > 0) return true;
            var expectedBalance = 
                await getExpectedBalanceAsync(userId, amount);
            if (expectedBalance == null)
            {
                AddError(ErrorStatus.NotFound, userId);
                return false;
            }
            if (expectedBalance >= 0) return true;
            AddError(ErrorStatus.InsufficientFunds, -expectedBalance);
            return false;
        }
    }
}
