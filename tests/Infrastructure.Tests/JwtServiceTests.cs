using CRN.ProductApi.Infrastructure.Identity; using Xunit;
namespace CRN.ProductApi.Infrastructure.Tests;
public class JwtServiceTests { [Fact] public void PasswordHash_RoundTrips(){var hash=PasswordService.Hash("Secret@123");Assert.True(PasswordService.Verify("Secret@123",hash));Assert.False(PasswordService.Verify("wrong",hash));} }
