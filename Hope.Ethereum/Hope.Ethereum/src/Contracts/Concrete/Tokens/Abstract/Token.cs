using System;
using System.Threading.Tasks;

namespace Hope.Ethereum.Tokens
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
            GetName();
            GetSymbol();
            GetDecimals();
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

        private async void GetName()
        {
            Name = await QueryName();
            CheckInitializationStatus();
        }

        private async void GetSymbol()
        {
            Symbol = await QuerySymbol();
            CheckInitializationStatus();
        }

        private async void GetDecimals()
        {
            Decimals = await QueryDecimals();
            CheckInitializationStatus();
        }

        private void CheckInitializationStatus()
        {
            if (++initializationCounter == 3)
            {
                if (initializationSuccessful = Decimals.HasValue && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Symbol))
                    OnTokenInitializationSuccessful?.Invoke();
                else
                    OnTokenInitializationUnsuccessful?.Invoke();
            }
        }

        public abstract Task<string> QueryName();

        public abstract Task<string> QuerySymbol();

        public abstract Task<int> QueryDecimals();
    }
}