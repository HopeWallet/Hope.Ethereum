using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using NethereumUtils.Unity.Coroutines;
using System;
using System.Collections;

namespace NethereumUtils.Unity
{
    /// <summary>
    /// Class which extends the nethereum unity transactions to allow for more actions on successful transactions and unsuccessful ones.
    /// </summary>
    public static class TransactionUtils
    {
        /// <summary>
        /// Checks the details of a transaction hash.
        /// </summary>
        /// <param name="txHash"> The transaction hash. </param>
        /// <param name="onTxReceived"> Action to call once the transaction is received. </param>
        public static void CheckTransactionDetails(string txHash, Action<Transaction> onTxReceived) => _GetTransactionDetailsCoroutine(txHash, onTxReceived).StartCoroutine();

        /// <summary>
        /// Starts the coroutine for getting the details of a transaction.
        /// </summary>
        /// <param name="txHash"> The transaction hash. </param>
        /// <param name="onTxReceived"> Action to call once the transaction details have been received. </param>
        /// <returns> The transaction request to await. </returns>
        private static IEnumerator _GetTransactionDetailsCoroutine(string txHash, Action<Transaction> onTxReceived)
        {
            var request = new EthGetTransactionByHashUnityRequest(NetworkProvider.GetNetworkChainUrl());

            yield return request.SendRequest(txHash);

            onTxReceived?.Invoke(request.Result);
        }
    }
}
