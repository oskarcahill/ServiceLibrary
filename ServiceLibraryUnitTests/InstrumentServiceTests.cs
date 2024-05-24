using Moq;
using ServiceLibrary;
using System.Net;

[TestClass]
public class InstrumentServiceTests
{
   private Mock<IInstrumentDataAccessWrapper> _instrumentDataAccessMock;
   private Mock<IExchangeRepositoryWrapper> _exchangeRepositoryMock;
   private Mock<IHttpClientWrapper> _httpClientMock;
   private InstrumentService _service;

   [TestInitialize]
   public void Setup()
   {
      _instrumentDataAccessMock = new Mock<IInstrumentDataAccessWrapper>();
      _exchangeRepositoryMock = new Mock<IExchangeRepositoryWrapper>();
      _httpClientMock = new Mock<IHttpClientWrapper>();
      _service = new InstrumentService();

      // Assign the mocks to the static properties of InstrumentService
      InstrumentService.InstrumentDataAccess = _instrumentDataAccessMock.Object;
      InstrumentService.ExchangeRepository = _exchangeRepositoryMock.Object;
      InstrumentService.HttpClient = _httpClientMock.Object;
   }

   [TestMethod]
   public async Task AddPriceSnapshot_ValidInput_AddsPrice()
   {
      // Arrange
      var type = InstrumentType.Equity;
      var name = "TestInstrument";
      var symbols = new string[] { "TST" };
      var exchangeId = 1;
      var instrumentId = 123;
      var micCodes = "MIC123";
      var price = 100.0;
      var exchange = new Exchange(exchangeId, micCodes);

      _instrumentDataAccessMock.Setup(m => m.GetInstrumentIdAsync(name, exchangeId, type, It.IsAny<string>())).ReturnsAsync(0);
      _instrumentDataAccessMock.Setup(m => m.AddInstrumentAsync(type, name, symbols, exchangeId, It.IsAny<string>())).Returns(Task.CompletedTask);
      _instrumentDataAccessMock.Setup(m => m.GetInstrumentIdAsync(name, exchangeId, type, It.IsAny<string>())).ReturnsAsync(instrumentId);
      _exchangeRepositoryMock.Setup(m => m.GetByIdAsync(exchangeId)).ReturnsAsync(exchange);
      _httpClientMock.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
      {
         Content = new StringContent(price.ToString())
      });

      // Act
      await _service.AddPriceSnapshot(type, name, symbols, exchangeId);

      // Assert
      _instrumentDataAccessMock.Verify(m => m.AddInstrumentPriceAsync(instrumentId, price, It.IsAny<string>()), Times.Once);
   }
}