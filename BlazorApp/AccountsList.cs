namespace BlazorApp;

public class AccountsList
{
    public int AvailableItems { get; set; }
    public List<Account> Items { get; set; }
}

public class Account
{
    public string AccountId { get; set; }
    public string Name { get; set; }
    public decimal Available { get; set; }
    public decimal Balance { get; set; }
    // add additional fields as needed to fully describe the account structure.

}