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
            /// Class which contains the data needed to read the symbol of the ERC20 token contract.
            /// </summary>
            [Function("symbol", "string")]
            public sealed class Symbol : FunctionMessage
            {
            }
        }
    }
}