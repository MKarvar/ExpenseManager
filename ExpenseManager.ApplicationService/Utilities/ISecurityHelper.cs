namespace ExpenseManager.ApplicationService.Utilities
{
    public interface ISecurityHelper
    {
        string GetSha256Hash(string input);
    }
}