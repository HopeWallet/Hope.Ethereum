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

        [TestMethod]
        public async Task TestGetERC721Data()
        {
            // NOTE:
            // Since CryptoKitties was the first draft of ERC721, many functions were not formalized and decided upon.
            // Therefore, some functions will not work and some others will.
            ERC721 cryptokitties = new ERC721("0x06012c8cf97BEaD5deAe237070F9587f8E7A266d", "CryptoKitties", "CK", 0);

            var balance = await cryptokitties.QueryBalanceOf("0x12b353D1a2842D2272aB5A18C6814D69f4296873");

            Assert.IsTrue(balance > 0);
        }
    }
}
