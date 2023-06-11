using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Veery.Import.Triggers;

namespace Veery.UI.Selection {
    public class Links : SelectionProperty {
        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private GameObject _linkPrefab;

        [SerializeField]
        private GameObject _methodsPrefab;

        [SerializeField]
        private GameObject _contentGO;

        [SerializeField]
        private Button _addButton;

        public Delegate AddLink { get; set; }
        public Delegate RemoveLink { get; set; }
        public Delegate GetLinks { get; set; }

        public void Start() {
            _addButton.onClick.AddListener(Add);
            _backButton.onClick.AddListener(Back);
        }

        public void OnDestroy() {
            _addButton.onClick.RemoveListener(Add);
            _backButton.onClick.RemoveListener(Back);
        }

        public void SetUI() {
            Debug.Log($"[{GetType().Name}] SetUI()");
            foreach (TriggerLink link in GetLinks.DynamicInvoke() as List<TriggerLink>) {
                Link link_ = Instantiate(_linkPrefab, _contentGO.transform)
                    .GetComponent<Link>();
                link_.SelectionRootHandler = SelectionRootHandler;
                link_.Selection = Selection;
                link_.Parent = this;
                link_.Link_ = link;
                link_.AddLink = AddLink;
                link_.RemoveLink = RemoveLink;
                link_.GetLinks = GetLinks;
            }
        }

        public void ClearUI() {
            Debug.Log($"[{GetType().Name}] ClearUI()");
            foreach (Transform child in _contentGO.transform) {
                Destroy(child.gameObject);
            }
        }

        public void Add() {
            Debug.Log($"[{GetType().Name}] Add()");
            Methods methods = Instantiate(_methodsPrefab, SelectionRootHandler.transform.parent)
                .GetComponent<Methods>();
            methods.SelectionRootHandler = SelectionRootHandler;
            methods.Selection = Selection;
            methods.AddLink = AddLink;
            methods.RemoveLink = RemoveLink;
            methods.GetLinks = GetLinks;
            methods.Parent = this;
            methods.SetUI();
            gameObject.SetActive(false);
        }

        public void Back() {
            Debug.Log($"[{GetType().Name}] Back()");
            SelectionRootHandler.gameObject.SetActive(true);
            SelectionRootHandler.ClearUI();
            SelectionRootHandler.SetUI(Selection);
            Destroy(gameObject);
        }
    }
}
