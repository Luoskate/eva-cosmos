using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Veery.Import;

namespace Veery.UI {
    /// <summary>
    /// The <see cref="FileExplorerManager"/> is responsible for managing <b>ALL</b> file explorers (<see cref="FileExplorerController"/>). The scene must contain only one.
    /// </summary>
    public class FileExplorerManager : MonoBehaviour {
        private static FileExplorerManager _instance;

        #region Serialized Fields
        [SerializeField]
        private GameObject _elementContainerGO;

        [SerializeField]
        private IconDefinition _iconDefinition;

        [SerializeField]
        private ImportPrefabs _importPrefabs;
        #endregion Serialized Fields

        #region Properties
        public static FileExplorerManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<FileExplorerManager>();
                }

                return _instance;
            }
        }

        public Dictionary<string, string> IconDefinitions { get; private set; }
        public Dictionary<string, string> FolderDefinitions { get; private set; }
        public Dictionary<string, string> FolderExtendedDefinitions { get; private set; }
        public Dictionary<string, string> FileExtentionDefinitions { get; private set; }
        public Dictionary<string, string> FileNameDefinitions { get; private set; }
        #endregion Properties

        #region Methods
        private void Start() {
            IconDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                _iconDefinition.iconAssociation.text);
            FolderDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                _iconDefinition.iconFolderName.text);
            FolderExtendedDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                _iconDefinition.iconFolderNameOpen.text);
            FileExtentionDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                _iconDefinition.iconFileExt.text);
            FileNameDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                _iconDefinition.iconFileName.text);
            Debug.Log($"[{GetType().Name}] Start() | Initialized {IconDefinitions.Count} icons");
        }

        /// <summary>
        /// Imports a file into the scene.
        /// </summary>
        /// <remarks>
        /// Currently supported extensions are:
        /// <list type="bullet">
        /// <item><description>.glb</description></item>
        /// <item><description>.gltf</description></item>
        /// <item><description>.png</description></item>
        /// <item><description>.jpg</description></item>
        /// <item><description>.jpeg</description></item>
        /// </list>
        /// </remarks>
        /// <param name="url">The file to import.</param>
        public Task<Tuple<bool, GameObject>> Import(string url) {
            return Import(url, Pose.identity);
        }

        public async Task<Tuple<bool, GameObject>> Import(string url, Pose pose) {
            string extension = url.Split('.')[^1];
            if (extension is "gltf" or "glb") {
                GameObject prefab = Instantiate(
                    _importPrefabs.model3dPrefab,
                    pose.position,
                    pose.rotation,
                    _elementContainerGO.transform);
                return new Tuple<bool, GameObject>(await prefab.GetComponent<Model3D>().Init(url), prefab);
            }

            if (extension is "png" or "jpg" or "jpeg") {
                GameObject prefab = Instantiate(_importPrefabs.imagePrefab, _elementContainerGO.transform);
                return new Tuple<bool, GameObject>(await prefab.GetComponent<Image>().Init(url), prefab);
            }

            return new Tuple<bool, GameObject>(false, null);
        }
        #endregion Methods

        [System.Serializable]
        private class IconDefinition {
            public TextAsset iconAssociation;
            public TextAsset iconFolderName;
            public TextAsset iconFolderNameOpen;
            public TextAsset iconFileExt;
            public TextAsset iconFileName;
        }

        [System.Serializable]
        private class ImportPrefabs {
            public GameObject model3dPrefab;
            public GameObject imagePrefab;
            public GameObject soundPrefab;
            public GameObject videoPrefab;
            public GameObject video360Prefab;
        }
    }
}
