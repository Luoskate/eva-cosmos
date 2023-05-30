using System.Threading.Tasks;
using UnityEngine;

namespace EVA.Import.Properties.Triggers {
    public class TriggersManager : MonoBehaviour {
        private static TriggersManager _instance;

        #region Serialized Fields
        [SerializeField]
        private TriggersPrefabs _triggerPrefabs;

        [SerializeField]
        private GameObject _elementContainerGO;

        [SerializeField]
        private Collider _head;
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
