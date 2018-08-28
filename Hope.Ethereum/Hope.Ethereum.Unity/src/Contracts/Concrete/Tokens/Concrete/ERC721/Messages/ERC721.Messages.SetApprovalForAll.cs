using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

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
            /// Class which contains the data needed to execute the ERC721 setApprovalForAll function.
            /// </summary>
            [Function("setApprovalForAll")]
            public sealed class SetApprovalForAll : FunctionMessage
            {
                /// <summary>
                /// The address to change approval for.
                /// </summary>
                [Parameter("address", "_operator", 1)]
                public string Operator { get; set; }

                /// <summary>
                /// Whether the operator address is allowed to transfer any owner tokens.
                /// </summary>
                [Parameter("bool", "_approved", 3)]
                public bool Approved { get; set; }
            }
        }
    }
}