using UnityEngine;

namespace NethereumUtils.Unity
{
    public sealed class CoroutineHandler : MonoBehaviour
    {
        private static CoroutineHandler Handler;

        public static CoroutineHandler GetHandler()
        {
            return Handler ?? (Handler = new GameObject().AddComponent<CoroutineHandler>());
        }
    }
}