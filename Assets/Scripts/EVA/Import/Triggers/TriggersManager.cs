using System;
using System.Threading.Tasks;
using UnityEngine;
using Veery.Interaction;

namespace Veery.Import.Triggers {
    /// <summary>
    /// Manages all triggers in the scene.
    /// </summary>
    public class TriggersManager : MonoBehaviour {
        private static TriggersManager _instance;

        #region Serialized Fields
        [SerializeField]
        [Tooltip("The prefabs used to create triggers.")]
        /// <summary>
        /// The prefabs used to create different types of triggers.
        /// </summary>
        private TriggersPrefabs _triggerPrefabs;

        [SerializeField]
        [Tooltip("The container for all trigger elements.")]
        /// <summary>
        /// The container game object for all trigger elements.
        /// </summary>
        private GameObject _elementContainerGO;

        [SerializeField]
        [Tooltip("The collider representing the head of the player.")]
        /// <summary>
        /// The collider representing the head of the player.
        /// </summary>
        private Collider _head;

        [SerializeField]
        [Tooltip("The interactor used to select objects.")]
        /// <summary>
        /// The interactor used to select objects at a distance.
        /// </summary>
        private DistanceSelectInteractor _interactor;
        #endregion Serialized Fields

        #region Properties
        /// <summary>
        /// Gets the singleton instance of the <see cref="TriggersManager"/>.
        /// </summary>
        public static TriggersManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<TriggersManager>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets the collider representing the head of the player.
        /// </summary>
        public Collider Head => _head;

        /// <summary>
        /// Gets the interactor used to select objects at a distance.
        /// </summary>
        public DistanceSelectInteractor Interactor => _interactor;
        #endregion Properties

        /// <summary>
        /// Instantiates an area trigger prefab and initializes it.
        /// </summary>
        public void ImportAreaTrigger() {
            GameObject areaTriggerGO = Instantiate(_triggerPrefabs.areaTriggerPrefab, _elementContainerGO.transform);
            Task<bool> initTask = areaTriggerGO.GetComponent<AreaTrigger>().Init();
            initTask.Wait();
        }

        [Serializable]
        /// <summary>
        /// A container class for prefabs used to create different types of triggers.
        /// </summary>
        public class TriggersPrefabs {
            /// <summary>
            /// The prefab used to create an area trigger.
            /// </summary>
            public GameObject areaTriggerPrefab;
        }
    }
}
