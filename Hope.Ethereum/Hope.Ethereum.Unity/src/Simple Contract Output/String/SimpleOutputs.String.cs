using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Hope.Ethereum
{
    /// <summary>
    /// Class which contains simple output return types from solidity functions.
    /// </summary>
    public static partial class SimpleOutputs
    {
        /// <summary>
        /// Class which acts as a string return type for solidity functions.
        /// </summary>
        [FunctionOutput]
        public sealed class String : IFunctionOutputDTO
        {
            /// <summary>
            /// The value of the string return type.
            /// </summary>
            [Parameter("string", 1)]
            public string Value { get; set; }
        }
    }
}