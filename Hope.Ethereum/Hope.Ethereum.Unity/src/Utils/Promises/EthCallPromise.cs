using System;

namespace Hope.Ethereum.Unity.Promises
{
    /// <summary>
    /// Class used for retrieving an eventual value after querying the ethereum network for some data.
    /// </summary>
    /// <typeparam name="T"> The return type of this ethereum network call. </typeparam>
    public sealed class EthCallPromise<T> : Promise<EthCallPromise<T>, T>
    {
        /// <summary>
        /// Invokes the success method with the one argument.
        /// </summary>
        /// <param name="args"> The arguments passed to the EthCallPromise. </param>
        protected override void InternalBuild(params Func<object>[] args)
        {
            var arg = args[0]?.Invoke();
            if (arg == null)
                InternalInvokeError("Error retrieving data. Please make sure the contract address has the function you are trying to execute.");
            else if (arg.GetType() == typeof(string) && ((string)arg).Equals("error"))
                InternalInvokeError((string)args[1]?.Invoke());
            else
                InternalInvokeSuccess((T)arg);
        }
    }
}
