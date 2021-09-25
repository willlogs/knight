using DB.Knight.AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DB.Utils
{
    public class RagdollManager : MonoBehaviour
    {
        public SimpleEnemy enemy;
        [SerializeField] private bool _isActive = false;
        [SerializeField] private Collider _selfCollider;
        [SerializeField] private Rigidbody _mainRB;
        [SerializeField] private SkinnedMeshRenderer _mesh;

        private Rigidbody[] _rbs;
        private Collider[] _colliders;

        [Button("Activate")]
        public void Activate()
        {
            transform.parent = null;
            _mesh.transform.parent = null;
            _isActive = true;
            SetActivation();
        }

        [Button("Deactivate")]
        public void Deactivate()
        {
            _isActive = false;
            SetActivation();
        }

        private void Start()
        {
            _rbs = GetComponentsInChildren<Rigidbody>().ToArray();
            _colliders = GetComponentsInChildren<Collider>().ToArray();

            SetActivation();
        }

        private void SetActivation()
        {
            try
            {
                _mainRB.isKinematic = _isActive;
                _selfCollider.enabled = !_isActive;
            }
            catch { }

            foreach (Rigidbody rb in _rbs)
            {
                bool active = !_isActive;
                rb.isKinematic = active;
                rb.interpolation = active?RigidbodyInterpolation.None:RigidbodyInterpolation.Interpolate;
                if (_isActive)
                {
                    rb.drag = 5f;
                }
            }

            foreach (Collider c in _colliders)
            {
                c.enabled = _isActive;
            }
        }
    }
}