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
        /// Class which contains the different messages which interact/change the ERC721 token contract values on the blockchain.
        /// </summary>
        public static partial class Messages
        {
            /// <summary>
            /// Class which contains the data needed to execute the ERC721 approve function.
            /// </summary>
            [Function("approve")]
            public sealed class Approve : FunctionMessage
            {
                /// <summary>
                /// The address to give approval to.
                /// </summary>
                [Parameter("address", "_to", 1)]
                public string To { get; set; }

                /// <summary>
                /// The id of the ERC721 token to approve.
                /// </summary>
                [Parameter("uint256", "_tokenId", 3)]
                public BigInteger TokenId { get; set; }
            }
        }
    }
}