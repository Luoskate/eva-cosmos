using UnityEngine;

namespace EVA.UI {
    public class Collapsable : MonoBehaviour {
        [SerializeField]
        private GameObject collapseContainerGO;

        [SerializeField]
        private RectTransform notchRectTransform;

        [SerializeField]
        private RectTransform _CollapsableRectTransform;

        [SerializeField]
        private RectTransform _SurfaceRectTransform;

        private Vector3 _OpenedSize;
        private Vector3 _CollapsedSize;

        public bool IsCollapsed { get; private set; }

        private void Start() {
            _OpenedSize = new Vector3(_CollapsableRectTransform.sizeDelta.x, _CollapsableRectTransform.sizeDelta.y, 1);
            _CollapsedSize = new Vector3(_OpenedSize.x, notchRectTransform.sizeDelta.y, 1);
            IsCollapsed = true;
            collapseContainerGO.SetActive(!IsCollapsed);
            _CollapsableRectTransform.sizeDelta = _CollapsedSize;
            _SurfaceRectTransform.localScale = _CollapsedSize;
        }

        public void Collapse() {
            Debug.Log($"[{GetType().Name}] Collapse()");
            IsCollapsed = !IsCollapsed;
            collapseContainerGO.SetActive(!IsCollapsed);
            _CollapsableRectTransform.sizeDelta = (IsCollapsed) ? _CollapsedSize : _OpenedSize;
            _SurfaceRectTransform.localScale = (IsCollapsed) ? _CollapsedSize : _OpenedSize;
        }
    }
}
