using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace Hope.Ethereum.Tokens
{
    /// <summary>
    /// Class which mimics the ethereum ERC721 token standard and contains most functions which are active in the token standard.
    /// </summary>
    public partial class ERC721
    {
        /// <summary>
        /// Class which contains the different queries for receiving data from the ERC721 token contract.
        /// </summary>
        public static partial class Queries
        {
            /// <summary>
            /// Class which contains the data needed to read the approved address of a token id of the ERC721 token contract.
            /// </summary>
            [Function("getApproved", "address")]
            public sealed class GetApproved : FunctionMessage
            {
                [Parameter("uint256", "_tokenId", 1)]
                public BigInteger TokenId { get; set; }
            }
        }
    }
}