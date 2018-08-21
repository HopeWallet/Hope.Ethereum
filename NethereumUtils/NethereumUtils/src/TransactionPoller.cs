using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using System;
using System.Timers;

namespace NethereumUtils.Standard
{
    public sealed class TransactionPoller
    {
        private readonly Timer timer;
        private readonly string txHash;

        private event Action OnTransactionSuccess;
        private event Action OnTransactionFail;

        private const double UPDATE_INTERVAL = 3000.0;

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

        public TransactionPoller OnTransactionSuccessful(Action onTransactionSuccess)
        {
            OnTransactionSuccess += onTransactionSuccess;
            return this;
        }

        public TransactionPoller OnTransactionFailure(Action onTransactionFail)
        {
            OnTransactionFail += onTransactionFail;
            return this;
        }

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