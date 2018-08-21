using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.Signer;
using System.Numerics;
using System.Threading.Tasks;

namespace NethereumUtils.Standard
{
    public static class EthUtils
    {
        public static async Task<decimal> GetEthBalance(string address)
        {
            EthGetBalance ethGetBalance = new EthGetBalance(NetworkProvider.GetWeb3().Client);
            return SolidityUtils.ConvertFromUInt((await ethGetBalance.SendRequestAsync(address)).Value, 18);
        }

        public static async Task<TransactionPoller> SendEth(string privateKey, string addressTo, decimal amount)
        {
            return await SendEth(privateKey, addressTo, amount, await GasUtils.EstimateGasPrice(GasUtils.GasPriceTarget.Standard));
        }

        public static async Task<TransactionPoller> SendEth(string privateKey, string addressTo, decimal amount, BigInteger gasPrice)
        {
            return await SendEth(privateKey, addressTo, amount, gasPrice, await GasUtils.EstimateEthGasLimit(addressTo, SolidityUtils.ConvertToUInt(amount, 18)));
        }

        public static async Task<TransactionPoller> SendEth(string privateKey, string addressTo, decimal amount, BigInteger gasPrice, BigInteger gasLimit)
        {
            BigInteger value = SolidityUtils.ConvertToUInt(amount, 18);

            EthECKey ethECKey = new EthECKey(privateKey);

            InMemoryNonceService nonceService = new InMemoryNonceService(ethECKey.GetPublicAddress(), NetworkProvider.GetWeb3().Client);

            TransactionSigner signer = new TransactionSigner();
            string signedTxData = signer.SignTransaction(
                                            privateKey,
                                            NetworkProvider.GetActiveNetworkChain(),
                                            addressTo,
                                            value,
                                            (await nonceService.GetNextNonceAsync()).Value,
                                            gasPrice,
                                            gasLimit,
                                            string.Empty);

            EthSendRawTransaction rawTransaction = new EthSendRawTransaction(NetworkProvider.GetWeb3().Client);
            return new TransactionPoller(await rawTransaction.SendRequestAsync(signedTxData));
        }
    }
}
