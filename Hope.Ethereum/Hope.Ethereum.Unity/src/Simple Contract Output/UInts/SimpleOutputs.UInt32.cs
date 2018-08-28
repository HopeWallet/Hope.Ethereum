using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Hope.Ethereum.Unity.FunctionOutput
{
    /// <summary>
    /// Class which contains simple output return types from solidity functions.
    /// </summary>
    public static partial class SimpleOutputs
    {
        /// <summary>
        /// Class which acts as a uint32 return type for solidity functions.
        /// </summary>
        [FunctionOutput]
        public sealed class UInt32 : UIntBase
        {
            /// <summary>
            /// The value of the uint32 return type.
            /// </summary>
            [Parameter("uint32", 1)]
            public override dynamic Value { get; set; }
        }
    }
}