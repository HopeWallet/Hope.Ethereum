using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Hope.Ethereum.Unity.Tokens
{
    /// <summary>
    /// Class which mimics the ethereum ERC20 token standard and contains most functions which are active in the token standard.
    /// </summary>
    public sealed partial class ERC20
    {
        /// <summary>
        /// Class which contains the different queries for receiving data from the ERC20 token contract.
        /// </summary>
        public static partial class Queries
        {
            /// <summary>
            /// Class which contains the data needed to read the allowance of a certain address given to a spender.
            /// </summary>
            [Function("allowance", "uint256")]
            public sealed class Allowance : FunctionMessage
            {
                /// <summary>
                /// The owner of the tokens which have been given allowance.
                /// </summary>
                [Parameter("address", "_owner", 1)]
                public string Owner { get; set; }

                /// <summary>
                /// The address given allowance to.
                /// </summary>
                [Parameter("address", "_spender", 1)]
                public string Spender { get; set; }
            }
        }
    }
}