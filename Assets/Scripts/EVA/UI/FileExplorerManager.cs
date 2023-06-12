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
        [Tooltip("The container GameObject for the imported objects.")]
        /// <summary>
        /// The container GameObject for the imported objects.
        /// </summary>
        private GameObject _elementContainerGO;

        [SerializeField]
        /// <summary>
        /// The icon definition asset containing the icon associations for the file explorer.
        /// </summary>
        private IconDefinition _iconDefinition;

        [SerializeField]
        /// <summary>
        /// The import prefabs used for instantiating imported objects.
        /// </summary>
        private ImportPrefabs _importPrefabs;
        #endregion Serialized Fields

        #region Properties
        /// <summary>
        /// Gets the singleton instance of the <see cref="FileExplorerManager"/>.
        /// </summary>
        public static FileExplorerManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<FileExplorerManager>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets the dictionary of icon definitions used for the file explorer.
        /// </summary>
        public Dictionary<string, string> IconDefinitions { get; private set; }

        /// <summary>
        /// Gets the dictionary of folder definitions used for the file explorer.
        /// </summary>
        public Dictionary<string, string> FolderDefinitions { get; private set; }

        /// <summary>
        /// Gets the dictionary of extended folder definitions used for the file explorer.
        /// </summary>
        public Dictionary<string, string> FolderExtendedDefinitions { get; private set; }

        /// <summary>
        /// Gets the dictionary of file extension definitions used for the file explorer.
        /// </summary>
        public Dictionary<string, string> FileExtensionDefinitions { get; private set; }

        /// <summary>
        /// Gets the dictionary of file name definitions used for the file explorer.
        /// </summary>
        public Dictionary<string, string> FileNameDefinitions { get; private set; }
        #endregion Properties

        #region Methods
        private void Start() {
            try {
                IconDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    _iconDefinition.iconAssociation.text);
                FolderDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    _iconDefinition.iconFolderName.text);
                FolderExtendedDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    _iconDefinition.iconFolderNameOpen.text);
                FileExtensionDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    _iconDefinition.iconFileExt.text);
                FileNameDefinitions = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    _iconDefinition.iconFileName.text);
                Debug.Log($"[{GetType().Name}] Start() | Initialized {IconDefinitions.Count} icons");
            } catch (Exception e) {
                Debug.LogError($"[{GetType().Name}] Start() | Error initializing icon definitions: {e}");
            }
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
        /// <returns>A tuple containing a boolean indicating whether the import was successful and the imported GameObject.</returns>
        public Task<Tuple<bool, GameObject>> Import(string url) {
            return Import(url, Pose.identity);
        }

        /// <summary>
        /// Imports a file into the scene.
        /// </summary>
        /// <remarks>
        /// See <see cref="Import(string)"/> for supported extensions.
        /// </remarks>
        /// <param name="url">The file to import.</param>
        /// <param name="pose">The pose of the imported object.</param>
        /// <returns>A tuple containing a boolean indicating whether the import was successful and the imported GameObject.</returns>
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

        [Serializable]
        /// <summary>
        /// Represents an icon definition used in the file explorer.
        /// </summary>
        private class IconDefinition {
            [Tooltip("The association between icon keys and sprite paths.")]
            /// <summary>
            /// The association between icon keys and theirs paths.
            /// </summary>
            public TextAsset iconAssociation;

            [Tooltip("The association between folder names and folder icon keys.")]
            /// <summary>
            /// Represents a TextAsset containing the association between folder names and folder icon keys used in the file explorer.
            /// </summary>
            public TextAsset iconFolderName;

            [Tooltip("The association between open folder names and folder icon keys.")]
            /// <summary>
            /// Represents a TextAsset containing the association between folder names and open folder icon keys used in the file explorer.
            /// </summary>
            public TextAsset iconFolderNameOpen;

            [Tooltip("The association between file extensions and file icon keys.")]
            // Represents a TextAsset containing the association between file extensions and file icon keys used in the file explorer.
            public TextAsset iconFileExt;

            [Tooltip("The association between file names and file icon keys.")]
            /// <summary>
            /// Represents a TextAsset containing the association between file names and file icon keys used in the file explorer.
            /// </summary>
            public TextAsset iconFileName;
        }

        [Serializable]
        // Represents a class containing prefabs used for importing files in the file explorer.
        private class ImportPrefabs {
            [Tooltip("The prefab for 3D models.")]
            /// <summary>
            /// Represents the prefab for 3D models used for importing files in the file explorer.
            /// </summary>
            public GameObject model3dPrefab;

            [Tooltip("The prefab for images.")]
            /// <summary>
            /// Represents the prefab for image files used for importing files in the file explorer.
            /// </summary>
            public GameObject imagePrefab;

            [Tooltip("The prefab for sound files.")]
            /// <summary>
            /// Represents the prefab for sound files used for importing files in the file explorer.
            /// </summary>
            public GameObject soundPrefab;

            [Tooltip("The prefab for video files.")]
            /// <summary>
            /// Represents the prefab for video files used for importing files in the file explorer.
            /// </summary>
            public GameObject videoPrefab;

            [Tooltip("The prefab for 360-degree videos.")]
            /// <summary>
            /// Represents the prefab for 360-degree videos used for importing files in the file explorer.
            /// </summary>
            public GameObject video360Prefab;
        }
    }
}
