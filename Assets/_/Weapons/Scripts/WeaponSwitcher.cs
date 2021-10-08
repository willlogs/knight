using DB.Knight.Horse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Knight.Weapons
{
    public class WeaponSwitcher : MonoBehaviour
    {
        public UnityEvent OnAfterTriggered;

        public void SetWeapon(int idx)
        {
            if (_hasWC)
            {
                _wc.SetWeapon(idx);
            }
            _UI.SetActive(false);
            _ww._waypoints = _nextWP;
            _ww.GotoNext(true);
            Destroy(gameObject);
        }

        [SerializeField] private GameObject _UI;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform[] _nextWP;

        private WeaponController _wc;
        private WaypointWalker _ww;
        [SerializeField] private bool _hasWC;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 10)
            {
                _wc = other.GetComponent<WeaponController>();
                _ww = other.transform.parent.GetComponent<WaypointWalker>();
                _hasWC = _wc != null && _ww != null;

                if (_hasWC)
                {
                    _UI.SetActive(true);
                    _ww.StopMoving();
                    _wc.DisableWeapon();
                }

                OnAfterTriggered?.Invoke();
            }
        }
    }
}