using AutoMapper; using CRN.ProductApi.Application.DTOs; using CRN.ProductApi.Application.Mapping; using CRN.ProductApi.Application.Services; using CRN.ProductApi.Application.Interfaces; using CRN.ProductApi.Domain.Entities; using Moq; using Xunit;
namespace CRN.ProductApi.Application.Tests;
public class ProductServiceTests
{
 [Fact] public async Task CreateAsync_CreatesProduct(){var repo=new Mock<IProductRepository>();var uow=new Mock<IUnitOfWork>();var mapper=new MapperConfiguration(c=>c.AddProfile<MappingProfile>()).CreateMapper();var sut=new ProductService(repo.Object,uow.Object,mapper);var result=await sut.CreateAsync(new CreateProductDto("Laptop",new(){2,3}),"admin",default);Assert.Equal("Laptop",result.ProductName);Assert.Equal(2,result.Items.Count);repo.Verify(x=>x.AddAsync(It.IsAny<Product>(),It.IsAny<CancellationToken>()),Times.Once);uow.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Once);}
}
