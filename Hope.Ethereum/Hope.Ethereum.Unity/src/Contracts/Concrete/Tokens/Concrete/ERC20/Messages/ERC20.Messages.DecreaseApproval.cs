using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace Hope.Ethereum.Tokens
{
    /// <summary>
    /// Class which mimics the ethereum ERC20 token standard and contains most functions which are active in the token standard.
    /// </summary>
    public sealed partial class ERC20
    {
        /// <summary>
        /// Class which contains the different messages which interact/change the ERC20 token contract values on the blockchain.
        /// </summary>
        public static partial class Messages
        {
            /// <summary>
            /// Class which contains the data needed to execute the ERC20 decreaseApproval function.
            /// </summary>
            [Function("decreaseApproval", "bool")]
            public sealed class DecreaseApproval : FunctionMessage
            {
                /// <summary>
                /// The address to remove transfer approval from.
                /// </summary>
                [Parameter("address", "_spender", 1)]
                public string Spender { get; set; }

                /// <summary>
                /// The amount to remove from the allowance of the spender address.
                /// </summary>
                [Parameter("uint256", "_subtractedValue", 2)]
                public BigInteger SubtractedValue { get; set; }
            }
        }
    }
}