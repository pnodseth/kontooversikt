namespace BlazorApp;

public class AccountsList
{
    public int AvailableItems { get; set; }
    public  List<AccountItem> Items { get; set; } = new();
}

public class AccountResponse
{
    public AccountItem Item { get; set; } = null!;
    public string ErrorType { get; set; } = null!;
    public bool IsError { get; set; }
    public string ErrorCode { get; set; } = null!;
    public string ErrorMessage { get; set; } = null!;
    public string TraceId { get; set; } = null!;
}

public class AccountItem
{
    public string AccountId { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string OwnerCustomerId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public decimal Available { get; set; }
    public decimal Balance { get; set; }
    public decimal CreditLimit { get; set; }
}