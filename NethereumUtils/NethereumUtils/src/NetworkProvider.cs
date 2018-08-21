using Nethereum.Signer;
using Nethereum.Web3;

namespace NethereumUtils.Standard
{
    public static class NetworkProvider
    {
        private const string MAINNET_URL = "https://mainnet.infura.io";
        private const string RINKEBY_URL = "https://rinkeby.infura.io";

        private static Chain ActiveNetwork = Chain.Rinkeby;
        private static Web3 Web3 = new Web3(RINKEBY_URL);

        public static void SwitchNetworkChain(Chain network)
        {
            ActiveNetwork = network;
            Web3 = new Web3(GetNetworkChainUrl());
        }

        public static Web3 GetWeb3()
        {
            return Web3;
        }

        public static Chain GetActiveNetworkChain()
        {
            return ActiveNetwork;
        }

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
