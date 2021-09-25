using DB.Knight.Weapons;
using PT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DB.Knight.AI
{
    public class BowWeapon : EnemyWeapon
    {
        [SerializeField] private Arrow _arrow;
        [SerializeField] private Animator _archerAnimator;
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private Transform _aimT;

        private Transform _arrowHolder;
        private bool _canArch = true;
        private bool _hasArrow = true;

        public override void Trigger()
        {
            if (_canArch)
            {
                _archerAnimator.SetTrigger("Trigger");
                _canArch = false;
                TimeManager.Instance.DoWithDelay(1f, () =>
                {
                    _canArch = true;
                });
            }
        }

        public void ShootArrow()
        {
            _hasArrow = false;
            _arrow.transform.parent = null;
            _arrow.GetShot(_aimT.position + Vector3.up - _arrow.transform.position, 3);
        }

        public void LoadArrow()
        {
            if (!_hasArrow)
            {
                _arrow = Instantiate(_arrowPrefab).GetComponent<Arrow>();
                _arrow.transform.parent = _arrowHolder;
                _arrow.transform.localPosition = Vector3.zero;
                _arrow.transform.localScale = Vector3.zero;
                _arrow.transform.localRotation = Quaternion.identity;
                _arrow.transform.DOScale(1.2f, 0.5f);

                _hasArrow = true;
            }
        }

        private void Start()
        {
            _arrowHolder = new GameObject().transform;
            _arrowHolder.parent = _arrow.transform.parent;
            _arrowHolder.position = _arrow.transform.position;
            _arrowHolder.rotation = _arrow.transform.rotation;
        }
    }
}