using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Extensions;
using Nethereum.Contracts.QueryHandlers;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.Signer;
using NethereumUtils.Standard.Gas;
using NethereumUtils.Standard.Network;
using System.Numerics;
using System.Threading.Tasks;
using static NethereumUtils.Standard.Gas.GasUtils;

namespace NethereumUtils.Standard.Contract
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
            await SendContractMessage(function, privateKey, contractAddress, SolidityUtils.ConvertFromUInt(await EstimateGasPrice(gasPriceTarget), 18));
        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress, decimal gasPrice)
            where TFunc : FunctionMessage, new()
        {

        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress, decimal gasPrice, BigInteger gasLimit)
            where TFunc : FunctionMessage, new()
        {

        }

        public static async Task SendContractMessage<TFunc>(TFunc function, string privateKey, string contractAddress, decimal gasPrice, BigInteger gasLimit, BigInteger value)
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
                                            nonceService.CurrentNonce,
                                            GasUtils.GetFunctionalGasPrice(gasPrice),
                                            gasLimit,
                                            function.CreateTransactionInput(contractAddress).Data);

            EthSendRawTransaction rawTransaction = new EthSendRawTransaction(NetworkUtils.GetWeb3().Client);
            await rawTransaction.SendRequestAsync(signedTxData);
            //rawTransaction.

            //signer.SignTransaction()

            //var contractHandler = NetworkUtils.GetWeb3().Eth.GetContractHandler(contractAddress);
            //contractHandler.SignTransactionAsync(function);
            //var transactionHandler = NetworkUtils.GetWeb3().Eth.GetContractTransactionHandler<TFunc>();
            //transactionHandler.SignTransactionAsync()
            //var transactionInput = function.CreateTransactionInput(contractAddress);

            //signer.SignTransaction("0x", NetworkUtils.GetActiveNetwork(), )
            //signer.SignTransaction
            //NetworkUtils.GetWeb3().TransactionManager.

        }
    }
}
