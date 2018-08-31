using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace Hope.Ethereum.Tokens
{
    /// <summary>
    /// Class which mimics the ethereum ERC20 token standard and contains most functions which are active in the token standard.
    /// </summary>
    public partial class ERC20
    {
        /// <summary>
        /// Class which contains the different messages which interact/change the ERC20 token contract values on the blockchain.
        /// </summary>
        public static partial class Messages
        {
            /// <summary>
            /// Class which contains the data needed to execute the ERC20 approve function.
            /// </summary>
            [Function("approve", "bool")]
            public sealed class Approve : FunctionMessage
            {
                /// <summary>
                /// The address to give transfer approval to.
                /// </summary>
                [Parameter("address", "_spender", 1)]
                public string Spender { get; set; }

                /// <summary>
                /// The amount of the ERC20 token to give approval to the address.
                /// </summary>
                [Parameter("uint256", "_value", 2)]
                public BigInteger Value { get; set; }
            }
        }
    }
}