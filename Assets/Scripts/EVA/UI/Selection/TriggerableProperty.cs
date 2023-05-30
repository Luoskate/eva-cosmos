using System.Collections.Generic;
using EVA.Import.Properties;
using EVA.Import.Properties.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace EVA.UI.Selection {
    public class TriggerableProperty : SelectionProperty {
        #region Serialized Fields
        [SerializeField]
        private Toggle _toggle;

        [SerializeField]
        private Button _editButton;

        [SerializeField]
        private GameObject _linksPrefab;
        #endregion Serialized Fields

        private delegate void AddLinks(TriggerLink link);
        private delegate void RemoveLinks(TriggerLink link);
        private delegate List<TriggerLink> GetLinks();

        #region Methods
        public void Start() {
            _toggle.onValueChanged.AddListener(Toggle);
            _editButton.onClick.AddListener(ShowLinks);
            _toggle.isOn = ((ITriggerable)Selection).IsEnabled();
        }

        public void OnDestroy() {
            _toggle.onValueChanged.RemoveListener(Toggle);
            _editButton.onClick.RemoveListener(ShowLinks);
        }

        public void ShowLinks() {
            Debug.Log($"[{GetType().Name}] ShowLinks()");

            SelectionRootHandler.gameObject.SetActive(false);
            Links links = Instantiate(_linksPrefab, SelectionRootHandler.transform.parent).GetComponent<Links>();
            links.SelectionRootHandler = SelectionRootHandler;
            links.Selection = Selection;
            AddLinks addLink = ((ITriggerable)Selection).AddLink;
            links.AddLink = addLink;
            Debug.Log($"[{GetType().Name}] addLink: {addLink}");
            RemoveLinks removeLink = ((ITriggerable)Selection).RemoveLink;
            links.RemoveLink = removeLink;
            GetLinks getLinks = ((ITriggerable)Selection).GetLinks;
            links.GetLinks = getLinks;
            links.SetUI();
        }

        public void Toggle(bool value) {
            ((ITriggerable)Selection).Toggle(value);
        }
        #endregion Methods
    }
}
