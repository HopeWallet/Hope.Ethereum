using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Hope.Ethereum.FunctionOuput
{
    /// <summary>
    /// Class which contains simple output return types from solidity functions.
    /// </summary>
    public static partial class SimpleOutputs
    {
        /// <summary>
        /// Class which acts as a bool return type for solidity functions.
        /// </summary>
        [FunctionOutput]
        public sealed class Bool : IFunctionOutputDTO
        {
            /// <summary>
            /// The value of the bool return type.
            /// </summary>
            [Parameter("bool", 1)]
            public bool Value { get; set; }
        }
    }
}