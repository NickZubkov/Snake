using System.Collections.Generic;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using TMPro;
using UICoreECS;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.DragonIO.UI.UIEntity
{
    public class UILeaderBoard : AUIEntity
    {
        [Title("First")]
        [SerializeField] private List<TextMeshProUGUI> _names;
        [SerializeField] private List<TextMeshProUGUI> _points;
        [SerializeField] private List<Image> _imagesCountries;
        [SerializeField] private List<Image> _imagesOutline;
       
        
        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<Components.LeaderBoard>().View = this;
        }

        public void SetBoardValue(Dragons.Components.DragonHead[] dragonHeads)
        {
            for (int i = 0; i < 4; i++)
            {
                var head = dragonHeads.SafeGetAt(i);
                _names[i].text = head.DragonName;
                _names[i].color = head.DragonNameColor;
                _imagesCountries[i].sprite = head.CountryImage.sprite;
                _points[i].text = $"{head.Points}";
                if (head.HeadID == -1)
                {
                    _imagesCountries[i].gameObject.SetActive(false);
                    _imagesOutline[i].gameObject.SetActive(true);
                }
                else
                {
                    _imagesCountries[i].gameObject.SetActive(true);
                    _imagesOutline[i].gameObject.SetActive(false);
                }
            }
        }
    }
}