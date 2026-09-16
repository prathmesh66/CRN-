using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CRN.ProductApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CRN.ProductApi.API.Tests;

public class ProductsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ProductsApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("admin", "Admin@123"));
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return auth!.AccessToken;
    }

    private async Task<string> GetUserTokenAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("user", "User@123"));
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return auth!.AccessToken;
    }

    [Fact]
    public async Task Auth_Login_AdminAndUser_Succeeds()
    {
        var adminRes = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("admin", "Admin@123"));
        Assert.Equal(HttpStatusCode.OK, adminRes.StatusCode);
        var adminAuth = await adminRes.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        Assert.NotNull(adminAuth);
        Assert.False(string.IsNullOrWhiteSpace(adminAuth.AccessToken));

        var userRes = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("user", "User@123"));
        Assert.Equal(HttpStatusCode.OK, userRes.StatusCode);
        var userAuth = await userRes.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        Assert.NotNull(userAuth);
        Assert.False(string.IsNullOrWhiteSpace(userAuth.AccessToken));
    }

    [Fact]
    public async Task Auth_Refresh_RotatesTokensSuccessfully()
    {
        var loginRes = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("admin", "Admin@123"));
        var auth = await loginRes.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        Assert.NotNull(auth);

        var refreshRes = await _client.PostAsJsonAsync("/api/v1/auth/refresh", new RefreshRequest(auth.AccessToken, auth.RefreshToken));
        Assert.Equal(HttpStatusCode.OK, refreshRes.StatusCode);

        var refreshedAuth = await refreshRes.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        Assert.NotNull(refreshedAuth);
        Assert.False(string.IsNullOrWhiteSpace(refreshedAuth.AccessToken));
        Assert.NotEqual(auth.RefreshToken, refreshedAuth.RefreshToken);
    }

    [Fact]
    public async Task Products_Get_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/v1/products");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Products_Create_AsUser_ReturnsForbidden()
    {
        var userToken = await GetUserTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/products");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        request.Content = JsonContent.Create(new CreateProductDto("Test Product", new List<int> { 5, 10 }));

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Products_FullCrudAndItemsFlow_Succeeds()
    {
        var adminToken = await GetAdminTokenAsync();

        // 1. Create Product
        var createReq = new HttpRequestMessage(HttpMethod.Post, "/api/v1/products");
        createReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        createReq.Content = JsonContent.Create(new CreateProductDto("Gaming Laptop", new List<int> { 3, 7 }));
        var createRes = await _client.SendAsync(createReq);
        Assert.Equal(HttpStatusCode.Created, createRes.StatusCode);

        var created = await createRes.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        Assert.NotNull(created);
        Assert.Equal("Gaming Laptop", created.ProductName);
        Assert.Equal(2, created.Items.Count);
        int productId = created.Id;

        // 2. Get Product by ID
        var getReq = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/products/{productId}");
        getReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var getRes = await _client.SendAsync(getReq);
        Assert.Equal(HttpStatusCode.OK, getRes.StatusCode);
        var fetched = await getRes.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        Assert.NotNull(fetched);
        Assert.Equal(productId, fetched.Id);

        // 3. Get Items for Product
        var getItemsReq = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/products/{productId}/items");
        getItemsReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var getItemsRes = await _client.SendAsync(getItemsReq);
        Assert.Equal(HttpStatusCode.OK, getItemsRes.StatusCode);
        var items = await getItemsRes.Content.ReadFromJsonAsync<List<ItemDto>>(JsonOptions);
        Assert.NotNull(items);
        Assert.Equal(2, items.Count);

        // 4. Get Paged Products
        var pagedReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/products?pageNumber=1&pageSize=10");
        pagedReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var pagedRes = await _client.SendAsync(pagedReq);
        Assert.Equal(HttpStatusCode.OK, pagedRes.StatusCode);
        var paged = await pagedRes.Content.ReadFromJsonAsync<PagedResult<ProductDto>>(JsonOptions);
        Assert.NotNull(paged);
        Assert.True(paged.TotalCount > 0);
        Assert.True(paged.TotalPages >= 1);

        // 5. Update Product
        var updateReq = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/products/{productId}");
        updateReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        updateReq.Content = JsonContent.Create(new UpdateProductDto("Gaming Laptop Pro", new List<int> { 10 }));
        var updateRes = await _client.SendAsync(updateReq);
        Assert.Equal(HttpStatusCode.OK, updateRes.StatusCode);
        var updated = await updateRes.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        Assert.NotNull(updated);
        Assert.Equal("Gaming Laptop Pro", updated.ProductName);
        Assert.Single(updated.Items);

        // 6. Delete Product
        var deleteReq = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/products/{productId}");
        deleteReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var deleteRes = await _client.SendAsync(deleteReq);
        Assert.Equal(HttpStatusCode.NoContent, deleteRes.StatusCode);

        // 7. Verify Deleted
        var verifyReq = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/products/{productId}");
        verifyReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var verifyRes = await _client.SendAsync(verifyReq);
        Assert.Equal(HttpStatusCode.NotFound, verifyRes.StatusCode);
    }

    [Fact]
    public async Task Products_ValidationErrors_FormattedCorrectly()
    {
        var adminToken = await GetAdminTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/products");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        request.Content = JsonContent.Create(new { productName = "", quantities = new List<int>() });

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        Assert.Equal(400, root.GetProperty("statusCode").GetInt32());
        Assert.Equal("Validation failed", root.GetProperty("message").GetString());
        var errors = root.GetProperty("errors");
        Assert.True(errors.GetArrayLength() > 0);
    }
}
