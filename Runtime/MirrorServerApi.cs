using Mirror;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Agava.MirrorServerApi
{
    public static class MirrorServerApi
    {
        private static PortTransport _transport7777;
        private static PortTransport _transport7778;
        private static string _requestBase;

        public static bool Initialized => _requestBase != null;

        public static void Initialize(PortTransport transport7777, PortTransport transport7778, string apiIp, ushort apiPort)
        {
            _transport7777 = transport7777 ?? throw new ArgumentNullException(nameof(transport7777));
            _transport7778 = transport7778 ?? throw new ArgumentNullException(nameof(transport7778));
            _requestBase = $"http://{apiIp}:{apiPort}";
        }

        public static async Task<string> CreateServer()
        {
            return await Post($"{_requestBase}/createserver", new WWWForm());
        }

        public static async Task Connect(string joinCode, Action<Server> serverLoaded = null)
        {
            var responseString = await Post($"{_requestBase}/connect/{joinCode}", new WWWForm());

            var server = JsonUtility.FromJson<Server>(responseString);

            _transport7777.Port = Convert.ToUInt16(server.port7777);
            _transport7778.Port = Convert.ToUInt16(server.port7778);

            var delayTime = Time.realtimeSinceStartup + 1;

            while (Time.realtimeSinceStartup < delayTime)
                await Task.Yield();

            serverLoaded?.Invoke(server);

            NetworkManager.singleton.StartClient();
        }

        private static async Task<string> Post(string requestUri, WWWForm form)
        {
            using var request = UnityWebRequest.Post(requestUri, form);
            var response = request.SendWebRequest();

            while (response.isDone == false)
                await Task.Yield();

            if (request.error != null)
                throw new HttpRequestException($"Request error: {request.error}");

            return request.downloadHandler.text;
        }
    }
}
