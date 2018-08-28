using Nethereum.Signer;

namespace Hope.Ethereum.Unity.Utils
{
    /// <summary>
    /// Class used for providing the network data to other ethereum utility classes.
    /// </summary>
    public static class NetworkProvider
    {
        private const string MAINNET_URL = "https://mainnet.infura.io";
        private const string RINKEBY_URL = "https://rinkeby.infura.io";

        private static Chain ActiveNetwork = Chain.Rinkeby;

        /// <summary>
        /// Switches the current network chain.
        /// </summary>
        /// <param name="network"> The new active network chain. </param>
        public static void SwitchNetworkChain(Chain network)
        {
            if (network != Chain.MainNet && network != Chain.Rinkeby)
                network = Chain.Rinkeby;

            ActiveNetwork = network;
        }

        /// <summary>
        /// Gets the active network chain.
        /// </summary>
        /// <returns> The active Chain. </returns>
        public static Chain GetActiveNetworkChain()
        {
            return ActiveNetwork;
        }

        /// <summary>
        /// Gets the network url given the current Chain.
        /// </summary>
        /// <returns> The network url. </returns>
        public static string GetNetworkChainUrl()
        {
            switch (ActiveNetwork)
            {
                case Chain.MainNet:
                    return MAINNET_URL;
                default:
                case Chain.Rinkeby:
                    return RINKEBY_URL;
            }
        }
    }
}
