using DB.Utils;
using PT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Weapons
{
    public class Joust : MonoBehaviour
    {
        [SerializeField] private Transform _horse;

        private bool _used = false;
        private Joint _curJoint = null;
        private Transform _curRoot;

        private void Start()
        {
            _used = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6 && !_used)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.gameObject.layer = 8;
                    ConfigurableJoint cj = gameObject.AddComponent<ConfigurableJoint>();
                    cj.connectedBody = rb;
                    cj.enablePreprocessing = false;

                    cj.autoConfigureConnectedAnchor = false;
                    cj.connectedAnchor = Vector3.zero;

                    cj.xMotion = ConfigurableJointMotion.Locked;
                    cj.yMotion = ConfigurableJointMotion.Locked;
                    cj.zMotion = ConfigurableJointMotion.Locked;

                    cj.angularXMotion = ConfigurableJointMotion.Locked;
                    cj.angularYMotion = ConfigurableJointMotion.Locked;
                    cj.angularZMotion = ConfigurableJointMotion.Locked;

                    _curJoint = cj;
                    _curRoot.parent = _horse;

                    /*FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                    fj.connectedBody = rb;
                    _curJoint = fj;*/

                    _used = true;

                    TimeManager.Instance.DoWithDelay(2.5f, () =>
                    {
                        _curRoot.parent = null;
                        Destroy(_curJoint);

                        TimeManager.Instance.DoWithDelay(1f, () =>
                        {
                            _used = false;
                        });
                    });
                }
            }
            else if (collision.gameObject.layer == 23)
            {
                RagdollManager rdm = collision.gameObject.GetComponentInChildren<RagdollManager>();
                if (rdm != null)
                {
                    if(rdm.enemy != null)
                    {
                        rdm.enemy.Die();
                    }
                    rdm.Activate();

                    _curRoot = rdm.root;
                }
            }
        }
    }
}