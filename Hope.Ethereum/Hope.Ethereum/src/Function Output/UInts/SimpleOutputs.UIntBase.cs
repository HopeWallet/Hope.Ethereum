using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Hope.Ethereum.FunctionOuput
{
    /// <summary>
    /// Class which contains simple output return types from solidity functions.
    /// </summary>
    public static partial class SimpleOutputs
    {
        /// <summary>
        /// Class which acts as a base class for all uint return types.
        /// </summary>
        public abstract class UIntBase : IFunctionOutputDTO
        {
            /// <summary>
            /// The value of the uint return type.
            /// </summary>
            public abstract dynamic Value { get; set; }
        }
    }
}