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
        [SerializeField] private WeaponTranslator _weapon;
        [FoldoutGroup("Weapon")]
        [SerializeField] private Transform _pole;

        private void Start()
        {
            SetUpWeapon(_weapon);
        }

        private void SetUpWeapon(WeaponTranslator wt)
        {
            _weapon.gameObject.SetActive(false);

            _weapon = wt;
            _weapon.gameObject.SetActive(true);

            _weapon.SetUp(_mouseInputManager, _pole);
        }
    }
}