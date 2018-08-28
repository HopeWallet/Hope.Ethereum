using Hope.Ethereum.Unity.Promises;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Signer;
using System.Collections;

namespace Hope.Ethereum.Unity.Utils
{
    /// <summary>
    /// Class which contains useful utility methods for sending messages to smart contracts or querying data from them.
    /// </summary>
    public static class ContractUtils
    {
        /// <summary>
        /// Sends a message to an ethereum smart contract with the intent to change a part of the contract on the blockchain.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="FunctionMessage"/> to execute on the blockchain given the contract address. </typeparam>
        /// <param name="function"> The function to execute. </param>
        /// <param name="contractAddress"> The contract address to execute the <see cref="FunctionMessage"/> on. </param>
        /// <param name="privateKey"> The private key of the address sending the transaction. </param>
        /// <param name="signedUnityRequest"> The <see cref="TransactionSignedUnityRequest"/> to use to send the message. </param>
        /// <param name="gasPrice"> The <see cref="HexBigInteger"/> gas price to use with the transaction. </param>
        /// <param name="gasLimit"> The <see cref="HexBigInteger"/> gas limit to use with the transaction. </param>
        /// <returns> Promise of the transaction result of sending the contract message. </returns>
        public static EthTransactionPromise SendContractMessage<TFunc>(
            TFunc function,
            string contractAddress,
            string privateKey,
            HexBigInteger gasPrice,
            HexBigInteger gasLimit) where TFunc : FunctionMessage
        {
            var promise = new EthTransactionPromise();
            _SendContractMessageCoroutine(function, promise, contractAddress, privateKey, gasPrice, gasLimit).StartCoroutine();

            return promise;
        }

        /// <summary>
        /// Coroutine which sends a message to an ethereum smart contract.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="FunctionMessage"/> to execute on the blockchain given the contract address. </typeparam>
        /// <param name="function"> The function to execute. </param>
        /// <param name="promise"> Promise of the transaction result of sending the contract message. </param>
        /// <param name="contractAddress"> The contract address to execute the <see cref="FunctionMessage"/> on. </param>
        /// <param name="privateKey"> The private key of the address sending the transaction. </param>
        /// <param name="signedUnityRequest"> The <see cref="TransactionSignedUnityRequest"/> to use to send the message. </param>
        /// <param name="gasPrice"> The <see cref="HexBigInteger"/> gas price to use with the transaction. </param>
        /// <param name="gasLimit"> The <see cref="HexBigInteger"/> gas limit to use with the transaction. </param>
        private static IEnumerator _SendContractMessageCoroutine<TFunc>(
            TFunc function,
            EthTransactionPromise promise,
            string contractAddress,
            string privateKey,
            HexBigInteger gasPrice,
            HexBigInteger gasLimit) where TFunc : FunctionMessage
        {
            function.SetDefaultFromAddressIfNotSet(privateKey);
            function.Gas = gasLimit;
            function.GasPrice = gasPrice;

            EthECKey ethKey = new EthECKey(privateKey);
            TransactionSignedUnityRequest unityRequest = new TransactionSignedUnityRequest(NetworkProvider.GetNetworkChainUrl(), privateKey, ethKey.GetPublicAddress());

            yield return unityRequest.SignAndSendTransaction(function.CreateTransactionInput(contractAddress));

            promise.Build(unityRequest, () => unityRequest.Result, NetworkProvider.GetNetworkChainUrl);
        }

        /// <summary>
        /// Queries some data from an ethereum smart contract which is active on the blockchain.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="FunctionMessage"/> of the smart contract to execute which will return us some data. </typeparam>
        /// <typeparam name="TOut"> The <see cref="IFunctionOutputDTO"/> which represents the data which was returned from the <see cref="ContractFunction"/>. </typeparam>
        /// <param name="function"> The contract function to query data from. </param>
        /// <param name="contractAddress"> The contract address to execute the <see cref="FunctionMessage"/> on. </param>
        /// <param name="senderAddress"> The address of the sender requesting this data. </param>
        /// <returns> The promise which will return the call result. </returns>
        public static EthCallPromise<TOut> QueryContract<TFunc, TOut>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new() where TOut : IFunctionOutputDTO, new()
        {
            var promise = new EthCallPromise<TOut>();
            _QueryContractCoroutine(function, promise, contractAddress, senderAddress).StartCoroutine();

            return promise;
        }

        /// <summary>
        /// Coroutine which queries some data from an ethereum smart contract.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="FunctionMessage"/> of the smart contract to execute which will return us some data. </typeparam>
        /// <typeparam name="TOut"> The <see cref="IFunctionOutputDTO"/> which represents the data which was returned from the <see cref="ContractFunction"/>. </typeparam>
        /// <param name="function"> The contract function to query data from. </param>
        /// <param name="promise"> Promise of eventually returning the data from the contract query. </param>
        /// <param name="contractAddress"> The contract address to execute the <see cref="FunctionMessage"/> on. </param>
        /// <param name="senderAddress"> The address of the sender requesting this data. </param>
        private static IEnumerator _QueryContractCoroutine<TFunc, TOut>(
            TFunc function,
            EthCallPromise<TOut> promise,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new() where TOut : IFunctionOutputDTO, new()
        {
            function.SetDefaultFromAddressIfNotSet(senderAddress);

            var queryRequest = new QueryUnityRequest<TFunc, TOut>(NetworkProvider.GetNetworkChainUrl(), senderAddress);
            yield return queryRequest.Query(function, contractAddress);

            promise.Build(queryRequest, () => queryRequest.Result);
        }
    }
}