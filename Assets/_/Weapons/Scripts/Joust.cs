using DB.Utils;
using PT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Weapons
{
    public class Joust : MonoBehaviour
    {
        private bool _used = false;
        private Joint _curJoint = null;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6 && !_used)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    CharacterJoint cj = gameObject.AddComponent<CharacterJoint>();
                    cj.connectedBody = rb;
                    cj.autoConfigureConnectedAnchor = false;
                    cj.connectedAnchor = Vector3.zero;
                    //cj.breakForce = 80000f;
                    _curJoint = cj;
                    _used = true;

                    TimeManager.Instance.DoWithDelay(1.5f, () => {
                        Destroy(_curJoint);
                        TimeManager.Instance.DoWithDelay(1f, () => {
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
                }
            }
        }
    }
}