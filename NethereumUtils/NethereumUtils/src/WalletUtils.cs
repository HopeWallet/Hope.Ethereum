using Nethereum.HdWallet;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using Nethereum.Signer;
using System.Numerics;
using System.Threading.Tasks;

namespace NethereumUtils.Standard
{
    /// <summary>
    /// Class with a bunch of utility methods which relate to general wallet info and ethereum info of the wallet.
    /// </summary>
    public static class WalletUtils
    {
        /// <summary>
        /// Determines the correct wallet derivation path to use based on how many words are in the mnemonic phrase.
        /// </summary>
        /// <param name="mnemonicPhrase"> The mnemonic phrase to get the path for. </param>
        /// <returns> The correct path to derive the wallet from. </returns>
        public static string DetermineCorrectPath(string mnemonicPhrase)
        {
            switch (GetMnemonicWords(mnemonicPhrase).Length)
            {
                case 12:
                case 15:
                case 18:
                case 21:
                    return Wallet.DEFAULT_PATH;
                case 24:
                    return Wallet.ELECTRUM_LEDGER_PATH;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the individual words of a mnemonic phrase.
        /// </summary>
        /// <param name="str"> The string which contains the words. </param>
        /// <returns> The array of individual words. </returns>
        public static string[] GetMnemonicWords(string str) => str.Trim().Split(' ', '\t', '\n');

        public static async Task<decimal> GetEthBalance(string address)
        {
            EthGetBalance ethGetBalance = new EthGetBalance(NetworkUtils.GetWeb3().Client);
            return SolidityUtils.ConvertFromUInt((await ethGetBalance.SendRequestAsync(address)).Value, 18);
        }

        public static async Task SendEth(string privateKey, string addressTo, decimal amount)
        {
            await SendEth(privateKey, addressTo, amount, await GasUtils.EstimateGasPrice(GasUtils.GasPriceTarget.Standard));
        }

        public static async Task SendEth(string privateKey, string addressTo, decimal amount, BigInteger gasPrice)
        {
            await SendEth(privateKey, addressTo, amount, gasPrice, await GasUtils.EstimateEthGasLimit(addressTo, SolidityUtils.ConvertToUInt(amount, 18)));
        }

        public static async Task SendEth(string privateKey, string addressTo, decimal amount, BigInteger gasPrice, BigInteger gasLimit)
        {
            BigInteger value = SolidityUtils.ConvertToUInt(amount, 18);

            EthECKey ethECKey = new EthECKey(privateKey);

            InMemoryNonceService nonceService = new InMemoryNonceService(ethECKey.GetPublicAddress(), NetworkUtils.GetWeb3().Client);

            TransactionSigner signer = new TransactionSigner();
            string signedTxData = signer.SignTransaction(
                                            privateKey,
                                            NetworkUtils.GetActiveNetwork(),
                                            addressTo,
                                            value,
                                            (await nonceService.GetNextNonceAsync()).Value,
                                            gasPrice,
                                            gasLimit,
                                            string.Empty);

            EthSendRawTransaction rawTransaction = new EthSendRawTransaction(NetworkUtils.GetWeb3().Client);
            await rawTransaction.SendRequestAsync(signedTxData);
        }
    }
}
