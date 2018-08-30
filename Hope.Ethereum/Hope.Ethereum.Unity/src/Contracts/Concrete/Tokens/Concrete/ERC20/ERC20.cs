using Hope.Ethereum.Unity.FunctionOutput;
using Hope.Ethereum.Unity.Promises;
using Hope.Ethereum.Unity.Utils;
using System.Numerics;

namespace Hope.Ethereum.Unity.Tokens
{
    /// <summary>
    /// Class which contains methods for interacting with ERC20 tokens.
    /// </summary>
    public sealed partial class ERC20 : Token
    {
        public ERC20(string mainnetAddress, string rinkebyAddress) : base(mainnetAddress, rinkebyAddress)
        {
        }

        public ERC20(string mainnetAddress) : base(mainnetAddress)
        {
        }

        /// <summary>
        /// Initializes the ERC20 token with all required values.
        /// </summary>
        /// <param name="mainnetAddress"> The mainnet address of this ERC20 token. </param>
        /// <param name="name"> The name of this ERC20 token. </param>
        /// <param name="symbol"> The symbol of this ERC20 token. </param>
        /// <param name="decimals"> The decimal count of this ERC20 token. </param>
        public ERC20(string mainnetAddress, string name, string symbol, int decimals) : base(mainnetAddress, name, symbol, decimals)
        {
        }

        public ERC20(string mainnetAddress, string rinkebyAddress, string name, string symbol, int decimals) : base(mainnetAddress, rinkebyAddress, name, symbol, decimals)
        {
        }

        public override EthCallPromise<string> QueryName()
        {
            EthCallPromise<string> promise = new EthCallPromise<string>();
            SimpleContractQueries.QueryStringOutput(new Queries.Name(), ContractAddress, null)
                                 .OnSuccess(name => promise.Build(() => name?.Value))
                                 .OnError(error => promise.Build(() => "error", () => error));

            return promise;
        }

        public override EthCallPromise<string> QuerySymbol()
        {
            EthCallPromise<string> promise = new EthCallPromise<string>();
            SimpleContractQueries.QueryStringOutput(new Queries.Symbol(), ContractAddress, null)
                                 .OnSuccess(symbol => promise.Build(() => symbol?.Value))
                                 .OnError(error => promise.Build(() => "error", () => error));

            return promise;
        }

        public override EthCallPromise<int?> QueryDecimals()
        {
            EthCallPromise<int?> promise = new EthCallPromise<int?>();
            SimpleContractQueries.QueryUInt256Output(new Queries.Decimals(), ContractAddress, null)
                                 .OnSuccess(decimals => promise.Build(() => (int?)decimals?.Value))
                                 .OnError(error => promise.Build(() => "error", () => error));

            return promise;
        }

        /// <summary>
        /// Gets the token balance of an address.
        /// </summary>
        /// <param name="address"> The address to check the balance of. </param>
        public EthCallPromise<decimal> QueryBalanceOf(string address)
        {
            EthCallPromise<decimal> promise = new EthCallPromise<decimal>();
            SimpleContractQueries.QueryUInt256Output(new Queries.BalanceOf { Owner = address }, ContractAddress, address)
                                 .OnSuccess(balance => promise.Build(() => SolidityUtils.ConvertFromUInt(balance.Value, Decimals.Value)))
                                 .OnError(error => promise.Build(() => "error", () => error));

            return promise;
        }

        /// <summary>
        /// Gets the total supply of this ERC20 token contract.
        /// </summary>
        public EthCallPromise<decimal> QueryTotalSupply()
        {
            EthCallPromise<decimal> promise = new EthCallPromise<decimal>();
            SimpleContractQueries.QueryUInt256Output(new Queries.TotalSupply(), ContractAddress, null)
                                 .OnSuccess(supply => promise.Build(() => SolidityUtils.ConvertFromUInt(supply.Value, Decimals.Value)))
                                 .OnError(error => promise.Build(() => "error", () => error));

            return promise;
        }

        public EthCallPromise<decimal> QueryAllowance(string owner, string spender)
        {
            EthCallPromise<decimal> promise = new EthCallPromise<decimal>();
            SimpleContractQueries.QueryUInt256Output(new Queries.Allowance { Owner = owner, Spender = spender }, ContractAddress, null)
                                 .OnSuccess(allowance => promise.Build(() => SolidityUtils.ConvertFromUInt(allowance.Value, Decimals.Value)))
                                 .OnError(error => promise.Build(() => "error", () => error));

            return promise;
        }

        public EthTransactionPromise Approve(string privateKey, string spender, decimal amount, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.Approve approve = new Messages.Approve
            {
                Spender = spender,
                Value = SolidityUtils.ConvertToUInt(amount, Decimals.Value)
            };

            return ContractUtils.SendContractMessage(approve, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        /// <summary>
        /// Transfers a certain number of tokens of this contract from a wallet to another address.
        /// </summary>
        /// <param name="gasLimit"> The gas limit to use when sending the tokens. </param>
        /// <param name="gasPrice"> The gas price to use when sending the tokens. </param>
        /// <param name="privateKey"> The private key of the address sending the tokens. </param>
        /// <param name="addressTo"> The address the tokens are being sent to. </param>
        /// <param name="address"> The address to transfer the tokens to. </param>
        /// <param name="amount"> The amount of tokens to transfer. </param>
        public EthTransactionPromise Transfer(string privateKey, string addressTo, decimal amount, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.Transfer transfer = new Messages.Transfer
            {
                AmountToSend = SolidityUtils.ConvertToUInt(amount, Decimals.Value),
                To = addressTo
            };

            return ContractUtils.SendContractMessage(transfer, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public EthTransactionPromise TransferFrom(string privateKey, string addressFrom, string addressTo, decimal amount, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.TransferFrom transferFrom = new Messages.TransferFrom
            {
                AmountToSend = SolidityUtils.ConvertToUInt(amount, Decimals.Value),
                From = addressFrom,
                To = addressTo
            };

            return ContractUtils.SendContractMessage(transferFrom, privateKey, ContractAddress, gasPrice, gasLimit);
        }
    }
}