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

        public ERC20(string mainnetAddress) : base(mainnetAddress)
        {
        }

        public ERC20(string mainnetAddress, string rinkebyAddress, string name, string symbol, int decimals) : base(mainnetAddress, rinkebyAddress, name, symbol, decimals)
        {
        }

        public ERC20(string mainnetAddress, string rinkebyAddress) : base(mainnetAddress, rinkebyAddress)
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

        /// <summary>
        /// Transfers a certain number of tokens of this contract from a wallet to another address.
        /// </summary>
        /// <param name="gasLimit"> The gas limit to use when sending the tokens. </param>
        /// <param name="gasPrice"> The gas price to use when sending the tokens. </param>
        /// <param name="privateKey"> The private key of the address sending the tokens. </param>
        /// <param name="addressTo"> The address the tokens are being sent to. </param>
        /// <param name="address"> The address to transfer the tokens to. </param>
        /// <param name="amount"> The amount of tokens to transfer. </param>
        public Task<TransactionPoller> Transfer(BigInteger gasLimit, BigInteger gasPrice, string privateKey, string addressTo, decimal amount)
        {
            Messages.Transfer transfer = new Messages.Transfer
            {
                AmountToSend = SolidityUtils.ConvertToUInt(amount, Decimals.Value),
                To = addressTo
            };

            return ContractUtils.SendContractMessage(transfer, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public Task<TransactionPoller> TransferFrom(BigInteger gasLimit, BigInteger gasPrice, string privateKey, string addressFrom, string addressTo, decimal amount)
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