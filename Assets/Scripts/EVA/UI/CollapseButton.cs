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
        private Collapsable collapsable;

        private void Start() {
            notchButtonImage.sprite = notchCollapsedButtonSprite;
        }

        public void OnClick() {
            Debug.Log($"[{GetType().Name}] OnClick()");
            collapsable.Collapse();
            notchButtonImage.sprite = (collapsable.IsCollapsed) ? notchCollapsedButtonSprite : notchOpenedButtonSprite;
            FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        }
    }
}
