using DB.Utils;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Weapons
{
    public class WeaponTranslator : MonoBehaviour
    {
        public bool _slowsDown = false;
        [FoldoutGroup("Weapon")]
        [SerializeField] protected Transform _pole;

        [FoldoutGroup("Variables")]
        [SerializeField] protected float _speed, _lerpSpeed = 1;
        [FoldoutGroup("Variables")]
        [SerializeField] public float _waitTime = 2.5f, _waitDistance = 7.5f, _slowMoFactor = 0.1f;
        [FoldoutGroup("Variables")]
        [SerializeField] protected Rigidbody _rb;

        private bool _works = false;
        protected MouseInput _mouseInputManager;

        public virtual void ShutUp()
        {
            _works = false;
        }

        public virtual void SetUp(MouseInput mi, Transform pole = null)
        {
            _mouseInputManager = mi;
            this._pole = pole;
            _works = true;
        }

        protected Vector3 _offset;
        protected Vector3 _diff;

        protected virtual void Start()
        {
            _offset = transform.localPosition - _pole.localPosition;
            ApplyOffset();
        }

        protected virtual void ApplyOffset()
        {
            transform.localPosition = _pole.localPosition + _offset;
            transform.up = transform.position - _pole.position;
        }

        protected virtual void Update()
        {
            Vector2 mp = new Vector2(-_mouseInputManager.mouseInput.y, _mouseInputManager.mouseInput.x);
            Translate(mp);
        }

        protected virtual void Translate(Vector2 mp)
        {
            _offset = transform.localPosition - _pole.localPosition;
            Quaternion rot = new Quaternion(
                mp.x * Time.unscaledDeltaTime * _speed,
                mp.y * Time.unscaledDeltaTime * _speed,
                0,
                1
            );
            rot = Quaternion.Lerp(Quaternion.identity, rot, Time.unscaledDeltaTime * _lerpSpeed);

            _offset = rot * _offset;
            _diff = _mouseInputManager.mouseInput;

            ApplyOffset();
        }
    }
}