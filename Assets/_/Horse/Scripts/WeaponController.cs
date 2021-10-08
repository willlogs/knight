using DB.Knight.Weapons;
using DB.Utils;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Knight.Horse
{
    public class WeaponController : MonoBehaviour
    {
        [FoldoutGroup("Input Manager")]
        [SerializeField] private MouseInput _mouseInputManager;

        [FoldoutGroup("Weapon")]
        [SerializeField] private WeaponTranslator[] _weapons;
        [FoldoutGroup("Weapon")]
        [SerializeField] private int _wpnIdx;
        [FoldoutGroup("Weapon")]
        [SerializeField] public WeaponTranslator _weapon;
        [FoldoutGroup("Weapon")]
        [SerializeField] private Transform _pole;

        [SerializeField] private SphereCollider _trigger;

        public void DisableWeapon()
        {
            _weapon.ShutUp();
            _weapon.gameObject.SetActive(false);
        }

        public void SetWeapon(int idx)
        {
            _wpnIdx = idx;
            _weapon.ShutUp();
            _weapon.gameObject.SetActive(false);

            _weapon = _weapons[_wpnIdx];
            _weapon.gameObject.SetActive(true);

            _weapon.SetUp(_mouseInputManager, _pole);
            _trigger.radius = _weapon._waitDistance;
        }

        private void Start()
        {
            _weapon = _weapons[_wpnIdx];
            SetUpWeapon(_weapon);
        }

        private void SetUpWeapon(WeaponTranslator wt)
        {
            _weapon.gameObject.SetActive(false);

            _weapon = wt;
            _weapon.gameObject.SetActive(true);

            _weapon.SetUp(_mouseInputManager, _pole);
            _trigger.radius = _weapon._waitDistance;
        }
    }
}