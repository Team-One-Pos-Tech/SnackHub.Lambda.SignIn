using System.Threading.Tasks;
using System.Net.Http;
using Xunit;

namespace HelloWorld.Tests
{
  public class FunctionTest
  {
    private static readonly HttpClient client = new HttpClient();

    [Fact]
    public async Task TestHelloWorldFunctionHandler()
    {
           
    }
  }
}