using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace Hope.Ethereum.Unity.Tokens
{
    /// <summary>
    /// Class which mimics the ethereum ERC721 token standard and contains most functions which are active in the token standard.
    /// </summary>
    public sealed partial class ERC721
    {
        /// <summary>
        /// Class which contains the different queries for receiving data from the ERC721 token contract.
        /// </summary>
        public static partial class Queries
        {
            /// <summary>
            /// Class which contains the data needed to read the token id by index of the ERC721 token contract.
            /// </summary>
            [Function("tokenByIndex", "uint256")]
            public sealed class TokenByIndex : FunctionMessage
            {
                [Parameter("uint256", "_index", 1)]
                public BigInteger Index { get; set; }
            }
        }
    }
}