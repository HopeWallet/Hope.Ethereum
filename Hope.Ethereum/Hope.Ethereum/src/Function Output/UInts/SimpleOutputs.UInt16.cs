using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Hope.Ethereum.FunctionOuput
{
    /// <summary>
    /// Class which contains simple output return types from solidity functions.
    /// </summary>
    public static partial class SimpleOutputs
    {
        /// <summary>
        /// Class which acts as a uint16 return type for solidity functions.
        /// </summary>
        [FunctionOutput]
        public sealed class UInt16 : UIntBase
        {
            /// <summary>
            /// The value of the uint16 return type.
            /// </summary>
            [Parameter("uint16", 1)]
            public override dynamic Value { get; set; }
        }
    }
}