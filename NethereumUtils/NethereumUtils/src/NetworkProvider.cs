using Nethereum.Signer;
using Nethereum.Web3;

namespace NethereumUtils.Standard
{
    /// <summary>
    /// Class used for providing the network data to other ethereum utility classes.
    /// </summary>
    public static class NetworkProvider
    {
        private const string MAINNET_URL = "https://mainnet.infura.io";
        private const string RINKEBY_URL = "https://rinkeby.infura.io";

        private static Chain ActiveNetwork = Chain.Rinkeby;
        private static Web3 Web3 = new Web3(RINKEBY_URL);

        /// <summary>
        /// Switches the current network chain.
        /// </summary>
        /// <param name="network"> The new active network chain. </param>
        public static void SwitchNetworkChain(Chain network)
        {
            ActiveNetwork = network;
            Web3 = new Web3(GetNetworkChainUrl());
        }

        /// <summary>
        /// Gets the current Web3 instance.
        /// </summary>
        /// <returns> The current Web3 instance. </returns>
        public static Web3 GetWeb3()
        {
            return Web3;
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
        private static string GetNetworkChainUrl()
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
