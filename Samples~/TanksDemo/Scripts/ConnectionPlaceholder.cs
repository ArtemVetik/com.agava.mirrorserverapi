using System.Collections;
using TMPro;
using UnityEngine;

namespace Agava.MirrorServerApi.Samples.TanksDemo
{
    public class ConnectionPlaceholder : MonoBehaviour
    {
        private const float AniationDelay = 0.25f;

        [SerializeField] private Canvas _selfCanvas;
        [SerializeField] private TMP_Text _infoText;

        private Coroutine _animation;

        public void Show()
        {
            if (_animation != null)
                StopCoroutine(_animation);

            _animation = StartCoroutine(AnimationLoop());

            _selfCanvas.enabled = true;
        }

        public void Hide()
        {
            _selfCanvas.enabled = false;

            StopCoroutine(_animation);
            _animation = null;
        }

        private IEnumerator AnimationLoop()
        {
            int dotCount = 0;
            var delay = new WaitForSeconds(AniationDelay);

            while (true)
            {
                _infoText.text = "Connection" + new string('.', dotCount);
                dotCount = (dotCount + 1) % 3;
                yield return delay;
            }
        }
    }
}