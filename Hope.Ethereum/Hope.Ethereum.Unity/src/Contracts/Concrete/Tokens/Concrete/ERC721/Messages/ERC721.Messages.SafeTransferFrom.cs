using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace Hope.Ethereum.Tokens
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
            /// Class which contains the data needed to execute the ERC721 safeTransferFrom function.
            /// </summary>
            [Function("safeTransferFrom")]
            public sealed class SafeTransferFrom : FunctionMessage
            {
                /// <summary>
                /// The address transferring the ERC721 token.
                /// </summary>
                [Parameter("address", "_from", 1)]
                public string From { get; set; }

                /// <summary>
                /// The address to transfer the ERC721 token to.
                /// </summary>
                [Parameter("address", "_to", 2)]
                public string To { get; set; }

                /// <summary>
                /// The id of the ERC721 token to send.
                /// </summary>
                [Parameter("uint256", "_tokenId", 3)]
                public BigInteger TokenId { get; set; }
            }
        }
    }
}