using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.Signer;
using System.Numerics;
using System.Threading.Tasks;

namespace NethereumUtils.Standard
{
    /// <summary>
    /// Utility class used for sending ether and retrieving the ether balance of certain addresses.
    /// </summary>
    public static class EthUtils
    {
        /// <summary>
        /// Gets the ether balance of an address.
        /// </summary>
        /// <param name="address"> The address to check for the ether balance. </param>
        /// <returns> Task which returns the decimal ether balance of the address. </returns>
        public static async Task<decimal> GetEtherBalance(string address)
        {
            EthGetBalance ethGetBalance = new EthGetBalance(NetworkProvider.GetWeb3().Client);
            return SolidityUtils.ConvertFromUInt((await ethGetBalance.SendRequestAsync(address)).Value, 18);
        }

        /// <summary>
        /// Sends a given amount of ether to an address.
        /// Uses an estimated gas price and gas limit for the transaction.
        /// </summary>
        /// <param name="privateKey"> The private key of the address which is sending the ether. </param>
        /// <param name="addressTo"> The address the ether is being sent to. </param>
        /// <param name="amount"> The amount of ether being sent, in eth. (not wei) </param>
        /// <returns> Task which returns the TransactionPoller which will await the transaction result. </returns>
        public static async Task<TransactionPoller> SendEther(string privateKey, string addressTo, decimal amount)
        {
            return await SendEther(privateKey, addressTo, amount, await GasUtils.EstimateGasPrice(GasUtils.GasPriceTarget.Standard));
        }

        /// <summary>
        /// Sends a given amount of ether to an address.
        /// Uses an estimated gas limit for the transaction.
        /// </summary>
        /// <param name="privateKey"> The private key of the address which is sending the ether. </param>
        /// <param name="addressTo"> The address the ether is being sent to. </param>
        /// <param name="amount"> The amount of ether being sent, in eth. (not wei) </param>
        /// <param name="gasPrice"> The gas price (in wei) to use to send the transaction. </param>
        /// <returns> Task which returns the TransactionPoller which will await the transaction result. </returns>
        public static async Task<TransactionPoller> SendEther(string privateKey, string addressTo, decimal amount, BigInteger gasPrice)
        {
            return await SendEther(privateKey, addressTo, amount, gasPrice, await GasUtils.EstimateEthGasLimit(addressTo, SolidityUtils.ConvertToUInt(amount, 18)));
        }

        /// <summary>
        /// Sends a given amount of ether to an address.
        /// </summary>
        /// <param name="privateKey"> The private key of the address which is sending the ether. </param>
        /// <param name="addressTo"> The address the ether is being sent to. </param>
        /// <param name="amount"> The amount of ether being sent, in eth. (not wei) </param>
        /// <param name="gasPrice"> The gas price (in wei) to use to send the transaction. </param>
        /// <param name="gasLimit"> The gas limit (in wei) to use to send the transaction. </param>
        /// <returns> Task which returns the TransactionPoller which will await the transaction result. </returns>
        public static async Task<TransactionPoller> SendEther(string privateKey, string addressTo, decimal amount, BigInteger gasPrice, BigInteger gasLimit)
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
