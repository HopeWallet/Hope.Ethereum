using Hope.Ethereum.Utils;
using System.Numerics;
using System.Threading.Tasks;

namespace Hope.Ethereum.Tokens
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

        public override async Task<string> QueryName()
        {
            var name = await SimpleContractQueries.QueryStringOutput(new Queries.Name(), ContractAddress, null);
            return name?.Value;
        }

        public override async Task<string> QuerySymbol()
        {
            var symbol = await SimpleContractQueries.QueryStringOutput(new Queries.Symbol(), ContractAddress, null);
            return symbol?.Value;
        }

        public override async Task<int> QueryDecimals()
        {
            var decimals = await SimpleContractQueries.QueryUInt256Output(new Queries.Decimals(), ContractAddress, null);
            return decimals?.Value;
        }

        /// <summary>
        /// Gets the token balance of an address.
        /// </summary>
        /// <param name="address"> The address to check the balance of. </param>
        /// <param name="onBalanceReceived"> Callback action which should pass in the received balance of Gold tokens on the address. </param>
        public async Task<decimal> QueryBalanceOf(string address)
        {
            var balance = await SimpleContractQueries.QueryUInt256Output(new Queries.BalanceOf { Owner = address }, ContractAddress, address);
            return SolidityUtils.ConvertFromUInt(balance.Value, Decimals.Value);
        }

        /// <summary>
        /// Gets the total supply of this ERC20 token contract.
        /// </summary>
        /// <param name="onSupplyReceived"> Callback action which should pass in the total supply of this token. </param>
        public async Task<decimal> QueryTotalSupply()
        {
            var supply = await SimpleContractQueries.QueryUInt256Output(new Queries.TotalSupply(), ContractAddress, null);
            return SolidityUtils.ConvertFromUInt(supply.Value, Decimals.Value);
        }

        public async Task<decimal> QueryAllowance(string owner, string spender)
        {
            var allowance = await SimpleContractQueries.QueryUInt256Output(new Queries.Allowance { Owner = owner, Spender = spender }, ContractAddress, null);
            return SolidityUtils.ConvertFromUInt(allowance.Value, Decimals.Value);
        }

        public Task<TransactionPoller> Approve(string privateKey, string spender, decimal amount, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.Approve approve = new Messages.Approve
            {
                Spender = spender,
                Value = SolidityUtils.ConvertToUInt(amount, Decimals.Value)
            };

            return ContractUtils.SendContractMessage(approve, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public Task<TransactionPoller> IncreaseApproval(string privateKey, string spender, decimal addedAmount, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.IncreaseApproval increaseApproval = new Messages.IncreaseApproval
            {
                Spender = spender,
                AddedValue = SolidityUtils.ConvertToUInt(addedAmount, Decimals.Value)
            };

            return ContractUtils.SendContractMessage(increaseApproval, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public Task<TransactionPoller> DecreaseApproval(string privateKey, string spender, decimal subtractedAmount, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.DecreaseApproval decreaseApproval = new Messages.DecreaseApproval
            {
                Spender = spender,
                SubtractedValue = SolidityUtils.ConvertToUInt(subtractedAmount, Decimals.Value)
            };

            return ContractUtils.SendContractMessage(decreaseApproval, privateKey, ContractAddress, gasPrice, gasLimit);
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
        public Task<TransactionPoller> Transfer(string privateKey, string addressTo, decimal amount, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.Transfer transfer = new Messages.Transfer
            {
                AmountToSend = SolidityUtils.ConvertToUInt(amount, Decimals.Value),
                To = addressTo
            };

            return ContractUtils.SendContractMessage(transfer, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public Task<TransactionPoller> TransferFrom(string privateKey, string addressFrom, string addressTo, decimal amount, BigInteger gasLimit, BigInteger gasPrice)
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