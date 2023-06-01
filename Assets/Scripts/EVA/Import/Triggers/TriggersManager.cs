using System.Threading.Tasks;
using EVA.Interaction;
using UnityEngine;

namespace EVA.Import.Triggers.Triggers {
    public class TriggersManager : MonoBehaviour {
        private static TriggersManager _instance;

        #region Serialized Fields
        [SerializeField]
        private TriggersPrefabs _triggerPrefabs;

        [SerializeField]
        private GameObject _elementContainerGO;

        [SerializeField]
        private Collider _head;

        [SerializeField]
        private DistanceSelectInteractor _interactor;
        #endregion Serialized Fields

        #region Properties
        public static TriggersManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<TriggersManager>();
                }

                return _instance;
            }
        }

        public Collider Head => _head;
        public DistanceSelectInteractor Interactor => _interactor;
        #endregion Properties

        public void ImportAreaTrigger() {
            GameObject areaTriggerGO = Instantiate(_triggerPrefabs.areaTriggerPrefab, _elementContainerGO.transform);
            Task<bool> initTask = areaTriggerGO.GetComponent<AreaTrigger>().Init();
            initTask.Wait();
        }

        [System.Serializable]
        public class TriggersPrefabs {
            public GameObject areaTriggerPrefab;
        }
    }
}
