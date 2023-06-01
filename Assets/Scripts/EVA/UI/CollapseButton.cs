using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EVA.UI {
    public class CollapseButton : MonoBehaviour {
        [SerializeField]
        private Image notchButtonImage;

        [SerializeField]
        private Sprite notchCollapsedButtonSprite;

        [SerializeField]
        private Sprite notchOpenedButtonSprite;

        [SerializeField]
        private List<Collapsable> collapsables;

        private void Start() {
            notchButtonImage.sprite = notchCollapsedButtonSprite;
        }

        public void OnClick() {
            Debug.Log($"[{GetType().Name}] OnClick()");
            foreach (Collapsable collapsable in collapsables) {
                collapsable.Collapse();
            }

            notchButtonImage.sprite = (collapsables[0].IsCollapsed)
                ? notchCollapsedButtonSprite
                : notchOpenedButtonSprite;
            FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        }
    }
}
