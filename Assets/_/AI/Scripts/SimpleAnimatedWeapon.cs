using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT.Utils;

namespace DB.Knight.AI
{
    public class SimpleAnimatedWeapon : EnemyWeapon
    {
        [SerializeField] private float _betweenShots;
		[SerializeField] private Animator _animator;

        private bool _canShoot = true;

        public override void Trigger()
        {
			if(_canShoot){
				_animator.SetTrigger("Trigger");
				_canShoot = false;
				TimeManager.Instance.DoWithDelay(
					_betweenShots,
					() => { _canShoot = true; }
				);
			}
        }
    }
}