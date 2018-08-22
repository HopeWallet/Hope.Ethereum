using Nethereum.JsonRpc.UnityClient;
using System;
using System.Collections.Generic;

namespace NethereumUtils.Unity.Promises
{
    /// <summary>
    /// Base class which manages the eventual result of a Nethereum UnityRequest.
    /// </summary>
    /// <typeparam name="TPromise"> The concrete Promise class type. </typeparam>
    /// <typeparam name="TReturn"> The return type of the Promise. </typeparam>
    public abstract class Promise<TPromise, TReturn> where TPromise : Promise<TPromise, TReturn>, new()
    {
        private TReturn successVal;
        private string errorVal;

        private bool finished;

        protected event Action<TReturn> OnPromiseSuccess;
        protected event Action<string> OnPromiseError;
        protected event Action OnPromiseSuccessOrError;

        /// <summary>
        /// Initializes the Promise by assigning some initial event calls.
        /// </summary>
        protected Promise()
        {
            OnPromiseSuccess += _ => OnPromiseSuccessOrError?.Invoke();
            OnPromiseError += _ => OnPromiseSuccessOrError?.Invoke();
            OnPromiseSuccessOrError += () => finished = true;
        }

        /// <summary>
        /// Adds an action to be called if the result of the Promise is success.
        /// </summary>
        /// <param name="onPromiseSuccess"> Action to call with the result if the Promise is successful. </param>
        /// <returns> The current Promise. </returns>
        public TPromise OnSuccess(Action<TReturn> onPromiseSuccess)
        {
            if (finished && !EqualityComparer<TReturn>.Default.Equals(successVal, default(TReturn)))
                onPromiseSuccess?.Invoke(successVal);
            else
                OnPromiseSuccess += onPromiseSuccess;

            return this as TPromise;
        }

        /// <summary>
        /// Adds an action to be called if the result of the Promise is an error.
        /// </summary>
        /// <param name="onPromiseError"> Action to call with the error message if the Promise runs into an error or fails. </param>
        /// <returns> The current Promise. </returns>
        public TPromise OnError(Action<string> onPromiseError)
        {
            if (finished && !string.IsNullOrEmpty(errorVal))
                onPromiseError?.Invoke(errorVal);
            else
                OnPromiseError += onPromiseError;

            return this as TPromise;
        }

        /// <summary>
        /// Adds an action to be called if the result of the Promise is successful or is an error.
        /// </summary>
        /// <param name="onPromiseSuccessOrError"> Action to call when the Promise is finished executing, whether ending on error or successfully. </param>
        /// <returns> The current Promise. </returns>
        public TPromise OnSuccessOrError(Action onPromiseSuccessOrError)
        {
            if (finished)
                onPromiseSuccessOrError?.Invoke();
            else
                OnPromiseSuccessOrError += onPromiseSuccessOrError;

            return this as TPromise;
        }

        /// <summary>
        /// Builds the current Promise which will eventually call the OnPromiseSuccess or OnPromiseError events.
        /// Pass the arguments which will be sent to the concrete Promise once the preliminary check for errors is successful.
        /// </summary>
        /// <typeparam name="T"> The return type of the UnityRequest. </typeparam>
        /// <param name="request"> The UnityRequest to check. </param>
        /// <param name="args"> The arguments to do a more in depth Promise check for success or error. </param>
        public void Build<T>(UnityRequest<T> request, params Func<object>[] args)
        {
            if (request.Exception == null && !EqualityComparer<T>.Default.Equals(request.Result, default(T)))
                InternalBuild(args);
            else
                OnPromiseError?.Invoke(request.Exception.Message);
        }

        /// <summary>
        /// Invokes the OnPromiseSuccess event.
        /// </summary>
        /// <param name="returnVal"> The return value to pass to the event. </param>
        protected void InternalInvokeSuccess(TReturn returnVal)
        {
            if (finished)
                return;

            OnPromiseSuccess?.Invoke(returnVal);
            successVal = returnVal;
        }

        /// <summary>
        /// Invokes the OnPromiseError event.
        /// </summary>
        /// <param name="errorMessage"> The error message of the Promise. </param>
        protected void InternalInvokeError(string errorMessage)
        {
            if (finished)
                return;

            OnPromiseError?.Invoke(errorMessage);
            errorVal = errorMessage;
        }

        /// <summary>
        /// Internal abstract method used for further verifying if a Promise is successful or not.
        /// </summary>
        /// <param name="args"> The arguments to check for success or failure. </param>
        protected abstract void InternalBuild(params Func<object>[] args);
    }
}
