using DB.Knight.Horse;
using PT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.AI
{
    public class CamRotator : MonoBehaviour
    {
        [SerializeField] private float _rotSpeed = 1f, _slowmoAfter = 2.5f;
        [SerializeField] private WeaponController _wc;

        private List<SimpleEnemy> _enteries = new List<SimpleEnemy>();
        private Transform _target;
        private bool _hasTarget = false;
        private bool _thinking = false;
        private bool _isSlow = false;

        private void Update()
        {
            if(_hasTarget)
            {
                if (_target != null)
                {
                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        Quaternion.LookRotation(_target.position + Vector3.up * 2 - transform.position),
                        Time.unscaledDeltaTime * _rotSpeed
                    );
                }
                else
                    OnLastDied();
            }
            else
            {
                transform.forward = Vector3.Lerp(transform.forward, transform.parent.forward, Time.unscaledDeltaTime * _rotSpeed);
            }
        }

        private void OnLastDied()
        {
            if (!_thinking)
            {
                _thinking = true;
                TimeManager.Instance.DoWithDelay(_wc._weapon._waitTime, () =>
                {
                    for (int i = _enteries.Count - 1; i >= 0; i--)
                    {
                        if (_enteries[i] == null)
                        {
                            _enteries.RemoveAt(i);
                        }
                    }

                    if (_enteries.Count > 0)
                    {
                        _enteries[0].OnDeath += OnLastDied;
                        _target = _enteries[0].transform;

                        if (!_isSlow && _wc._weapon._slowsDown)
                        {
                            TimeManager.Instance.AddLayer(0.5f, _wc._weapon._slowMoFactor);
                            _isSlow = true;
                        }
                    }
                    else
                    {
                        _hasTarget = false;
                        TimeManager.Instance.GoBackALayer(0.1f);
                        _isSlow = false;
                    }
                    _thinking = false;
                });
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            if(other.gameObject.layer == 23)
            {
                SimpleEnemy enemy = other.gameObject.GetComponent<SimpleEnemy>();
                if(enemy != null)
                {
                    _enteries.Add(enemy);

                    if (!_hasTarget)
                    {
                        _target = enemy.transform;
                        _hasTarget = true;
                        enemy.OnDeath += OnLastDied;

                        if (!_isSlow && _wc._weapon._slowsDown)
                        {
                            TimeManager.Instance.AddLayer(0.5f, _wc._weapon._slowMoFactor);
                            _isSlow = true;
                        }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
                return;

            if (other.gameObject.layer == 23)
            {
                SimpleEnemy enemy = other.gameObject.GetComponent<SimpleEnemy>();
                if (enemy != null)
                {
                    _enteries.Remove(enemy);
                    if(_target == enemy.transform)
                    {
                        OnLastDied();
                    }
                }
            }
        }
    }
}