using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Util;
using NethereumUtils.Standard.Network;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace NethereumUtils.Standard.Gas
{
    /// <summary>
    /// Class used for anything related to ethereum transaction gas.
    /// </summary>
    public static class GasUtils
    {
        /// <summary>
        /// Enum for the type of gas price to aim for.
        /// </summary>
        public enum GasPriceTarget { Slow, Standard, Fast };

        private const int ETH_GAS_LIMIT = 21000;

        public static async Task<BigInteger> EstimateGasLimit<TFunc>(
            TFunc function,
            string contractAddress,
            string callerAddress) where TFunc : FunctionMessage, new()
        {
            function.SetDefaultFromAddressIfNotSet(callerAddress);

            EthEstimateGas estimateGasLimit = new EthEstimateGas(NetworkUtils.GetWeb3().Client);
            return ((await estimateGasLimit.SendRequestAsync(function.CreateCallInput(contractAddress))).Value * 100) / 90;
        }

        public static void EstimateGasLimit(Action<BigInteger> onGasReceived)
        {
            onGasReceived(new BigInteger(ETH_GAS_LIMIT));
        }

        public static async Task<BigInteger> EstimateGasPrice(GasPriceTarget gasPriceTarget)
        {
            EthGasPrice estimateGasPrice = new EthGasPrice(NetworkUtils.GetWeb3().Client);
            return ModifyGasPrice(gasPriceTarget, (await estimateGasPrice.SendRequestAsync()).Value);
        }

        /// <summary>
        /// Gets the readable gas price from the regular gwei form of the gas price.
        /// </summary>
        /// <param name="gasPrice"> The gas price to convert. </param>
        /// <returns> The readable gas price converted from gwei to wei.  </returns>
        public static decimal GetReadableGasPrice(BigInteger gasPrice) => UnitConversion.Convert.FromWei(gasPrice, UnitConversion.EthUnit.Gwei);

        /// <summary>
        /// Gets the functional gas price used for transaction input.
        /// </summary>
        /// <param name="gasPrice"> The gas price in wei to convert to the form used in transaction input. </param>
        /// <returns> The functional gas price. </returns>
        public static BigInteger GetFunctionalGasPrice(decimal gasPrice) => UnitConversion.Convert.ToWei(gasPrice, UnitConversion.EthUnit.Gwei);

        /// <summary>
        /// Calculates the maximum gas cost of a transaction given the gas price and the gas limit.
        /// </summary>
        /// <param name="gasPrice"> The functional gas price. </param>
        /// <param name="gasLimit"> The gas limit. </param>
        /// <returns> The maximum gas cost of in ether. </returns>
        public static decimal CalculateMaximumGasCost(BigInteger gasPrice, BigInteger gasLimit) => UnitConversion.Convert.FromWei(gasPrice * gasLimit);

        /// <summary>
        /// Modifies the current gas price to reflect the GasPriceTarget.
        /// </summary>
        /// <param name="priceTarget"> The target to modify the gas price towards. </param>
        /// <param name="currentPrice"> The current functional gas price to modify. </param>
        /// <returns> The new functional gas price after modification. </returns>
        private static BigInteger ModifyGasPrice(GasPriceTarget priceTarget, BigInteger currentPrice)
        {
            switch (priceTarget)
            {
                case GasPriceTarget.Slow:
                    return (currentPrice * 2) / 3;
                case GasPriceTarget.Fast:
                    return currentPrice * 2;
                default:
                    return currentPrice;
            }
        }
    }
}
