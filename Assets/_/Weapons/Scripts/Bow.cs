using DB.Utils;
using DG.Tweening;
using PT.Utils;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DB.Knight.Weapons
{
    public class Bow : WeaponTranslator
    {
        [FoldoutGroup("Bow Properties")]
        [SerializeField] private Transform _arrowPoint, _knot, _knotObjective, _pullerHand, _aimer;
        [FoldoutGroup("Bow Properties")]
        [SerializeField] private GameObject _arrowPrefab;
        [FoldoutGroup("Bow Properties")]
        [SerializeField] private float _pullRadius, _range = 0.4f;
        [FoldoutGroup("Bow Properties")]
        [SerializeField] private DOTweenAnimation _shakeAnimation;

        [FoldoutGroup("UI")]
        [SerializeField] private AimPointer _aimPointer;

        private Tweener _pullTweener;
        private Arrow _arrow;
        private bool _hasArrow = false, _isPulling = false;
        private Vector3 _defaultKnotPos = Vector3.zero;
        private float _strength = 0;

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

        private void SpawnArrow()
        {
            GameObject arrowGO = Instantiate(_arrowPrefab);
            Arrow a = arrowGO.GetComponent<Arrow>();
            arrowGO.transform.parent = _knot;
            arrowGO.transform.localPosition = Vector3.zero;
            arrowGO.transform.forward = transform.forward;

            a.transform.localScale = Vector3.zero;
            a.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true).OnComplete(() => {
                _hasArrow = true;
                _arrow = a;
            });
        }

        private void StartPulling()
        {
            _strength = 0;
            _isPulling = true;
            if (_hasArrow)
            {
                _arrow.transform.parent = _knot;
            }
            _pullerHand.parent = _knot;
            _defaultKnotPos = _knot.localPosition;
            _pullTweener = _knot.DOLocalMove(
                _knotObjective.localPosition,
                1
            ).SetUpdate(true).OnComplete(() =>
            {
                _shakeAnimation.DORestart();
            });
        }

        private void Shoot()
        {
            TimeManager.Instance.AddLayer(0.3f, 1.5f);
            TimeManager.Instance.DoWithDelay(0.2f, () =>
            {
                TimeManager.Instance.GoBackALayer(0.3f);
            });

            _arrow.transform.parent = null;
            _isPulling = false;
            _shakeAnimation.DOPause();
            _pullTweener?.Kill();
            _knot.localPosition = _defaultKnotPos;
            _arrow.transform.parent = null;
            _arrow.transform.localScale = Vector3.one;
            _arrow.GetShot(_aimer.position - _arrow.transform.position, (int)(20 * _strength), _range);
            _hasArrow = false;
            _strength = 0;

            SpawnArrow();
        }

        private void OnStart()
        {
            StartPulling();
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
            SpawnArrow();
        }
    }
}