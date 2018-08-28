using System.Collections;
using UnityEngine;

namespace Hope.Ethereum.Unity.Utils
{
    public static class CoroutineExtensions
    {
        private static CoroutineServiceProvider ServiceProvider = new GameObject().AddComponent<CoroutineServiceProvider>();

        public static void StartCoroutine(this IEnumerator coroutine) => ServiceProvider.StartCoroutine(coroutine);
    }
}