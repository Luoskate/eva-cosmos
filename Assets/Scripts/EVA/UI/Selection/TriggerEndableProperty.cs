using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Veery.Import.Triggers;

namespace Veery.UI.Selection {
    public class TriggerEndableProperty : SelectionProperty {
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
            _toggle.isOn = ((ITriggerEndable)Selection).IsEnabled();
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
            AddLinks addLink = ((ITriggerEndable)Selection).AddLink;
            links.AddLink = addLink;
            RemoveLinks removeLink = ((ITriggerEndable)Selection).RemoveLink;
            links.RemoveLink = removeLink;
            GetLinks getLinks = ((ITriggerEndable)Selection).GetLinks;
            links.GetLinks = getLinks;
            links.SetUI();
        }

        public void Toggle(bool value) {
            ((ITriggerEndable)Selection).Toggle(value);
        }
        #endregion Methods
    }
}