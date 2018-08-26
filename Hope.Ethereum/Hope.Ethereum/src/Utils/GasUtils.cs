using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Util;
using System.Numerics;
using System.Threading.Tasks;

namespace NethereumUtils.Standard
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
        /// Estimates the gas limit of sending ether from one address to another.
        /// </summary>
        /// <param name="addressTo"> The address the ether is being sent to. </param>
        /// <param name="value"> The amount of ether being sent (in wei). </param>
        /// <returns> Task which returns the gas limit of sending ether. </returns>
        public static async Task<BigInteger> EstimateEthGasLimit(string addressTo, BigInteger value)
        {
            CallInput callInput = new CallInput("", addressTo, new HexBigInteger(value));
            EthEstimateGas estimateGasLimit = new EthEstimateGas(NetworkProvider.GetWeb3().Client);
            return await estimateGasLimit.SendRequestAsync(callInput);
        }

        /// <summary>
        /// Estimates the gas limit of calling a function of an ethereum contract.
        /// </summary>
        /// <typeparam name="TFunc"> The type of the function to call. </typeparam>
        /// <param name="function"> The concrete FunctionMessage to call. </param>
        /// <param name="contractAddress"> The contract address which will be used to call the function. </param>
        /// <param name="callerAddress"> The address calling the contract function. </param>
        /// <returns> Task which returns the gas limit of calling a function of an ethereum contract. </returns>
        public static async Task<BigInteger> EstimateContractGasLimit<TFunc>(
            TFunc function,
            string contractAddress,
            string callerAddress) where TFunc : FunctionMessage, new()
        {
            function.SetDefaultFromAddressIfNotSet(callerAddress);

            EthEstimateGas estimateGasLimit = new EthEstimateGas(NetworkProvider.GetWeb3().Client);
            return ((await estimateGasLimit.SendRequestAsync(function.CreateCallInput(contractAddress))).Value * 100) / 90;
        }

        /// <summary>
        /// Estimates the gas price of an ethereum transaction.
        /// </summary>
        /// <param name="gasPriceTarget"> The target gas price. </param>
        /// <returns> Task which returns the estimated gas price of an ethereum transaction. </returns>
        public static async Task<BigInteger> EstimateGasPrice(GasPriceTarget gasPriceTarget = GasPriceTarget.Standard)
        {
            EthGasPrice estimateGasPrice = new EthGasPrice(NetworkProvider.GetWeb3().Client);
            return ModifyGasPrice(gasPriceTarget, (await estimateGasPrice.SendRequestAsync()).Value);
        }

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
