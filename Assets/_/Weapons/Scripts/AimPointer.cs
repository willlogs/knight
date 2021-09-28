using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DB.Knight.Weapons
{
    public class AimPointer : MonoBehaviour
    {
        public Transform aimGoal;

        [SerializeField] private RectTransform _aimUI, _canvas;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Image[] _pins;
        [SerializeField] private float _targetMovementSpeed = 5f;
        [SerializeField] private Gradient _strengthGradient;
        [SerializeField] private Vector2 _defaultSize, _tightSize;

        private float _strength;
        private float halfWidthS, halfHeightS;
        private float halfWidth, halfHeight;
        private float s2cw, s2ch;

        private void Start()
        {
            halfWidthS = Screen.width * 0.5f;
            halfHeightS = Screen.height * 0.5f;

            halfWidth = _canvas.sizeDelta.x * 0.5f;
            halfHeight = _canvas.sizeDelta.y * 0.5f;

            s2cw = Screen.width / _canvas.sizeDelta.x;
        }

        public void Translate(Vector2 diff)
        {
            Vector2 change = (Vector2)(diff * Time.unscaledDeltaTime * _targetMovementSpeed);
            Vector2 newPos = _aimUI.anchoredPosition + change;
            _aimUI.anchoredPosition = Vector2.Lerp(_aimUI.anchoredPosition, newPos, Time.unscaledDeltaTime * 2);

            _aimUI.anchoredPosition = new Vector2(
                Mathf.Clamp(_aimUI.anchoredPosition.x, -halfWidth, halfWidth),
                Mathf.Clamp(_aimUI.anchoredPosition.y, -halfHeight, halfHeight)
            );

            Vector2 sp = _canvas.localScale * _aimUI.anchoredPosition;
            sp = sp + Vector2.up * halfHeightS + Vector2.right * halfWidthS;

            Ray r = Camera.main.ScreenPointToRay(sp);
            RaycastHit hitInfo;
            Physics.Raycast(r, out hitInfo, 15, _layerMask, QueryTriggerInteraction.Ignore);
            if(hitInfo.collider != null)
            {
                aimGoal.position = hitInfo.point;
            }
            else
            {
                aimGoal.position = Camera.main.transform.position + r.direction * 20;
            }
        }

        public void ApplyStrength(float strength)
        {
            _strength = strength;
            ApplyStrength();
        }

        private void ApplyStrength()
        {
            foreach(Image img in _pins)
            {
                img.color = _strengthGradient.Evaluate(_strength);
            }

            Vector2 size = Vector2.Lerp(_defaultSize, _tightSize, _strength);
            _aimUI.sizeDelta = size;
        }
    }
}