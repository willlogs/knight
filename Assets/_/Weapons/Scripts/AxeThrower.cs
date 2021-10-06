using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using PT.Utils;
using DB.Utils;

namespace DB.Knight.Weapons
{
    public class AxeThrower : WeaponTranslator
    {
        [FoldoutGroup("Axe Properties")]
        [SerializeField] private GameObject _axePrefabs;
        [FoldoutGroup("Axe Properties")]
        [SerializeField] private Transform _startPosition, _aimer;
        [FoldoutGroup("Axe Properties")]
        [SerializeField] private float _range = 0.4f;

        [FoldoutGroup("UI")]
        [SerializeField] private AimPointer _aimPointer;

        private bool _isPulling = false;
        private float _strength = 0;
        private Axe _axe;

        public override void SetUp(MouseInput mi, Transform pole = null)
        {
            base.SetUp(mi, pole);
            mi.OnClickStart += OnStart;
            mi.OnClickEnd += OnEnd;
        }

        public override void ShutUp()
        {
            base.ShutUp();
            _mouseInputManager.OnClickStart -= OnStart;
            _mouseInputManager.OnClickEnd -= OnEnd;
        }

        private void Shoot()
        {
            TimeManager.Instance.AddLayer(0f, 1.5f);
            TimeManager.Instance.DoWithDelay(0.2f, () =>
            {
                TimeManager.Instance.GoBackALayer(0.1f);
            });

            _axe.GetShot(_aimer.position - _axe.transform.position, (int)(3 * _strength), _range);
            SpawnAxe();
            _strength = 0f;
            _isPulling = false;
        }

        private void OnStart()
        {
            _isPulling = true;
        }

        private void SpawnAxe()
        {
            GameObject go = Instantiate(_axePrefabs);
            go.transform.position = _startPosition.position;
            go.transform.parent = _startPosition;
            go.transform.forward = transform.forward;
            Vector3 scale = go.transform.localScale;
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(scale, 0.5f).SetUpdate(true);
            _axe = go.GetComponent<Axe>();
        }

        private void OnEnd()
        {
            Shoot();
        }

        private void OnDestroy()
        {
            _mouseInputManager.OnClickStart -= OnStart;
            _mouseInputManager.OnClickEnd -= OnEnd;
        }

        protected override void Update()
        {
            if (_isPulling)
            {
                _aimPointer.Translate(_mouseInputManager.mouseInput);

                if (_strength < 1)
                {
                    _strength += Time.unscaledDeltaTime / 1;
                }
            }
            _aimPointer.ApplyStrength(_strength);
        }

        protected override void Start()
        {
            SpawnAxe();
        }
    }
}