using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Agava.MirrorServerApi.Samples.TanksDemo
{
    public class Tank : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator  _animator;
        [SerializeField] private TextMesh  _healthBar;
        [SerializeField] private Transform _turret;
        [Header("Movement")]
        [SerializeField] private float _rotationSpeed = 80;
        [Header("Firing")]
        [SerializeField] private KeyCode _shootKey = KeyCode.Space;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform  _projectileMount;

        [Header("Stats")]
        [SyncVar][SerializeField] private int health = 4;

        private void Update()
        {
            _healthBar.text = new string('-', health);
            
            if(Application.isFocused == false)
                return; 

            if (isLocalPlayer)
            {
                float horizontal = Input.GetAxis("Horizontal");
                transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

                float vertical = Input.GetAxis("Vertical");
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                _agent.velocity = forward * Mathf.Max(vertical, 0) * _agent.speed;
                _animator.SetBool("Moving", _agent.velocity != Vector3.zero);

                if (Input.GetKeyDown(_shootKey))
                    CmdFire();

                RotateTurret();
            }
        }

        [Command]
        private void CmdFire()
        {
            GameObject projectile = Instantiate(_projectilePrefab, _projectileMount.position, _projectileMount.rotation);
            NetworkServer.Spawn(projectile);
            RpcOnFire();
        }

        [ClientRpc]
        private void RpcOnFire()
        {
            _animator.SetTrigger("Shoot");
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Projectile>() != null)
            {
                --health;

                if (health == 0)
                    NetworkServer.Destroy(gameObject);
            }
        }

        private void RotateTurret()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.DrawLine(ray.origin, hit.point);

                Vector3 lookRotation = new Vector3(hit.point.x, _turret.transform.position.y, hit.point.z);
                _turret.transform.LookAt(lookRotation);
            }
        }
    }
}