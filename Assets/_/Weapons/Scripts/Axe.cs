using DB.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Weapons
{
    public class Axe : MonoBehaviour
    {
        [SerializeField] private float _throwingSpeed = 20f;
        [SerializeField] private Vector3 _torque;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _destroyAfter = 5f, _forcePower = 10000;

        private bool _used = false;

        public void GetShot(Vector3 dir, int dups = 0, float range = 0.2f)
        {
            transform.parent = null;
            _rb.isKinematic = false;
            transform.forward = dir;
            _rb.velocity = dir.normalized * _throwingSpeed;
            _rb.angularVelocity = transform.rotation * _torque;

            for (int i = 0; i < dups; i++)
            {
                GameObject clone = Instantiate(gameObject);
                clone.transform.forward = transform.forward;
                clone.transform.position = transform.position +
                new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized * range * dups / 5;
                Axe arrow = clone.GetComponent<Axe>();
                arrow.GetShot(dir, 0);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == gameObject.layer)
                return;

            if (collision.gameObject.layer == 23)
            {
                RagdollManager rm = collision.gameObject.GetComponentInChildren<RagdollManager>();
                if (rm != null)
                {
                    rm.Activate(0);
                    if (rm.enemy != null)
                    {
                        rm.enemy.Die();
                    }
                }
            }
            else if (!_used)
            {
                _used = true;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    _rb.isKinematic = true;
                    _collider.enabled = false;

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
    }
}