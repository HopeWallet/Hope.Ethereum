using Hope.Ethereum.Unity.FunctionOutput;
using Hope.Ethereum.Unity.Promises;
using System;

namespace Hope.Ethereum.Unity.Tokens
{
    /// <summary>
    /// Base class for dynamic ethereum tokens.
    /// </summary>
    public abstract class Token : EthereumContract
    {
        private event Action OnTokenInitializationSuccessful;
        private event Action OnTokenInitializationUnsuccessful;

        private int initializationCounter;
        private bool initializationSuccessful;

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
        protected Token(string mainnetAddress, string name, string symbol, int decimals) : this(mainnetAddress, string.Empty, name, symbol, decimals)
        {
        }

        protected Token(string mainnetAddress) : this(mainnetAddress, string.Empty)
        {
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
            InitializationQuery(QueryName, name => Name = name, _ => Name = null);
            InitializationQuery(QuerySymbol, symbol => Symbol = symbol, _ => Symbol = null);
            InitializationQuery(QueryDecimals, decimals => Decimals = decimals, _ => Decimals = null);
        }

        private void InitializationQuery<T>(Func<EthCallPromise<T>> query, Action<T> onSuccess, Action<string> onError)
        {
            query().OnSuccess(onSuccess).OnError(onError).OnSuccess(_ => CheckInitializationStatus()).OnError(_ => CheckInitializationStatus());
        }

        public void OnInitializationSuccessful(Action onInitializationSuccessful)
        {
            if (initializationCounter == 3 && initializationSuccessful)
                onInitializationSuccessful?.Invoke();
            else
                OnTokenInitializationSuccessful += onInitializationSuccessful;
        }

        public void OnInitializationUnsuccessful(Action onInitializationUnsuccessful)
        {
            if (initializationCounter == 3 && !initializationSuccessful)
                onInitializationUnsuccessful?.Invoke();
            else
                OnTokenInitializationUnsuccessful += onInitializationUnsuccessful;
        }

        private void CheckInitializationStatus()
        {
            if (++initializationCounter == 3)
            {
                if (initializationSuccessful = Decimals != null && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Symbol))
                    OnTokenInitializationSuccessful?.Invoke();
                else
                    OnTokenInitializationUnsuccessful?.Invoke();
            }
        }

        public abstract EthCallPromise<string> QueryName();

        public abstract EthCallPromise<string> QuerySymbol();

        public abstract EthCallPromise<int?> QueryDecimals();
    }
}