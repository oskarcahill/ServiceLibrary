/*
 * You MUST NOT change this code. This is an existing consumer of the InstrumentService class and you must maintain backwards compatibility.
 * 
*/

using ServiceLibrary;

var service = new InstrumentService();
try
{
    await service.AddPriceSnapshot(InstrumentType.Equity, "Apple", new string[] { "AAPL" }, 1);
    Console.Out.WriteLine("Adding price snapshot for Apple Equity instrument was successful");
}
catch (Exception ex)
{
    Console.Out.WriteLine($"Adding price snapshot for Apple Equity instrument was unsuccessful. {ex.GetBaseException().Message}");
}