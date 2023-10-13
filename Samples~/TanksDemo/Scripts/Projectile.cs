using Mirror;
using UnityEngine;

namespace Agava.MirrorServerApi.Samples.TanksDemo
{
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _destroyAfter = 2;
        [SerializeField] private float _force = 1000;

        private void Start() => _rigidBody.AddForce(transform.forward * _force);

        [ServerCallback]
        private void OnTriggerEnter(Collider _) => DestroySelf();

        public override void OnStartServer() => Invoke(nameof(DestroySelf), _destroyAfter);

        [Server]
        private void DestroySelf() => NetworkServer.Destroy(gameObject);
    }
}
