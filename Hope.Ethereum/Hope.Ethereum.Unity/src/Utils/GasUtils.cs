using Hope.Ethereum.Unity.Promises;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using System.Collections;
using System.Numerics;

namespace Hope.Ethereum.Unity.Utils
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
        /// Estimates the gas limit of a contract function.
        /// </summary>
        /// <typeparam name="TFunc"> The function type to estimate the gas limit for. </typeparam>
        /// <param name="function"> The function to estimate the the gas limit for. </param>
        /// <param name="contractAddress"> The address of the contract function to estimate. </param>
        /// <param name="callerAddress"> The address of the sender. </param>
        public static EthCallPromise<BigInteger> EstimateContractGasLimit<TFunc>(
            TFunc function,
            string contractAddress,
            string callerAddress) where TFunc : FunctionMessage
        {
            function.SetDefaultFromAddressIfNotSet(callerAddress);

            var promise = new EthCallPromise<BigInteger>();
            _EstimateGasLimitCoroutine(promise, function.CreateCallInput(contractAddress), true).StartCoroutine();

            return promise;
        }

        /// <summary>
        /// Estimates the gas limit for a basic ether transaction.
        /// </summary>
        /// <param name="addressTo"> The address the ether is being sent to. </param>
        /// <param name="value"> The amount of ether in wei that will be sent. </param>
        /// <returns> Promise of the gas limit estimate of an eth transaction. </returns>
        public static EthCallPromise<BigInteger> EstimateEthGasLimit(string addressTo, BigInteger value)
        {
            var promise = new EthCallPromise<BigInteger>();
            _EstimateGasLimitCoroutine(promise, new CallInput("", addressTo, new HexBigInteger(value)), false).StartCoroutine();

            return promise;
        }

        /// <summary>
        /// Estimates the gas price given the GasPriceTarget.
        /// </summary>
        /// <param name="gasPriceTarget">  The target gas price to aim for. </param>
        /// <returns> Promise of the eventual estimated gas price. </returns>
        public static EthCallPromise<BigInteger> EstimateGasPrice(GasPriceTarget gasPriceTarget = GasPriceTarget.Standard)
        {
            var promise = new EthCallPromise<BigInteger>();
            _EstimateGasPriceCoroutine(promise, gasPriceTarget).StartCoroutine();

            return promise;
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

        /// <summary>
        /// Estimates the gas limit of a certain function of a contract.
        /// </summary>
        /// <param name="promise"> Promise of the estimated gas limit of a transaction. </param>
        /// <param name="callInput"> The transaction input to estimate the gas limit for. </param>
        /// <param name="overEstimate"> Whether the gas limit should be slightly overestimated. </param>
        /// <returns> The time taken to retrieve the estimated gas limit. </returns>
        private static IEnumerator _EstimateGasLimitCoroutine(EthCallPromise<BigInteger> promise, CallInput callInput, bool overEstimate)
        {
            var request = new EthEstimateGasUnityRequest(NetworkProvider.GetNetworkChainUrl());
            yield return request.SendRequest(callInput);

            promise.Build(request, () => overEstimate ? (request.Result.Value * 100 / 90) : request.Result.Value);
        }

        /// <summary>
        /// Estimates the gas price based on current network congestion.
        /// </summary>
        /// <param name="promise"> Promise of the eventual gas price estimate. </param>
        /// <param name="gasPriceTarget"> The GasPriceTarget to aim for. </param>
        /// <returns> The time taken to retrieve the estimated gas limit. </returns>
        private static IEnumerator _EstimateGasPriceCoroutine(EthCallPromise<BigInteger> promise, GasPriceTarget gasPriceTarget)
        {
            var request = new EthGasPriceUnityRequest(NetworkProvider.GetNetworkChainUrl());
            yield return request.SendRequest();

            promise.Build(request, () => ModifyGasPrice(gasPriceTarget, request.Result.Value));
        }
    }
}
