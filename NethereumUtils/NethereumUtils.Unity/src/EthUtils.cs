﻿using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using NethereumUtils.Unity.Coroutines;
using NethereumUtils.Unity.Promises;
using System.Collections;

namespace NethereumUtils.Unity
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
        /// <param name="signedUnityRequest"> The signed request to send the ether with. </param>
        /// <param name="walletAddress"> The address of the wallet sending the ether. </param>
        /// <param name="gasLimit"> The gas limit of the ether send transaction. </param>
        /// <param name="gasPrice"> The gas price of the ether send transaction. </param>
        /// <param name="addressTo"> The address to send the ether to. </param>
        /// <param name="amount"> The amount of ether to send. </param>
        /// <returns> The promise which will contain the result of a successful/unsuccessful transaction. </returns>
        public static EthTransactionPromise SendEther(
            TransactionSignedUnityRequest signedUnityRequest,
            HexBigInteger gasLimit,
            HexBigInteger gasPrice,
            string walletAddress,
            string addressTo,
            decimal amount)
        {
            var promise = new EthTransactionPromise();
            _SendEtherCoroutine(promise, signedUnityRequest, gasLimit, gasPrice, walletAddress, addressTo, amount).StartCoroutine();

            return promise;
        }

        /// <summary>
        /// Sends ether from one address to another.
        /// </summary>
        /// <param name="promise"> Promise of the transaction result of sending ether. </param>
        /// <param name="signedUnityRequest"> The signed request to send the ether with. </param>
        /// <param name="walletAddress"> The address of the wallet sending the ether. </param>
        /// <param name="gasLimit"> The gas limit of the ether send transaction. </param>
        /// <param name="gasPrice"> The gas price of the ether send transaction. </param>
        /// <param name="addressTo"> The address to send the ether to. </param>
        /// <param name="amount"> The amount to send in ether. </param>
        /// <returns> The time waited for the request to be broadcast to the network. </returns>
        private static IEnumerator _SendEtherCoroutine(
            EthTransactionPromise promise,
            TransactionSignedUnityRequest signedUnityRequest,
            HexBigInteger gasLimit,
            HexBigInteger gasPrice,
            string walletAddress,
            string addressTo,
            dynamic amount)
        {
            var transactionInput = new TransactionInput("", addressTo, walletAddress, gasLimit, gasPrice, new HexBigInteger(SolidityUtils.ConvertToUInt(amount, 18)));
            yield return signedUnityRequest.SignAndSendTransaction(transactionInput);

            promise.Build(signedUnityRequest, () => signedUnityRequest.Result, () => NetworkProvider.GetNetworkChainUrl());
        }
    }
}
