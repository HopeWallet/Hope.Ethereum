using System.Collections.Generic;
using System.Numerics;

namespace Hope.Ethereum.Unity.Utils
{
    /// <summary>
    /// Class which contains some utility methods useful for translating stuff to and from solidity.
    /// </summary>
    public static class SolidityUtils
    {
        public static readonly Dictionary<int, BigInteger> DecimalValueLookup = new Dictionary<int, BigInteger>();

        /// <summary>
        /// Converts a number from its current readable representation to a uint usable in solidity.
        /// </summary>
        /// <param name="number"> The number to convert to solidity uint format. </param>
        /// <param name="decimals"> The number of decimal places the value converting will have. </param>
        /// <returns> The number converted to the <see cref="BigInteger"/> range. </returns>
        public static BigInteger ConvertToUInt(dynamic number, int decimals) => new BigInteger((decimal)number * (decimal)GetBigIntegerValue(decimals));

        /// <summary>
        /// Converts a number from its solidity uint representation to a readable representation in decimal format.
        /// </summary>
        /// <param name="number"> The number to convert to readable format. </param>
        /// <param name="decimals"> The number of decimal places to convert to. </param>
        /// <returns> The number converted from the <see cref="BigInteger"/> range to a readable decimal </returns>
        public static decimal ConvertFromUInt(dynamic number, int decimals) => (decimal)number / (decimal)GetBigIntegerValue(decimals);

        /// <summary>
        /// Gets the BigInteger value corresponding to the number of decimal places needed.
        /// </summary>
        /// <param name="decimals"> The number of decimal places to use in getting the BigInteger conversion value. </param>
        /// <returns> The BigInteger value to use when converting to and from solidity uints. </returns>
        private static BigInteger GetBigIntegerValue(int decimals)
        {
            if (!DecimalValueLookup.ContainsKey(decimals))
                DecimalValueLookup.Add(decimals, BigInteger.Pow(new BigInteger(10), decimals));

            return DecimalValueLookup[decimals];
        }
    }
}
