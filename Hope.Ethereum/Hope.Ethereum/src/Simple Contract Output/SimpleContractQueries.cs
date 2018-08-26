using static Hope.Ethereum.SimpleOutputs;
using UInt64 = Hope.Ethereum.SimpleOutputs.UInt64;
using UInt32 = Hope.Ethereum.SimpleOutputs.UInt32;
using UInt16 = Hope.Ethereum.SimpleOutputs.UInt16;
using String = Hope.Ethereum.SimpleOutputs.String;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Hope.Ethereum.Utils;

namespace Hope.Ethereum
{
    /// <summary>
    /// Class which contains simple output contract queries which return the result based on the query type.
    /// </summary>
    public static class SimpleContractQueries
    {
        /// <summary>
        /// Queries a contract function which returns a <see cref="UInt256"/> type.
        /// Also valid for any uints below 256.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="UInt256"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a UInt256 output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<UInt256> QueryUInt256Output<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, UInt256>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="UInt128"/> type.
        /// Also valid for any uints below 128.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="UInt128"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a UInt128 output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<UInt128> QueryUInt128Output<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, UInt128>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="UInt64"/> type.
        /// Also valid for any uints below 64.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="UInt64"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a UInt64 output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<UInt64> QueryUInt64Output<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, UInt64>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="UInt32"/> type.
        /// Also valid for any uints below 32.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="UInt32"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a UInt32 output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<UInt32> QueryUInt32Output<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, UInt32>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="UInt16"/> type.
        /// Also valid for any uints below 16.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="UInt16"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a UInt16 output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<UInt16> QueryUInt16Output<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, UInt16>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="UInt8"/> type.
        /// Only valid for uint8 type.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="UInt8"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a UInt8 output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<UInt8> QueryUInt8Output<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, UInt8>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="String"/> type.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="String"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a String output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<String> QueryStringOutput<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, String>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="Address"/> type.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="Address"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for an Address output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<Address> QueryAddressOutput<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, Address>(function, contractAddress, senderAddress);
        }

        /// <summary>
        /// Queries a contract function which returns a <see cref="Bool"/> type.
        /// </summary>
        /// <typeparam name="TFunc"> The <see cref="ContractFunction"/> to execute which will return a <see cref="Bool"/> type. </typeparam>
        /// <param name="function"> The ethereum contract function which will query for a Bool output. </param>
        /// <param name="contractAddress"> The contract address of the function to execute. </param>
        /// <param name="senderAddress"> The address sending the query. </param>
        public static Task<Bool> QueryBoolOutput<TFunc>(
            TFunc function,
            string contractAddress,
            string senderAddress) where TFunc : FunctionMessage, new()
        {
            return ContractUtils.QueryContract<TFunc, Bool>(function, contractAddress, senderAddress);
        }
    }
}