using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Hope.Ethereum.Tokens
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
            /// Class which contains the data needed to read the balance of a certain address of the ERC20 token contract.
            /// </summary>
            [Function("balanceOf", "uint256")]
            public sealed class BalanceOf : FunctionMessage
            {
                /// <summary>
                /// The owner to check the ERC20 token balance of.
                /// </summary>
                [Parameter("address", "_owner", 1)]
                public string Owner { get; set; }
            }
        }
    }
}