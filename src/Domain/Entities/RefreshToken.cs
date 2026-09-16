namespace CRN.ProductApi.Domain.Entities;
public class RefreshToken
{
 public int Id { get; set; }
 public int UserAccountId { get; set; }
 public string TokenHash { get; set; } = string.Empty;
 public DateTime ExpiresOnUtc { get; set; }
 public DateTime CreatedOnUtc { get; set; }
 public DateTime? RevokedOnUtc { get; set; }
 public UserAccount UserAccount { get; set; } = null!;
 public bool IsActive => RevokedOnUtc is null && ExpiresOnUtc > DateTime.UtcNow;
}
