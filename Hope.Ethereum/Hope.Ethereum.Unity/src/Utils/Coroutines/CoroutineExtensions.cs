using System.Collections;
using UnityEngine;

namespace NethereumUtils.Unity.Coroutines
{
    public static class CoroutineExtensions
    {
        private static CoroutineServiceProvider ServiceProvider = new GameObject().AddComponent<CoroutineServiceProvider>();

        public static void StartCoroutine(this IEnumerator coroutine) => ServiceProvider.StartCoroutine(coroutine);
    }
}