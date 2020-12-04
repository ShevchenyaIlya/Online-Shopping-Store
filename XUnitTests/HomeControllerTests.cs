using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductStore;
using ProductStore.Areas.Identity.Data;
using ProductStore.Controllers;
using ProductStore.Data;
using ProductStore.Models;
using ProductStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        //private AuthDbContext dbContext;

        const string cn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EventDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        const string cn1 = "Server=(localdb)\\mssqllocaldb;Database=ProductStore;Trusted_Connection=True;MultipleActiveResultSets=true";
        public HomeControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/Index")]
        public async Task CheckCorrectContent(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task TestFindProduct()
        {
            var logger = new Mock<ILogger<HomeController>>();
            //var options = new DbContextOptionsBuilder<AuthDbContext>()
            //    .UseSqlServer(cn1).Options;
            //dbContext = new AuthDbContext(options);
            var products = new Mock<IProductRepository>();
            var category = new Mock<ICategoryRepository>();
            var order = new Mock<IOrderRepository>();
            var mark = new Mock<IMarkRepository>();
            var user = new Mock<IUserRepository>();
            var sale = new Mock<ISaleRepository>();
            var comment = new Mock<ICommentRepository>();
            var basket = new Mock<IProductBasketRepository>();
            var controller = new HomeController(logger.Object, products.Object, category.Object, user.Object,
                mark.Object, comment.Object, order.Object, sale.Object, basket.Object);
            var result = await controller.FindProduct("apple");

            //var viewResult = Assert.IsType<PartialViewResult>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<Product>>(
            //    viewResult.ViewData.Model);
            //Assert.Single(model);
        }

        [Fact]
        public async Task TestIndex()
        {
            //var logger = new Mock<ILogger<HomeController>>();
            //var options = new DbContextOptionsBuilder<AuthDbContext>()
            //.UseSqlServer(cn1).Options;
            //dbContext = new AuthDbContext(options);
            //var controller = new HomeController(dbContext, logger.Object);
            //var result = await controller.Index();

            //var viewResult = Assert.IsType<ViewResult>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<Product>>(
            //    viewResult.ViewData.Model);

            //Assert.Equal(5, model.Count());
            //Assert.Equal(1, viewResult.ViewData.Count);
        }
    }
}
