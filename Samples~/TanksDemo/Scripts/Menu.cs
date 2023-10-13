using Mirror;
using Mirror.SimpleWeb;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Agava.MirrorServerApi.Samples.TanksDemo
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _joinCodeInput;
        [SerializeField] private Button _createServerButton;
        [SerializeField] private Button _connectButton;
        [SerializeField] private ConnectionPlaceholder _connectionPlaceholder;
        [Header("Initial parameters")]
        [SerializeField] private TelepathyTransport _telepathyTransport;
        [SerializeField] private SimpleWebTransport _webTransport;
        [SerializeField] private string _apiIp;
        [SerializeField] private ushort _apiPort;

        private void OnEnable()
        {
            _createServerButton.onClick.AddListener(CreateServer);
            _connectButton.onClick.AddListener(Connect);
        }

        private void OnDisable()
        {
            _createServerButton.onClick.RemoveListener(CreateServer);
            _connectButton.onClick.RemoveListener(Connect);
        }

        private void Start()
        {
            MirrorServerApi.Initialize(_telepathyTransport, _webTransport, _apiIp, _apiPort);
        }

        private async void CreateServer()
        {
            try
            {
                _connectionPlaceholder.Show();

                var joinCode = await MirrorServerApi.CreateServer();
                await MirrorServerApi.Connect(joinCode, (server) => ServerInfo.Server = server);
            }
            catch (Exception exception)
            {
                _connectionPlaceholder.Hide();
                throw new InvalidOperationException("Create Server Error", exception);
            }
        }

        private async void Connect()
        {
            var joinCode = _joinCodeInput.text;

            try
            {
                _connectionPlaceholder.Show();
                await MirrorServerApi.Connect(joinCode, (server) => ServerInfo.Server = server);
            }
            catch (Exception exception)
            {
                _connectionPlaceholder.Hide();
                throw new InvalidOperationException("Connect Error", exception);
            }
        }
    }
}