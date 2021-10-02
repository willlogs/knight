using DB.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Weapons
{
    public class Arrow : MonoBehaviour
    {
        public Transform arrowEnd;

        public void GetShot(Vector3 dir, int dups = 0, float range = 0.2f)
        {
            _rb.isKinematic = false;
            _collider.enabled = true;
            _rb.velocity = dir.normalized * _speed;

            for(int i = 0; i < dups; i++)
            {
                GameObject clone = Instantiate(gameObject);
                clone.transform.forward = transform.forward;
                clone.transform.position = transform.position +
                new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized * range * dups / 5;
                Arrow arrow = clone.GetComponent<Arrow>();
                arrow.GetShot(dir, 0);
            }
        }

        [FoldoutGroup("Physics")]
        [SerializeField] private Rigidbody _rb;
        [FoldoutGroup("Physics")]
        [SerializeField] private Collider _collider;
        [FoldoutGroup("Physics")]
        [SerializeField] private Transform _centerOfMass;

        [FoldoutGroup("Variables")]
        [SerializeField] private float _speed = 20f, _forcePower = 10000;
        [FoldoutGroup("Variables")]
        [SerializeField] private float _destroyAfter = 5f;

        [SerializeField] private bool _testShoot = false;

        private bool _used = false;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == gameObject.layer)
                return;

            if (collision.gameObject.layer == 23)
            {
                RagdollManager rm = collision.gameObject.GetComponentInChildren<RagdollManager>();
                if(rm != null)
                {
                    rm.Activate(0);
                    if(rm.enemy != null)
                    {
                        rm.enemy.Die();
                    }
                }
            }
            else if(!_used)
            {
                _used = true;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 offset = transform.position - _centerOfMass.position;
                    _rb.isKinematic = true;
                    _collider.enabled = false;

                    /*transform.position = collision.GetContact(0).point + offset;
                    transform.forward = -collision.GetContact(0).normal;*/

                    transform.parent = rb.transform;
                    rb.AddForceAtPosition(transform.forward * _forcePower, collision.GetContact(0).point);
                }
                else
                {
                    _collider.enabled = false;
                    _rb.isKinematic = true;
                }
                StartDecay();
            }
        }

        private void StartDecay()
        {
            transform.DOScale(0, _destroyAfter).OnComplete(() => {
                Destroy(gameObject);
            });
        }

        private void Awake()
        {
            _rb.centerOfMass = _centerOfMass.localPosition;
        }

        private void Start()
        {
            if (_testShoot)
            {
                _testShoot = false;
                GetShot(transform.forward);
            }
        }

        private void Update()
        {
            if(!_rb.isKinematic)
                transform.forward = _rb.velocity.normalized;
        }
    }
}