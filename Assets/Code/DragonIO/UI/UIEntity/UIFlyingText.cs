using System.Collections.Generic;
using DG.Tweening;
using Leopotam.Ecs;
using TMPro;
using UICoreECS;
using UnityEngine;

namespace Modules.DragonIO.UI.UIEntity
{
    public class UIFlyingText : AUIEntity
    {
        [SerializeField] private TextMeshProUGUI[] _texts;
        
        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<Components.FlyingText>().View = this;
        }

        public void RunText()
        {
            var text = _texts.SafeGetAt(Random.Range(0, _texts.Length));
            text.gameObject.SetActive(true);
            text.transform.DOScale(Vector3.one * 2f, 1f);
            var rect = text.rectTransform.position;
            var dirX = Random.Range(rect.x * 0.8f, rect.x);
            var dirY = Random.Range(rect.y * 0.8f, rect.y);
            var dir = new Vector2(dirX, dirY);
            text.rectTransform.DOMove(dir, 0.5f);
            var color = text.color * new Vector4(1, 1, 1, 0);
            text.DOColor(color, 1f).OnComplete(() =>
            {
                text.gameObject.SetActive(false);
                text.transform.localScale = Vector3.one;
                text.transform.localPosition = Vector3.zero;
                var color = new Vector4(text.color.r, text.color.g, text.color.b, 1);
                text.color = color;
            });
        }
    }
}