using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Hope.Ethereum.FunctionOuput
{
    /// <summary>
    /// Class which contains simple output return types from solidity functions.
    /// </summary>
    public static partial class SimpleOutputs
    {
        /// <summary>
        /// Class which acts as an address return type for solidity functions.
        /// </summary>
        [FunctionOutput]
        public sealed class Address : IFunctionOutputDTO
        {
            /// <summary>
            /// The value of the address return type.
            /// </summary>
            [Parameter("address", 1)]
            public string Value { get; set; }
        }
    }
}