using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Hope.Ethereum.Tokens
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
            /// Class which contains the data needed to read whether an operator is approved for all token ids.
            /// </summary>
            [Function("isApprovedForAll", "bool")]
            public sealed class IsApprovedForAll : FunctionMessage
            {
                [Parameter("address", "_owner", 1)]
                public string Owner { get; set; }

                [Parameter("address", "_operator", 1)]
                public string Operator { get; set; }
            }
        }
    }
}