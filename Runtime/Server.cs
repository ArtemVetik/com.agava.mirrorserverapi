using System;
using UnityEngine.Scripting;

namespace Agava.MirrorServerApi
{
    [Serializable]
    public class Server
    {
        [field: Preserve]
        public string joinCode;

        [field: Preserve]
        public string port7777;

        [field: Preserve]
        public string port7778;

        [field: Preserve]
        public string containerId;
    }
}
