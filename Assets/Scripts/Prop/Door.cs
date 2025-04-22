using PreggoJam.Player;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PreggoJam.Prop
{
    public class Door : MonoBehaviour, IActivable
    {
        private Vector2 _basePos;

        private void Awake()
        {
            _basePos = transform.position;
        }

        public void Toggle(bool value)
        {
            Sequence camSeq = DOTween.Sequence();
            camSeq.Append(transform.DOLocalMoveY(value ? _basePos.y : _basePos.y + 2.5f, 1f));
        }
    }
}