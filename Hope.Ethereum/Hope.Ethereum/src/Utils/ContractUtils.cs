using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.Contracts.QueryHandlers;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.Signer;
using System.Numerics;
using System.Threading.Tasks;
using static Hope.Ethereum.Utils.GasUtils;

namespace Hope.Ethereum.Utils
{
    /// <summary>
    /// Class which contains useful utility methods for sending messages to smart contracts or querying data from them.
    /// </summary>
    public static class ContractUtils
    {
        /// <summary>
        /// Queries some data from an ethereum contract.
        /// </summary>
        /// <typeparam name="TFunc"> The type of the function to send. </typeparam>
        /// <typeparam name="TOut"> The return type of the contract query. </typeparam>
        /// <param name="function"> The function containing the data to query. </param>
        /// <param name="contractAddress"> The address of the contract to query data from. </param>
        /// <param name="senderAddress"> The address of the sender which is querying data. </param>
        /// <returns> Task returning the return type of the query. </returns>
        public static Task<TOut> QueryContract<TFunc, TOut>(TFunc function, string contractAddress, string senderAddress)
            where TFunc : FunctionMessage, new()
            where TOut : IFunctionOutputDTO, new()
        {
            var queryHandler = new QueryToDTOHandler<TFunc, TOut>(NetworkProvider.GetWeb3().Client, senderAddress);

            return queryHandler.QueryAsync(contractAddress, function);
        }

        /// <summary>
        /// Sends a message to an ethereum smart contract with the intent to change a part of the contract on the blockchain.
        /// Uses an estimated standard gas price and estimated gas limit with the transaction.
        /// </summary>
        /// <typeparam name="TFunc"> The type of the function to execute. </typeparam>
        /// <param name="function"> The function to execute. </param>
        /// <param name="privateKey"> The private key of the address executing the contract function. </param>
        /// <param name="contractAddress"> The address of the contract which will process the message. </param>
        /// <returns> Task which returns the TransactionPoller which will await the transaction result. </returns>
        public static async Task<TransactionPoller> SendContractMessage<TFunc>(
            TFunc function,
            string privateKey,
            string contractAddress) where TFunc : FunctionMessage, new()
        {
            return await SendContractMessage(function, privateKey, contractAddress, await EstimateGasPrice(GasPriceTarget.Standard));
        }

        /// <summary>
        /// Sends a message to an ethereum smart contract with the intent to change a part of the contract on the blockchain.
        /// Uses an estimated gas limit with the transaction.
        /// </summary>
        /// <typeparam name="TFunc"> The type of the function to execute. </typeparam>
        /// <param name="function"> The function to execute. </param>
        /// <param name="privateKey"> The private key of the address executing the contract function. </param>
        /// <param name="contractAddress"> The address of the contract which will process the message. </param>
        /// <param name="gasPrice"> The gas price (in wei) to use to send the transaction. </param>
        /// <returns> Task which returns the TransactionPoller which will await the transaction result. </returns>
        public static async Task<TransactionPoller> SendContractMessage<TFunc>(
            TFunc function,
            string privateKey,
            string contractAddress,
            BigInteger gasPrice) where TFunc : FunctionMessage, new()
        {
            return await SendContractMessage(function, privateKey, contractAddress, gasPrice, await EstimateContractGasLimit(function, contractAddress, new EthECKey(privateKey).GetPublicAddress()));
        }

        /// <summary>
        /// Sends a message to an ethereum smart contract with the intent to change a part of the contract on the blockchain.
        /// </summary>
        /// <typeparam name="TFunc"> The type of the function to execute. </typeparam>
        /// <param name="function"> The function to execute. </param>
        /// <param name="privateKey"> The private key of the address executing the contract function. </param>
        /// <param name="contractAddress"> The address of the contract which will process the message. </param>
        /// <param name="gasPrice"> The gas price (in wei) to use to send the transaction. </param>
        /// <param name="gasLimit"> The gas limit (in wei) to use to send the transaction. </param>
        /// <returns> Task which returns the TransactionPoller which will await the transaction result. </returns>
        public static async Task<TransactionPoller> SendContractMessage<TFunc>(
            TFunc function,
            string privateKey,
            string contractAddress,
            BigInteger gasPrice,
            BigInteger gasLimit) where TFunc : FunctionMessage, new()
        {
            return await SendContractMessage(function, privateKey, contractAddress, gasPrice, gasLimit, 0);
        }

        /// <summary>
        /// Sends a message to an ethereum smart contract with the intent to change a part of the contract on the blockchain.
        /// </summary>
        /// <typeparam name="TFunc"> The type of the function to execute. </typeparam>
        /// <param name="function"> The function to execute. </param>
        /// <param name="privateKey"> The private key of the address executing the contract function. </param>
        /// <param name="contractAddress"> The address of the contract which will process the message. </param>
        /// <param name="gasPrice"> The gas price (in wei) to use to send the transaction. </param>
        /// <param name="gasLimit"> The gas limit (in wei) to use to send the transaction. </param>
        /// <param name="value"> The amount of eth (in wei) to send with the transaction. </param>
        /// <returns> Task which returns the TransactionPoller which will await the transaction result. </returns>
        public static async Task<TransactionPoller> SendContractMessage<TFunc>(
            TFunc function,
            string privateKey,
            string contractAddress,
            BigInteger gasPrice,
            BigInteger gasLimit,
            BigInteger value) where TFunc : FunctionMessage, new()
        {
            EthECKey ethECKey = new EthECKey(privateKey);

            InMemoryNonceService nonceService = new InMemoryNonceService(ethECKey.GetPublicAddress(), NetworkProvider.GetWeb3().Client);

            TransactionSigner signer = new TransactionSigner();
            string signedTxData = signer.SignTransaction(
                                            privateKey,
                                            NetworkProvider.GetActiveNetworkChain(),
                                            contractAddress,
                                            value,
                                            (await nonceService.GetNextNonceAsync()).Value,
                                            gasPrice,
                                            gasLimit,
                                            function.CreateTransactionInput(contractAddress).Data);

            EthSendRawTransaction rawTransaction = new EthSendRawTransaction(NetworkProvider.GetWeb3().Client);
            return new TransactionPoller(await rawTransaction.SendRequestAsync(signedTxData));
        }
    }
}
