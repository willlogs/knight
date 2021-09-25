using BzKovSoft.ObjectSlicer;
using DB.Knight.Weapons;
using DB.Utils;
using DG.Tweening;
using MoreMountains.Feedbacks;
using PT.Utils;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DB.Knight.Slice
{
    public class SlicerWeapon : WeaponTranslator
    {
        [FoldoutGroup("Slicer Specials")]
        [SerializeField] private Transform _sliceEffect;
        [FoldoutGroup("Slicer Specials")]
        [SerializeField] private TrailRenderer _trail;
        [FoldoutGroup("Slicer Specials")]
        [SerializeField] private int _resolution = 32;
        [FoldoutGroup("Slicer Specials")]
        [SerializeField] private float _swordSwingDuration = 0.5f;
        [FoldoutGroup("Slicer Specials")]
        [SerializeField] private LayerMask _layerMask;
        [FoldoutGroup("Slicer Specials")]
        [SerializeField] private MMFeedbacks _feedback;
        [FoldoutGroup("Slicer Specials")]
        [SerializeField] private Transform _topT, _forwardT;

        private bool _isTouching = false, _shouldClear = false;
        private List<RaycastHit> _hitInfos;

        private Vector2 mp;
        private Vector3 defaultPosition;
        private Vector3 forwardDir = Vector3.right;
        private Vector3 topOffset = Vector3.zero;

        private List<Tweener> _seq = new List<Tweener>();

        public override void SetUp(MouseInput mi, Transform pole)
        {
            base.SetUp(mi, pole);
            mi.OnClickStart += OnStart;
            mi.OnClickEnd += OnEnd;
            defaultPosition = transform.localPosition;
            topOffset = _topT.position - transform.position;
            forwardDir = _forwardT.position - transform.position;
        }

        protected override void Update()
        {
            // place the slice effect
            if (_isTouching)
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 campos = Camera.main.transform.position;

                _sliceEffect.transform.position = campos + r.direction * 2;

                if (_shouldClear)
                {
                    _shouldClear = false;
                    _trail.Clear();
                }

                UpdateSlice();
                _lastMouse = Input.mousePosition;
            }
            else
            {
                forwardDir = _forwardT.position - transform.position;
            }

            Vector3 upDir = transform.position - _pole.position;
            transform.rotation = Quaternion.LookRotation(forwardDir, upDir);
        }

        private bool _lastRayHit = false, _hasHit = false;
        private Vector2 _lastMouse = new Vector2();

        private void UpdateSlice()
        {
            ///
            /// TODO: have rosolution for steps bigger than a certain amount
            ///

            // cast ray and check for hit
            int resolution = 5;
            RaycastHit hitInfo = new RaycastHit();
            for (int i = 0; i <= resolution; i++)
            {
                float t = (float)i / (float)resolution;
                Ray r = Camera.main.ScreenPointToRay(Vector2.Lerp(_lastMouse, Input.mousePosition, t));
                Physics.Raycast(r, out hitInfo, 10, _layerMask);
                _lastRayHit = hitInfo.collider != null;

                // hit
                if (_lastRayHit)
                {
                    // first hit
                    if (!_hasHit)
                    {
                        _hasHit = true;
                    }

                    _hitInfos.Add(hitInfo);
                }
                else if (_hasHit) // got out
                {
                    // slice what you got
                    if (_hitInfos.Count > 1)
                    {
                        Vector3 start = new Vector3();
                        Vector3 end = new Vector3();

                        SliceableCharacter sc = _hitInfos[0].collider.GetComponent<SliceableCharacter>();
                        if (sc != null)
                        {
                            BzSliceableCollider bsc = sc.BakeMesh();
                            start = _hitInfos[0].point;
                            end = _hitInfos[_hitInfos.Count - 1].point;
                            Vector3 dir = end - start;
                            forwardDir = dir;
                            Vector3 firstDir = start - Camera.main.transform.position;
                            dir = Vector3.Cross(dir, firstDir);
                            bsc.Slice(new Plane(dir, _hitInfos[0].point));
                        }
                        else
                        {
                            BzSliceableCollider bsc = _hitInfos[0].collider.GetComponent<BzSliceableCollider>();
                            if (bsc != null)
                            {
                                start = _hitInfos[0].point;
                                end = _hitInfos[_hitInfos.Count - 1].point;
                                Vector3 dir = end - start;
                                forwardDir = dir;
                                Vector3 firstDir = start - Camera.main.transform.position;
                                dir = Vector3.Cross(dir, firstDir);
                                bsc.Slice(new Plane(dir, _hitInfos[0].point));
                            }
                        }

                        if(start != end)
                        {
                            transform.position = start;
                            _seq.Add(
                                transform.DOMove(start, _swordSwingDuration).SetUpdate(true)
                            );
                            _seq.Add(
                                transform.DOMove(end, _swordSwingDuration).SetUpdate(true)
                            );

                            if (!_sqPlaying)
                            {
                                ExecNextTweener();
                            }

                            _feedback.PlayFeedbacks();
                        }
                    }

                    _hitInfos = new List<RaycastHit>();
                    _hasHit = false;
                    break;
                }
            }            
        }

        private bool _sqPlaying = false;
        private void ExecNextTweener()
        {
            if(_seq.Count > 0)
            {
                Tweener t = _seq[0];
                _seq.RemoveAt(0);
                t.OnComplete(ExecNextTweener);
                _sqPlaying = true;
            }
            else
            {
                _sqPlaying = false;
            }
        }

        private void OnStart()
        {
            _trail.enabled = true;
            _isTouching = true;
            _hitInfos = new List<RaycastHit>();
            _lastMouse = Input.mousePosition;
            _trail.transform.parent = null;
            TimeManager.Instance.SlowDown(0.01f, 0.1f);
        }

        private void OnEnd()
        {
            _trail.enabled = false;
            _shouldClear = true;
            _isTouching = false;
            _lastRayHit = false;
            _hasHit = false;
            TimeManager.Instance.GoBackALayer(0.1f);

            _seq.Add(
                transform.DOLocalMove(defaultPosition, _swordSwingDuration)
                .SetUpdate(true)
                .OnStart(() => {
                    forwardDir = Vector3.right;
                })
            );

            if (!_sqPlaying)
            {
                ExecNextTweener();
            }
        }

        private void OnDestroy()
        {
            _mouseInputManager.OnClickStart -= OnStart;
            _mouseInputManager.OnClickEnd -= OnEnd;
        }
    }
}