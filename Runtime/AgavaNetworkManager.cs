using Mirror;
using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Agava.MirrorServerApi
{
    public class AgavaNetworkManager : NetworkManager
    {
        [SerializeField] private uint _waitTimeout = 20;

        private int _clientCount = 0;

        public override void OnStartServer()
        {
            base.OnStartServer();

            StartCoroutine(Wait(_waitTimeout, () =>
            {
                if (_clientCount > 0)
                    return;

                Application.Quit();
                RemoveServer();
            }));
        }

        public override void OnServerConnect(NetworkConnectionToClient connection)
        {
            base.OnServerConnect(connection);

            _clientCount++;
        }

        public override void OnServerDisconnect(NetworkConnectionToClient connection)
        {
            base.OnServerDisconnect(connection);

            _clientCount--;

            if (_clientCount > 0)
                return;

            RemoveServer();
        }

        private async void RemoveServer()
        {
            Application.Quit();

            var text = File.ReadAllText("/proc/self/cgroup");
            var containerId = Regex.Match(text, @"cpu:/docker/(\w*)").Groups[1].Value;

            var client = new HttpClient();
            await client.PostAsync($"http://webapi:80/removeserver/{containerId}", null);
        }

        private IEnumerator Wait(float seconds, Action success)
        {
            yield return new WaitForSeconds(seconds);
            success?.Invoke();
        }
    }
}
