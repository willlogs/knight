using DB.Knight.Weapons;
using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Knight.AI
{
    public class Princess : MonoBehaviour
    {
        [SerializeField] private Transform _handPosition;
        [SerializeField] private float _dropHeight = 0.5f, _multiplier = -1;
        [SerializeField] private Animator _animator;
        [SerializeField] private UnityEvent _stage2Event;

        private int _stage = 0;
        private Transform _tiptop;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 8 && _stage == 0)
            {
                Joust j = other.gameObject.GetComponent<Joust>();

                if (j != null)
                {
                    _tiptop = other.gameObject.GetComponent<Joust>().tiptop;
                    Vector3 _offset = _handPosition.position - transform.position;

                    /*Vector3 relPos = _tiptop.position;
                    relPos.y = transform.position.y;

                    transform.DORotateQuaternion(
                        Quaternion.LookRotation(relPos - transform.position),
                        0.5f
                    );*/

                    transform.DOMove(_tiptop.position - _offset, 0.1f).OnComplete(
                        () =>
                        {
                            _stage = 1;
                            _animator.SetTrigger("Next");
                        }    
                    );
                }
            }
        }

        private void Update()
        {
            if(_stage == 1)
            {
                Vector3 _offset = _handPosition.position - transform.position;
                transform.position = _tiptop.position - _offset;

                if (transform.position.y < _dropHeight)
                {
                    _stage = 2;
                    _stage2Event?.Invoke();
                    _animator.SetTrigger("Next");
                }
            }
        }
    }
}