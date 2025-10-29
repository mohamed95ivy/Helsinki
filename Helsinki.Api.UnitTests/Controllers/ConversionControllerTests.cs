using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Helsinki.Api.Controllers;
using Helsinki.Api.Dtos;
using Helsinki.Application.Interfaces.Services;
using Helsinki.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Helsinki.Api.UnitTests.Controllers
{
    public class ConversionControllerTests
    {
        private static Mock<IMapper> CreateMapperMock()
        {
            var mapper = new Mock<IMapper>(MockBehavior.Strict);
            mapper.Setup(m => m.Map<ConversionResponseDto>(It.IsAny<ConversionHistory>()))
                  .Returns((ConversionHistory ch) => new ConversionResponseDto
                  {
                      ConversionId = ch.ConversionId,
                      FromCurrency = ch.FromCurrency,
                      ToCurrency = ch.ToCurrency,
                      FromAmount = ch.FromAmount,
                      ToAmount = ch.ToAmount,
                      ExchangeRate = ch.ExchangeRate,
                      ConversionDate = ch.ConversionDate
                  });
            return mapper;
        }

        private static (IFixture fx, Mock<IConversionService> svc, Mock<IMapper> mapper) Arrange()
        {
            var fx = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var svc = fx.Freeze<Mock<IConversionService>>();
            var mapper = CreateMapperMock();
            return (fx, svc, mapper);
        }

        [Fact]
        public async Task Convert_Returns_Ok_And_Maps_Dto()
        {
            var (fx, svc, mapper) = Arrange();

            var rec = fx.Build<ConversionHistory>()
                .With(x => x.FromCurrency, "EUR")
                .With(x => x.ToCurrency, "USD")
                .With(x => x.FromAmount, 100m)
                .With(x => x.ToAmount, 110m)
                .With(x => x.ExchangeRate, 1.1m)
                .With(x => x.UserId, "u1")
                .Create();

            svc.Setup(s => s.ConvertAsync("u1", "EUR", "USD", 100m, It.IsAny<CancellationToken>()))
               .ReturnsAsync(rec);
            var request = new ConversionRequestDto
            {
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Amount = 100m,
                UserId = "u1"
            };
            var ctrl = new ConversionController(svc.Object, mapper.Object);

            var reposnse = await ctrl.Convert(request, default);

            Assert.NotNull(reposnse.Value);
            var dto = Assert.IsType<ConversionResponseDto>(reposnse!.Value);
            Assert.Equal(110m, dto.ToAmount);
            Assert.Equal("EUR", dto.FromCurrency);
            Assert.Equal("USD", dto.ToCurrency);

            svc.Verify(x => x.ConvertAsync(request.UserId, request.FromCurrency, request.ToCurrency, request.Amount, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Convert_Uses_Default_UserId_When_Missing()
        {
            var (fx, svc, mapper) = Arrange();

            var rec = fx.Build<ConversionHistory>()
                .With(x => x.FromCurrency, "EUR")
                .With(x => x.ToCurrency, "USD")
                .With(x => x.FromAmount, 1m)
                .With(x => x.ToAmount, 1.1m)
                .With(x => x.ExchangeRate, 1.1m)
                .Create();

            svc.Setup(s => s.ConvertAsync(
                    It.Is<string>(u => u == "candidate"),
                    "EUR", "USD", 1m, It.IsAny<CancellationToken>()))
               .ReturnsAsync(rec);

            var ctrl = new ConversionController(svc.Object, mapper.Object);

            _ = await ctrl.Convert(new ConversionRequestDto
            {
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Amount = 1m,
                UserId = null
            }, default);

            svc.Verify(x => x.ConvertAsync("candidate", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task History_Forwards_Params_And_Maps_List()
        {
            var (fx, svc, mapper) = Arrange();

            var list = fx.CreateMany<ConversionHistory>(2).ToList();
            list[0].FromCurrency = "EUR"; list[0].ToCurrency = "USD";
            list[1].FromCurrency = "EUR"; list[1].ToCurrency = "GBP";

            svc.Setup(s => s.GetHistoryAsync("u1", 5, 10, true, It.IsAny<CancellationToken>()))
               .ReturnsAsync(list);

            var ctrl = new ConversionController(svc.Object, mapper.Object);

            var result = await ctrl.History(5, 10, "desc", "u1", default);

            Assert.NotNull(result);
            var dtos = Assert.IsType<List<ConversionResponseDto>>(result!.Value);
            Assert.Equal(2, dtos.Count);
            svc.Verify(x => x.GetHistoryAsync(It.IsAny<string>(), 5, 10, true, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task History_Clamps_Take_And_Sets_Asc_Order()
        {
            var (fx, svc, mapper) = Arrange();

            svc.Setup(s => s.GetHistoryAsync(
                    "u1",
                    0,
                    It.Is<int>(t => t == 200),     
                    It.Is<bool>(n => n == false),  
                    It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<ConversionHistory>());

            var ctrl = new ConversionController(svc.Object, mapper.Object);

            _ = await ctrl.History(0, 1000, "asc", "u1", default);

            svc.Verify(x => x.GetHistoryAsync("u1", 0, 200, false, It.IsAny<CancellationToken>()));
        }

    }
}
