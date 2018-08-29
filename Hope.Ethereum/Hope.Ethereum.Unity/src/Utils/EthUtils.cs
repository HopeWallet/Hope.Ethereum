using Hope.Ethereum.Unity.Promises;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using System.Collections;
using System.Numerics;

namespace Hope.Ethereum.Unity.Utils
{
    /// <summary>
    /// Utility class used for sending ether and retrieving the ether balance of certain addresses.
    /// </summary>
    public static class EthUtils
    {
        /// <summary>
        /// Gets the amount of ether in a user's wallet.
        /// </summary>
        /// <param name="address"> The address to check for the ether balance. </param>
        /// <param name="onBalanceReceived"> Called when the eth balance has been received. </param>
        /// <returns> The promise which will return the eth balance. </returns>
        public static EthCallPromise<dynamic> GetEtherBalance(string address)
        {
            var promise = new EthCallPromise<dynamic>();
            _AddressEthBalanceCoroutine(promise, address).StartCoroutine();

            return promise;
        }

        /// <summary>
        /// Gets the ether balance of a certain wallet.
        /// </summary>
        /// <param name="promise"> Promise of an eventual eth balance returned. </param>
        /// <param name="address"> The address to check the ether balance for. </param>
        /// <returns> The time waited for the request to complete. </returns>
        private static IEnumerator _AddressEthBalanceCoroutine(EthCallPromise<dynamic> promise, string address)
        {
            var request = new EthGetBalanceUnityRequest(NetworkProvider.GetNetworkChainUrl());
            yield return request.SendRequest(address, BlockParameter.CreateLatest());

            promise.Build(request, () => SolidityUtils.ConvertFromUInt(request.Result.Value, 18));
        }

        /// <summary>
        /// Sends ether from this wallet to a given address.
        /// </summary>
        /// <param name="privateKey"> The private key of the address sending the transaction. </param>
        /// <param name="addressTo"> The address to send the ether to. </param>
        /// <param name="amount"> The amount of ether to send. </param>
        /// <returns> The promise which will contain the result of a successful/unsuccessful transaction. </returns>
        public static EthTransactionPromise SendEther(
            string privateKey,
            string addressTo,
            decimal amount)
        {
            var promise = new EthTransactionPromise();
            GasUtils.EstimateEthGasLimit(addressTo, SolidityUtils.ConvertToUInt(amount, 18)).OnSuccess(gasLimit
                => GasUtils.EstimateGasPrice(GasUtils.GasPriceTarget.Standard).OnSuccess(gasPrice
                    => _SendEtherCoroutine(promise, gasLimit, gasPrice, privateKey, addressTo, amount).StartCoroutine()));

            return promise;
        }

        /// <summary>
        /// Sends ether from this wallet to a given address.
        /// </summary>
        /// <param name="privateKey"> The private key of the address sending the transaction. </param>
        /// <param name="addressTo"> The address to send the ether to. </param>
        /// <param name="amount"> The amount of ether to send. </param>
        /// <param name="gasPrice"> The gas price of the ether send transaction. </param>
        /// <returns> The promise which will contain the result of a successful/unsuccessful transaction. </returns>
        public static EthTransactionPromise SendEther(
            string privateKey,
            string addressTo,
            decimal amount,
            BigInteger gasPrice)
        {
            var promise = new EthTransactionPromise();
            GasUtils.EstimateEthGasLimit(addressTo, SolidityUtils.ConvertToUInt(amount, 18))
                    .OnSuccess(gasLimit => _SendEtherCoroutine(promise, gasLimit, gasPrice, privateKey, addressTo, amount).StartCoroutine());

            return promise;
        }

        /// <summary>
        /// Sends ether from this wallet to a given address.
        /// </summary>
        /// <param name="privateKey"> The private key of the address sending the transaction. </param>
        /// <param name="addressTo"> The address to send the ether to. </param>
        /// <param name="amount"> The amount of ether to send. </param>
        /// <param name="gasPrice"> The gas price of the ether send transaction. </param>
        /// <param name="gasLimit"> The gas limit of the ether send transaction. </param>
        /// <returns> The promise which will contain the result of a successful/unsuccessful transaction. </returns>
        public static EthTransactionPromise SendEther(
            string privateKey,
            string addressTo,
            decimal amount,
            BigInteger gasPrice,
            BigInteger gasLimit)
        {
            var promise = new EthTransactionPromise();
            _SendEtherCoroutine(promise, gasLimit, gasPrice, privateKey, addressTo, amount).StartCoroutine();

            return promise;
        }

        /// <summary>
        /// Sends ether from one address to another.
        /// </summary>
        /// <param name="promise"> Promise of the transaction result of sending ether. </param>
        /// <param name="walletAddress"> The address of the wallet sending the ether. </param>
        /// <param name="gasLimit"> The gas limit of the ether send transaction. </param>
        /// <param name="gasPrice"> The gas price of the ether send transaction. </param>
        /// <param name="privateKey"> The private key of the address sending the transaction. </param>
        /// <param name="addressTo"> The address the ether is being sent to. </param>
        /// <param name="amount"> The amount to send in ether. </param>
        /// <returns> The time waited for the request to be broadcast to the network. </returns>
        private static IEnumerator _SendEtherCoroutine(
            EthTransactionPromise promise,
            BigInteger gasLimit,
            BigInteger gasPrice,
            string privateKey,
            string addressTo,
            dynamic amount)
        {
            EthECKey ethKey = new EthECKey(privateKey);
            TransactionSignedUnityRequest unityRequest = new TransactionSignedUnityRequest(NetworkProvider.GetNetworkChainUrl(), privateKey, ethKey.GetPublicAddress());
            TransactionInput transactionInput = new TransactionInput("", addressTo, ethKey.GetPublicAddress(), new HexBigInteger(gasLimit), new HexBigInteger(gasPrice), new HexBigInteger(SolidityUtils.ConvertToUInt(amount, 18)));
            yield return unityRequest.SignAndSendTransaction(transactionInput);

            promise.Build(unityRequest, () => unityRequest.Result, () => NetworkProvider.GetNetworkChainUrl());
        }
    }
}
