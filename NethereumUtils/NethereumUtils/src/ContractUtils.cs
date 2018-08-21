using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.Contracts.QueryHandlers;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.Signer;
using System.Numerics;
using System.Threading.Tasks;
using static NethereumUtils.Standard.GasUtils;

namespace NethereumUtils.Standard
{
    /// <summary>
    /// Class which contains useful utility methods for sending messages to smart contracts or querying data from them.
    /// </summary>
    public static class ContractUtils
    {
        public static Task<TOut> QueryContract<TFunc, TOut>(TFunc function, string contractAddress, string senderAddress)
            where TFunc : FunctionMessage, new()
            where TOut : IFunctionOutputDTO, new()
        {
            var queryHandler = new QueryToDTOHandler<TFunc, TOut>(NetworkUtils.GetWeb3().Client, senderAddress);

            return queryHandler.QueryAsync(contractAddress, function);
        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress)
            where TFunc : FunctionMessage, new()
        {
            await SendContractMessage(function, privateKey, contractAddress, GasPriceTarget.Standard);
        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress, GasPriceTarget gasPriceTarget)
            where TFunc : FunctionMessage, new()
        {
            await SendContractMessage(function, privateKey, contractAddress, await EstimateGasPrice(gasPriceTarget));
        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress, BigInteger gasPrice)
            where TFunc : FunctionMessage, new()
        {
            await SendContractMessage(function, privateKey, contractAddress, gasPrice, await EstimateContractGasLimit(function, contractAddress, new EthECKey(privateKey).GetPublicAddress()));
        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress, BigInteger gasPrice, BigInteger gasLimit)
            where TFunc : FunctionMessage, new()
        {
            await SendContractMessage(function, privateKey, contractAddress, gasPrice, gasLimit, 0);
        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress, BigInteger gasPrice, BigInteger gasLimit, BigInteger value)
            where TFunc : FunctionMessage, new()
        {
            EthECKey ethECKey = new EthECKey(privateKey);

            InMemoryNonceService nonceService = new InMemoryNonceService(ethECKey.GetPublicAddress(), NetworkUtils.GetWeb3().Client);

            TransactionSigner signer = new TransactionSigner();
            string signedTxData = signer.SignTransaction(
                                            privateKey,
                                            NetworkUtils.GetActiveNetwork(),
                                            contractAddress,
                                            value,
                                            (await nonceService.GetNextNonceAsync()).Value,
                                            gasPrice,
                                            gasLimit,
                                            function.CreateTransactionInput(contractAddress).Data);

            EthSendRawTransaction rawTransaction = new EthSendRawTransaction(NetworkUtils.GetWeb3().Client);
            await rawTransaction.SendRequestAsync(signedTxData);
        }
    }
}
