using System.Numerics;
using System.Threading.Tasks;
using Hope.Ethereum.FunctionOuput;
using Hope.Ethereum.Tokens;
using Hope.Ethereum.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hope.EthereumTests
{
    [TestClass]
    public class HopeEthereumTests
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
            decimal balance = await EthUtils.GetEtherBalance(address);

            Assert.IsTrue(balance > 0);
        }

        //[TestMethod]
        //public async Task SendEthTransactionAsync()
        //{
        //    decimal readableEthAmount = 0.0000000000001m;
        //    decimal readableGasPrice = 5.24m;

        //    BigInteger gasLimit = 75000;
        //    BigInteger gasPrice = GasUtils.GetFunctionalGasPrice(readableGasPrice);
        //    string privateKey = "0x215939f9664cc1a2ad9f004abea96286e81e57fc2c21a8204a1462bec915be8f";

        //    await EthUtils.SendEther(privateKey, "0x5831819C84C05DdcD2568dE72963AC9f7e2833b6", readableEthAmount, gasPrice);
        //        //.OnTransactionSuccessful(() => GetEthBalance())
        //        //.OnTransactionFailure(() => GetEthSendGasLimit());
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
        //    string privateKey = "0x215939f3664cc1a2ac9f000abaa9658de81657fc7c21a8a04a1062bec9156e8f";
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
            ERC20.Messages.Transfer transfer = new ERC20.Messages.Transfer
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
            SimpleOutputs.UInt256 output = await ContractUtils.QueryContract<ERC20.Queries.BalanceOf, SimpleOutputs.UInt256>(
                                        new ERC20.Queries.BalanceOf { Owner = "0xb332Feee826BF44a431Ea3d65819e31578f30446" },
                                        "0x5831819C84C05DdcD2568dE72963AC9f1e6831b6",
                                        null).ConfigureAwait(false);

            decimal result = SolidityUtils.ConvertFromUInt(output.Value, 18);
            Assert.IsTrue(result > 0);
        }
    }
}
