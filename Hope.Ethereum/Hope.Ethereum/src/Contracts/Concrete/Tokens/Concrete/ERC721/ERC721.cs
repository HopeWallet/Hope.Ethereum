using Hope.Ethereum.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hope.Ethereum.Tokens
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

        public override Task<int> QueryDecimals()
        {
            return Task.Run(() => 0);
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

        /// <summary>
        /// Gets the token balance of an address.
        /// </summary>
        /// <param name="address"> The address to check the balance of. </param>
        public async Task<decimal> QueryBalanceOf(string address)
        {
            var balance = await SimpleContractQueries.QueryUInt256Output(new Queries.BalanceOf { Owner = address }, ContractAddress, address);
            return SolidityUtils.ConvertFromUInt(balance.Value, Decimals.Value);
        }

        /// <summary>
        /// Gets the total supply of this ERC721 token contract.
        /// </summary>
        public async Task<decimal> QueryTotalSupply()
        {
            var supply = await SimpleContractQueries.QueryUInt256Output(new Queries.TotalSupply(), ContractAddress, null);
            return SolidityUtils.ConvertFromUInt(supply.Value, Decimals.Value);
        }

        public async Task<string> QueryOwnerOf(BigInteger tokenId)
        {
            var supply = await SimpleContractQueries.QueryAddressOutput(new Queries.OwnerOf { TokenId = tokenId }, ContractAddress, null);
            return supply?.Value;
        }

        public async Task<string> QueryTokenURI(BigInteger tokenId)
        {
            var supply = await SimpleContractQueries.QueryStringOutput(new Queries.TokenURI { TokenId = tokenId }, ContractAddress, null);
            return supply?.Value;
        }

        public async Task<BigInteger> QueryTokenOfOwnerByIndex(string ownerAddress, BigInteger index)
        {
            var tokenId = await SimpleContractQueries.QueryUInt256Output(new Queries.TokenOfOwnerByIndex { Owner = ownerAddress, Index = index }, ContractAddress, null);
            return tokenId?.Value;
        }

        public async Task<BigInteger> QueryTokenByIndex(BigInteger index)
        {
            var tokenId = await SimpleContractQueries.QueryUInt256Output(new Queries.TokenByIndex { Index = index }, ContractAddress, null);
            return tokenId?.Value;
        }

        public async Task<string> QueryGetApproved(BigInteger tokenId)
        {
            var approved = await SimpleContractQueries.QueryAddressOutput(new Queries.GetApproved { TokenId = tokenId }, ContractAddress, null);
            return approved?.Value;
        }

        public async Task<bool?> QueryIsApprovedForAll(string ownerAddress, string operatorAddress)
        {
            var approved = await SimpleContractQueries.QueryBoolOutput(new Queries.IsApprovedForAll { Owner = ownerAddress, Operator = operatorAddress }, ContractAddress, null);
            return approved?.Value;
        }

        public Task<TransactionPoller> SafeTransferFrom(string privateKey, string addressFrom, string addressTo, BigInteger tokenId, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.SafeTransferFrom safeTransferFrom = new Messages.SafeTransferFrom
            {
                From = addressFrom,
                To = addressTo,
                TokenId = tokenId
            };

            return ContractUtils.SendContractMessage(safeTransferFrom, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public Task<TransactionPoller> SafeTransferFrom(string privateKey, string addressFrom, string addressTo, BigInteger tokenId, byte[] data, BigInteger gasLimit, BigInteger gasPrice)
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

        public Task<TransactionPoller> Approve(string privateKey, string addressTo, BigInteger tokenId, BigInteger gasLimit, BigInteger gasPrice)
        {
            Messages.Approve approve = new Messages.Approve
            {
                To = addressTo,
                TokenId = tokenId
            };

            return ContractUtils.SendContractMessage(approve, privateKey, ContractAddress, gasPrice, gasLimit);
        }

        public Task<TransactionPoller> SetApprovalForAll(string privateKey, string operatorAddress, bool approved, BigInteger gasLimit, BigInteger gasPrice)
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