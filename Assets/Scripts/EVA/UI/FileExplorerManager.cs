using System.Collections.Generic;
using System.IO;
using GLTFast;
using Newtonsoft.Json;
using UnityEngine;

namespace EVA.UI {
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
        /// Currently supported formats:
        /// <list type="bullet">
        /// <item><description>.glb</description></item>
        /// <item><description>.gltf</description></item>
        /// </list>
        /// </remarks>
        /// <param name="file">The file to import.</param>
        public void Import(FileInfo file) {
            Debug.Log($"[{GetType().Name}] Import({file})");
            if (file.Extension is ".gltf" or ".glb") {
                GameObject prefab = Instantiate(_importPrefabs.model3dPrefab, _elementContainerGO.transform);
                GltfAsset gltf = prefab.GetComponentInChildren<GltfAsset>();
                gltf.Url = file.FullName;
                Debug.Log($"[{GetType().Name}] Import() | Imported {file.FullName} as {prefab.name}");
            }
        }

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
