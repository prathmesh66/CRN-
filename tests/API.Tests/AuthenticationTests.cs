using CRN.ProductApi.Infrastructure.Identity; using Xunit;
namespace CRN.ProductApi.API.Tests;
public class AuthenticationTests { [Fact] public void AdminSeedCredentials_AreDocumented(){Assert.True(PasswordService.Verify("Admin@123",PasswordService.Hash("Admin@123")));} }
