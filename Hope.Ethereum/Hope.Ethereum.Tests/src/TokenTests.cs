using Hope.Ethereum.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Hope.EthereumTests
{
    [TestClass]
    public sealed class TokenTests
    {
        [TestMethod]
        public async Task TestGetTokenData()
        {
            ERC20 prps = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);
            string name = prps.Name;
            string symbol = prps.Symbol;
            int? decimals = prps.Decimals;

            decimal balance = await prps.QueryBalanceOf("0xa8EF8e0855F84F25666Cc5b37C5aB8cBF9de314F");

            Assert.IsTrue(balance > 0);
        }
    }
}
