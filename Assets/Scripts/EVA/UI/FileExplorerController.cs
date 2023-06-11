using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Veery.UI {
    /// <summary>
    /// The <see cref="FileExplorerController"/> is used to control <b>ONE</b> file explorer and its elements (<see cref="FileExplorerElementController"/>).
    /// </summary>
    public class FileExplorerController : MonoBehaviour {
        #region Serialized Fields
        [SerializeField]
        [Tooltip("The button to navigate back to the parent folder.")]
        /// <summary>
        /// The button used to navigate back to the parent folder.
        /// </summary>
        private Button _explorerBackButton;

        [SerializeField]
        [Tooltip("The text displaying the current folder.")]
        /// <summary>
        /// The text displaying the current folder name.
        /// </summary>
        private TextMeshProUGUI _explorerCurrentFolderText;

        [SerializeField]
        [Tooltip("The current folder being displayed.")]
        /// <summary>
        /// The current folder being displayed in the file explorer.
        /// </summary>
        private DirectoryInfo _currentFolder;

        [SerializeField]
        [Tooltip("The container GameObject for the explorer list.")]
        /// <summary>
        /// The container GameObject for the explorer list.
        /// </summary>
        private GameObject _explorerListContainerGO;

        [SerializeField]
        [Tooltip("The prefab for the explorer element.")]
        /// <summary>
        /// The prefab for the explorer element.
        /// </summary>
        private GameObject _explorerElementPrefab;
        #endregion Serialized Fields

        #region Properties
        /// <summary>
        /// The <see cref="UI.FileExplorerManager" /> instance.
        /// </summary>
        public FileExplorerManager FileExplorerManager { get; private set; }
        #endregion Properties

        #region Methods
        private void Start() {
            // Get the instance of FileExplorerManager
            FileExplorerManager = FileExplorerManager.Instance;

            // Attach the OpenParent method to the click event of the back button
            _explorerBackButton.onClick.AddListener(OpenParent);

            // Create the current folder
            _currentFolder = Directory.CreateDirectory(Application.persistentDataPath + "/Import");
            Debug.Log($"[{GetType().Name}] Start() | Initializing folder {_currentFolder.FullName}");

            SetElements();
        }

        /// <summary>
        /// Instantiate <see cref="_explorerElementPrefab" /> for each file / directory in the current folder and set the <see cref="FileExplorerElementController" /> properties.
        /// </summary>
        private void SetElements() {
            // Loop through all files/directories in the current folder
            IEnumerable<FileSystemInfo> fileSystemInfos = _currentFolder.EnumerateFileSystemInfos();
            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos) {
                Debug.Log($"[{GetType().Name}] SetElements() | Found \"{fileSystemInfo.FullName}\"");
                // Instantiate a new explorer element for each element
                GameObject explorerElement = Instantiate(_explorerElementPrefab, _explorerListContainerGO.transform);
                FileExplorerElementController fileExplorerElementController = explorerElement
                    .GetComponent<FileExplorerElementController>();
                fileExplorerElementController.FileExplorerController = this;
                fileExplorerElementController.FileSystemInfo = fileSystemInfo;
            }
        }

        /// <summary>
        /// Open the selected folder.
        /// </summary>
        /// <param name="folder">name of the folder</param>
        public void OpenFolder(string folder) {
            // Create the path of the selected folder
            _currentFolder = new(Path.Combine(_currentFolder.FullName, folder));
            // Check if the current folder has a parent and enable the back button if it does
            _explorerBackButton.interactable = _currentFolder.Parent != null;

            // Update the current folder text
            _explorerCurrentFolderText.text = _currentFolder.Name;
            Debug.Log($"[{GetType().Name}] OpenFolder() | Opening folder {_currentFolder.FullName}");

            // Clear the explorer list
            foreach (Transform child in _explorerListContainerGO.transform) {
                Destroy(child.gameObject);
            }

            SetElements();
        }

        /// <summary>
        /// Open the parent folder.
        /// </summary>
        public void OpenParent() {
            // Check if the current folder has a parent
            if (_currentFolder.Parent == null) {
                _explorerBackButton.interactable = false;
                return;
            }

            // Set the current folder to the parent folder
            _currentFolder = new(_currentFolder.Parent.FullName);
            _explorerCurrentFolderText.text = _currentFolder.Name;
            Debug.Log($"[{GetType().Name}] BackFolder() | Opening folder {_currentFolder.FullName}");

            // Clear the explorer list
            foreach (Transform child in _explorerListContainerGO.transform) {
                Destroy(child.gameObject);
            }

            SetElements();
        }
        #endregion Methods
    }
}
