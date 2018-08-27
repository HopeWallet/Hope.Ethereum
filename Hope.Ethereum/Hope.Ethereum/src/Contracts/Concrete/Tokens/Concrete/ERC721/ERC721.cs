using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hope.Ethereum.Tokens
{
    public sealed partial class ERC721 : Token
    {
        public ERC721(string mainnetAddress) : base(mainnetAddress)
        {
        }

        public ERC721(string mainnetAddress, string rinkebyAddress) : base(mainnetAddress, rinkebyAddress)
        {
        }

        public ERC721(string mainnetAddress, string name, string symbol, int decimals) : base(mainnetAddress, name, symbol, decimals)
        {
        }

        public ERC721(string mainnetAddress, string rinkebyAddress, string name, string symbol, int decimals) : base(mainnetAddress, rinkebyAddress, name, symbol, decimals)
        {
        }

        public override Task<int> QueryDecimals()
        {
            return Task.Run(() => 0);
        }

        public override Task<string> QueryName()
        {
            throw new NotImplementedException();
        }

        public override Task<string> QuerySymbol()
        {
            throw new NotImplementedException();
        }
    }
}
