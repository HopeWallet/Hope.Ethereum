using Hope.Ethereum.Utils;

namespace Hope.Ethereum
{
    public abstract class EthereumContract
    {
        private readonly string mainnetAddress;
        private readonly string rinkebyAddress;

        public string ContractAddress => string.IsNullOrEmpty(rinkebyAddress) || NetworkProvider.GetActiveNetworkChain() == Nethereum.Signer.Chain.MainNet ? mainnetAddress : rinkebyAddress;

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
