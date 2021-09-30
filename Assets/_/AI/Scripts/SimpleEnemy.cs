using PT.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Knight.AI
{
    public class SimpleEnemy : MonoBehaviour
    {
        public UnityEvent OnDeathUE;
        public event Action OnDeath;

		[SerializeField] private EnemyWeapon _weapon;
        [SerializeField] private Transform _target;
        [SerializeField] private float _attackRange = 4f;
        [SerializeField] private Component[] _deleteTheseOnDeath;
        [SerializeField] private bool _fightInRange = true;

        private bool _isDead = false;

        public void Die()
        {
            TimeManager.Instance.DoWithDelay(2f, () =>
            {
                OnDeathUE?.Invoke();
            });
            OnDeath?.Invoke();
            transform.parent = null;
            foreach(Component c in _deleteTheseOnDeath)
            {
                Destroy(c);
            }
            _isDead = true;
            Destroy(gameObject);
        }

        private Vector3 _defTargetPos;

        private void Start()
        {
            _defTargetPos = _target.position;
        }

        private void OnTriggerStay(Collider other){
			if(other.gameObject.layer == 10 && !_isDead){
                _target.position = other.transform.position;

                Vector3 diff = other.transform.position - transform.position;
                if (_fightInRange)
                {
                    if (diff.magnitude < _attackRange)
                    {
                        _weapon.Trigger();
                    }
                }
                else
                {
                    _weapon.Trigger();
                }
			}
		}

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 10)
            {
                _target.position = _defTargetPos;
            }
        }
    }
}