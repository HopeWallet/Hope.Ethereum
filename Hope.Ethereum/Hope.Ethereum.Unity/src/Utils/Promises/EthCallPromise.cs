using System;

namespace NethereumUtils.Unity.Promises
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
            InternalInvokeSuccess((T)args[0]?.Invoke());
        }
    }
}
