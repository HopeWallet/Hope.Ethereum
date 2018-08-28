using Hope.Ethereum.Unity.FunctionOutput;
using Hope.Ethereum.Unity.Promises;

namespace Hope.Ethereum.Unity.Tokens
{
    /// <summary>
    /// Base class for dynamic ethereum tokens.
    /// </summary>
    public abstract class Token : EthereumContract
    {
        /// <summary>
        /// The name of the ethereum token.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The symbol of the ethereum token.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// The decimal count of the ethereum token.
        /// </summary>
        public int? Decimals { get; private set; }

        /// <summary>
        /// Initializes the ethereum token with the required values.
        /// </summary>
        /// <param name="mainnetAddress"> The mainnet address of the token contract. </param>
        /// <param name="name"> The name of the token. </param>
        /// <param name="symbol"> The symbol of the token. </param>
        /// <param name="decimals"> The decimal count of the token. </param>
        protected Token(string mainnetAddress, string name, string symbol, int decimals) : base(mainnetAddress)
        {
            Name = name;
            Symbol = symbol;
            Decimals = decimals;
        }

        protected Token(string mainnetAddress) : base(mainnetAddress)
        {
            GetDetails();
        }

        /// <summary>
        /// Initializes the ethereum token with the required values.
        /// </summary>
        /// <param name="mainnetAddress"> The mainnet address of the token contract. </param>
        /// <param name="rinkebyAddress"> The rinkeby address of the token contract. </param>
        /// <param name="name"> The name of the token. </param>
        /// <param name="symbol"> The symbol of the token. </param>
        /// <param name="decimals"> The decimal count of the token. </param>
        protected Token(string mainnetAddress, string rinkebyAddress, string name, string symbol, int decimals) : base(mainnetAddress, rinkebyAddress)
        {
            Name = name;
            Symbol = symbol;
            Decimals = decimals;
        }

        protected Token(string mainnetAddress, string rinkebyAddress) : base(mainnetAddress, rinkebyAddress)
        {
            GetDetails();
        }

        private void GetDetails()
        {
            QueryName().OnSuccess(name => Name = name?.Value);
            QuerySymbol().OnSuccess(symbol => Symbol = symbol?.Value);
            QueryDecimals().OnSuccess(decimals => Decimals = decimals?.Value);
        }

        public abstract EthCallPromise<SimpleOutputs.String> QueryName();

        public abstract EthCallPromise<SimpleOutputs.String> QuerySymbol();

        public abstract EthCallPromise<SimpleOutputs.UInt256> QueryDecimals();
    }
}