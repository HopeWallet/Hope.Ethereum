using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using System;
using System.Timers;

namespace NethereumUtils.Standard
{
    /// <summary>
    /// Class used to poll for a transaction receipt to see if it was successful/unsuccessful.
    /// </summary>
    public sealed class TransactionPoller
    {
        private readonly Timer timer;
        private readonly string txHash;

        private event Action OnTransactionSuccess;
        private event Action OnTransactionFail;

        private const double UPDATE_INTERVAL = 3000.0;

        /// <summary>
        /// Initializes the TransactionPoller with the transaction hash to poll.
        /// </summary>
        /// <param name="txHash"> The transaction hash to poll. </param>
        public TransactionPoller(string txHash)
        {
            this.txHash = txHash;

            if (AddressUtils.IsValidTransactionHash(txHash))
            {
                timer = new Timer(UPDATE_INTERVAL);
                timer.Elapsed += CheckTransactionReceipt;
                timer.Start();
            }
        }

        /// <summary>
        /// Adds an action to be called if the result of the transaction is successful.
        /// </summary>
        /// <param name="onTransactionSuccess"> Action to call if the transaction is successful. </param>
        /// <returns> The current TransactionPoller. </returns>
        public TransactionPoller OnTransactionSuccessful(Action onTransactionSuccess)
        {
            OnTransactionSuccess += onTransactionSuccess;
            return this;
        }

        /// <summary>
        /// Adds an action to be called if the result of the transaction is an error.
        /// </summary>
        /// <param name="onTransactionFail"> Action to call with the error message if the transaction runs into an error or fails. </param>
        /// <returns> The current TransactionPoller. </returns>
        public TransactionPoller OnTransactionFailure(Action onTransactionFail)
        {
            OnTransactionFail += onTransactionFail;
            return this;
        }

        /// <summary>
        /// Continuously checks the transaction receipt until it is found, and calls the respective event.
        /// </summary>
        /// <param name="sender"> Sender object. </param>
        /// <param name="eventArgs"> Timer event arguments. </param>
        private async void CheckTransactionReceipt(object sender, ElapsedEventArgs eventArgs)
        {
            EthGetTransactionReceipt ethGetTransactionReceipt = new EthGetTransactionReceipt(NetworkProvider.GetWeb3().Client);
            TransactionReceipt receipt = await ethGetTransactionReceipt.SendRequestAsync(txHash);

            if (receipt == null)
                return;

            if (receipt.HasErrors() == true || receipt.Status.Value == 0)
                OnTransactionFail?.Invoke();
            else
                OnTransactionSuccess?.Invoke();

            timer.Stop();
        }
    }
}