using System.Collections.Generic;
using System.Numerics;

namespace NethereumUtils.Unity
{
    /// <summary>
    /// Class which contains some utility methods useful for translating stuff to and from solidity.
    /// </summary>
    public static class SolidityUtils
    {
        public static readonly Dictionary<int, BigInteger> DecimalValueLookup = new Dictionary<int, BigInteger>();

        /// <summary>
        /// Extracts the function parameters from the given input string.
        /// </summary>
        /// <param name="inputData"> The input of the contract function. </param>
        /// <returns> The compiled array of strings which contain the data of each parameter. The name of the function is in the first index, parameters are in the rest. </returns>
        public static string[] ExtractFunctionParameters(string inputData)
        {
            string[] funcParams = new string[1 + (inputData.Length / 64)];

            for (int i = inputData.Length; i > 0; i -= 64)
            {
                var start = i - 64;
                var length = start < 0 ? inputData.Length % 64 : 64;

                if (start < 0)
                    start = 0;

                var param = inputData.Substring(start, length);
                var index = funcParams.Length - ((inputData.Length - i) / 64) - 1;

                funcParams[index] = index == 0 ? param : param.TrimStart('0');
            }

            return funcParams;
        }

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
