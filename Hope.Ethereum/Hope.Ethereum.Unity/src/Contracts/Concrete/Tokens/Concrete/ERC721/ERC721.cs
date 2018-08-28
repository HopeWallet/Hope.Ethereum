using Hope.Ethereum.Unity.FunctionOutput;
using Hope.Ethereum.Unity.Promises;
using Hope.Ethereum.Unity.Utils;
using System.Numerics;
using System.Threading.Tasks;

namespace Hope.Ethereum.Unity.Tokens
{
    public sealed partial class ERC721 : Token
    {
        public ERC721(string mainnetAddress) : base(mainnetAddress)
        {
        }

        public ERC721(string mainnetAddress, string rinkebyAddress) : base(mainnetAddress, rinkebyAddress)
        {
        }

        public ERC721(string mainnetAddress, string name, string symbol, int decimals) : base(mainnetAddress, name, symbol, decimals)
        {
        }

        public ERC721(string mainnetAddress, string rinkebyAddress, string name, string symbol, int decimals) : base(mainnetAddress, rinkebyAddress, name, symbol, decimals)
        {
        }

        public override EthCallPromise<SimpleOutputs.String> QueryName()
        {
            return SimpleContractQueries.QueryStringOutput(new Queries.Name(), ContractAddress, null);
        }

        public override EthCallPromise<SimpleOutputs.String> QuerySymbol()
        {
            return SimpleContractQueries.QueryStringOutput(new Queries.Symbol(), ContractAddress, null);
        }

        public override EthCallPromise<SimpleOutputs.UInt256> QueryDecimals()
        {
            EthCallPromise<SimpleOutputs.UInt256> decimalsPromise = new EthCallPromise<SimpleOutputs.UInt256>();
            decimalsPromise.Build(() => new SimpleOutputs.UInt256 { Value = 0 });
            return decimalsPromise;
        }

        /// <summary>
        /// Gets the token balance of an address.
        /// </summary>
        /// <param name="address"> The address to check the balance of. </param>
        public EthCallPromise<SimpleOutputs.UInt256> QueryBalanceOf(string address)
        {
            return SimpleContractQueries.QueryUInt256Output(new Queries.BalanceOf { Owner = address }, ContractAddress, address);
        }

        /// <summary>
        /// Gets the total supply of this ERC721 token contract.
        /// </summary>
        public EthCallPromise<SimpleOutputs.UInt256> QueryTotalSupply()
        {
            return SimpleContractQueries.QueryUInt256Output(new Queries.TotalSupply(), ContractAddress, null);
        }

        public EthCallPromise<SimpleOutputs.Address> QueryOwnerOf(BigInteger tokenId)
        {
            return SimpleContractQueries.QueryAddressOutput(new Queries.OwnerOf { TokenId = tokenId }, ContractAddress, null);
        }

        public EthCallPromise<SimpleOutputs.String> QueryTokenURI(BigInteger tokenId)
        {
            return SimpleContractQueries.QueryStringOutput(new Queries.TokenURI { TokenId = tokenId }, ContractAddress, null);
        }

        public EthCallPromise<SimpleOutputs.UInt256> QueryTokenOfOwnerByIndex(string ownerAddress, BigInteger index)
        {
            return SimpleContractQueries.QueryUInt256Output(new Queries.TokenOfOwnerByIndex { Owner = ownerAddress, Index = index }, ContractAddress, null);
        }

        public EthCallPromise<SimpleOutputs.UInt256> QueryTokenByIndex(BigInteger index)
        {
            return SimpleContractQueries.QueryUInt256Output(new Queries.TokenByIndex { Index = index }, ContractAddress, null);
        }

        public EthCallPromise<SimpleOutputs.Address> QueryGetApproved(BigInteger tokenId)
        {
            return SimpleContractQueries.QueryAddressOutput(new Queries.GetApproved { TokenId = tokenId }, ContractAddress, null);
        }

        public EthCallPromise<SimpleOutputs.Bool> QueryIsApprovedForAll(string ownerAddress, string operatorAddress)
        {
            return SimpleContractQueries.QueryBoolOutput(new Queries.IsApprovedForAll { Owner = ownerAddress, Operator = operatorAddress }, ContractAddress, null);
        }

        public EthTransactionPromise SafeTransferFrom(string privateKey, string addressFrom, string addressTo, BigInteger tokenId, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.SafeTransferFrom safeTransferFrom = new Messages.SafeTransferFrom
            {
                From = addressFrom,
                To = addressTo,
                TokenId = tokenId
            };

            return ContractUtils.SendContractMessage(safeTransferFrom, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public EthTransactionPromise SafeTransferFrom(string privateKey, string addressFrom, string addressTo, BigInteger tokenId, byte[] data, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.SafeTransferFromExtraData safeTransferFrom = new Messages.SafeTransferFromExtraData
            {
                From = addressFrom,
                To = addressTo,
                TokenId = tokenId,
                Data = data
            };

            return ContractUtils.SendContractMessage(safeTransferFrom, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public EthTransactionPromise Approve(string privateKey, string addressTo, BigInteger tokenId, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.Approve approve = new Messages.Approve
            {
                To = addressTo,
                TokenId = tokenId
            };

            return ContractUtils.SendContractMessage(approve, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public EthTransactionPromise SetApprovalForAll(string privateKey, string operatorAddress, bool approved, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.SetApprovalForAll setApprovalForAll = new Messages.SetApprovalForAll
            {
                Operator = operatorAddress,
                Approved = approved
            };

            return ContractUtils.SendContractMessage(setApprovalForAll, privateKey, ContractAddress, gasPrice, gasLimit);
        }
    }
}