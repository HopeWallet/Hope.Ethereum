using System.Numerics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using NethereumUtils.Standard;

namespace NethereumUtils.Tests
{
    [TestClass]
    public class NethereumUtilsStandardTests
    {
        [TestMethod]
        public async Task GetEthSendGasLimit()
        {
            string address = "0xb332Feee826BF44a431Ea3d65819e31578f30446";
            BigInteger gasLimit = await GasUtils.EstimateEthGasLimit(address, SolidityUtils.ConvertToUInt(0.04055123, 18));

            Assert.IsTrue(gasLimit > 0);
        }

        [TestMethod]
        public async Task GetEthBalance()
        {
            string address = "0xb332Feee826BF44a431Ea3d65819e31578f30446";
            decimal balance = await EthUtils.GetEthBalance(address);

            Assert.IsTrue(balance > 0);
        }

        //[TestMethod]
        //public async Task SendEthTransactionAsync()
        //{
        //    decimal readableEthAmount = 0.0000000000001m;
        //    decimal readableGasPrice = 5.24m;

        //    BigInteger gasLimit = 75000;
        //    BigInteger gasPrice = GasUtils.GetFunctionalGasPrice(readableGasPrice);
        //    string privateKey = "0x215939f9664cc1a2ac9f004abea96586e81e57fc7c21a8204a1062bec915be8f";

        //    await EthUtils.SendEth(privateKey, "0x5831819C84C05DdcD2568dE72963AC9f7e2833b6", readableEthAmount);
        //}

        //[TestMethod]
        //public async Task SendContractTransactionAsync()
        //{
        //    Transfer transfer = new Transfer
        //    {
        //        To = "0x5831819C84C05DdcD2568dE72963AC9f7e2833b6",
        //        Value = SolidityUtils.ConvertToUInt(1, 18)
        //    };

        //    decimal readableGasPrice = 5.24m;

        //    BigInteger value = SolidityUtils.ConvertToUInt(0.0402000333, 18);
        //    BigInteger gasLimit = 75000;
        //    BigInteger gasPrice = GasUtils.GetFunctionalGasPrice(readableGasPrice);
        //    string privateKey = "0x215939f9664cc1a2ac9f004abea96586e81e57fc7c21a8204a1062bec915be8f";
        //    string contractAddress = "0x5831819C84C05DdcD2568dE72963AC9f1e6831b6";

        //    await ContractUtils.SendContractMessage(transfer, privateKey, contractAddress, gasPrice, gasLimit, value);
        //}

        [TestMethod]
        public async Task EstimateGasPrice()
        {
            BigInteger gasPrice = await GasUtils.EstimateGasPrice(GasUtils.GasPriceTarget.Standard);

            Assert.IsTrue(gasPrice > 0);
        }

        [TestMethod]
        public async Task EstimateGasLimitTest()
        {
            Transfer transfer = new Transfer
            {
                To = "0x5831819C84C05DdcD2568dE72963AC9f7e2833b6",
                Value = SolidityUtils.ConvertToUInt(1, 18)
            };

            BigInteger gasLimit = await GasUtils.EstimateContractGasLimit(
                                                transfer,
                                                "0x5831819C84C05DdcD2568dE72963AC9f1e6831b6",
                                                "0xb332Feee826BF44a431Ea3d65819e31578f30446");

            Assert.IsTrue(gasLimit > 0);
        }

        [TestMethod]
        public async Task QueryTest()
        {
            UInt256 output = await ContractUtils.QueryContract<BalanceOf, UInt256>(
                                        new BalanceOf { Owner = "0xb332Feee826BF44a431Ea3d65819e31578f30446" },
                                        "0x5831819C84C05DdcD2568dE72963AC9f1e6831b6",
                                        null).ConfigureAwait(false);

            decimal result = SolidityUtils.ConvertFromUInt(output.Value, 18);
            Assert.IsTrue(result > 0);
        }
    }

    /// <summary>
    /// Class which contains the data needed to execute the ERC20 transfer function.
    /// </summary>
    [Function("transfer", "bool")]
    public sealed class Transfer : FunctionMessage
    {
        /// <summary>
        /// The address to transfer the ERC20 token to.
        /// </summary>
        [Parameter("address", "_to", 1)]
        public string To { get; set; }

        /// <summary>
        /// The amount of the ERC20 token to send to the destination address.
        /// </summary>
        [Parameter("uint256", "_value", 2)]
        public BigInteger Value { get; set; }
    }

    /// <summary>
    /// Class which contains the data needed to read the balance of a certain address of the ERC20 token contract.
    /// </summary>
    [Function("balanceOf", "uint256")]
    public sealed class BalanceOf : FunctionMessage
    {
        /// <summary>
        /// The owner to check the ERC20 token balance of.
        /// </summary>
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; }
    }

    /// <summary>
    /// Class which acts as a uint256 return type for solidity functions.
    /// </summary>
    [FunctionOutput]
    public sealed class UInt256 : IFunctionOutputDTO
    {
        /// <summary>
        /// The value of the uint256 return type.
        /// </summary>
        [Parameter("uint256", 1)]
        public dynamic Value { get; set; }
    }
}
