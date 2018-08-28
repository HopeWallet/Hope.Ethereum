using Hope.Ethereum.Utils;
using Nethereum.Signer;
using System;

namespace Hope.Ethereum
{
    public abstract class EthereumContract
    {
        private readonly string mainnetAddress;
        private readonly string rinkebyAddress;

        public string ContractAddress
        {
            get
            {
                if (NetworkProvider.GetActiveNetworkChain() == Chain.MainNet)
                {
                    if (string.IsNullOrEmpty(mainnetAddress))
                        throw new ArgumentNullException("No mainnet address to use.");
                    else
                        return mainnetAddress;
                }
                else
                {
                    if (string.IsNullOrEmpty(rinkebyAddress))
                        throw new ArgumentNullException("No rinkeby address to use.");
                    else
                        return rinkebyAddress;
                }
            }
        }

        protected EthereumContract(string mainnetAddress)
        {
            this.mainnetAddress = mainnetAddress;
        }

        protected EthereumContract(string mainnetAddress, string rinkebyAddress)
        {
            this.mainnetAddress = mainnetAddress;
            this.rinkebyAddress = rinkebyAddress;
        }
    }
}
