using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Hope.Ethereum
{
    /// <summary>
    /// Class which contains simple output return types from solidity functions.
    /// </summary>
    public static partial class SimpleOutputs
    {
        /// <summary>
        /// Class which acts as a uint64 return type for solidity functions.
        /// </summary>
        [FunctionOutput]
        public sealed class UInt64 : UIntBase
        {
            /// <summary>
            /// The value of the uint64 return type.
            /// </summary>
            [Parameter("uint64", 1)]
            public override dynamic Value { get; set; }
        }
    }
}