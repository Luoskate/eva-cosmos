using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EVA.UI {
    /// <summary>
    /// The <see cref="FileExplorerController"/> is used to control <b>ONE</b> file explorer and its elements (<see cref="FileExplorerElementController"/>).
    /// </summary>
    public class FileExplorerController : MonoBehaviour {
        #region Serialized Fields
        [SerializeField]
        private Button _explorerBackButton;

        [SerializeField]
        private TextMeshProUGUI _explorerCurrentFolderText;

        [SerializeField]
        private DirectoryInfo _currentFolder;

        [SerializeField]
        private GameObject _explorerListContainerGO;

        [SerializeField]
        private GameObject _explorerElementPrefab;
        #endregion Serialized Fields

        /// <summary>
        /// The <see cref="UI.FileExplorerManager" /> instance.
        /// </summary>
        public FileExplorerManager FileExplorerManager { get; private set; }

        #region Methods
        private void Start() {
            FileExplorerManager = FileExplorerManager.Instance;
            _currentFolder = Directory.CreateDirectory(Application.persistentDataPath + "/Import");
            Debug.Log($"[{GetType().Name}] Start() | Initializing folder {_currentFolder.FullName}");

            SetElements();
        }

        /// <summary>
        /// Instantiate <see cref="_explorerElementPrefab" /> for each file / directory in the current folder and set the <see cref="FileExplorerElementController" /> properties.
        /// </summary>
        private void SetElements() {
            // Loop through all files / directories in the current folder
            IEnumerable<FileSystemInfo> fileSystemInfos = _currentFolder.EnumerateFileSystemInfos();
            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos) {
                Debug.Log($"[{GetType().Name}] SetElements() | Found \"{fileSystemInfo.FullName}\"");
                // Instantiate a new explorer element for each element
                GameObject explorerElement = Instantiate(_explorerElementPrefab, _explorerListContainerGO.transform);
                explorerElement.GetComponent<FileExplorerElementController>().FileExplorerController = this;
                explorerElement.GetComponent<FileExplorerElementController>().FileSystemInfo = fileSystemInfo;
            }
        }

        /// <summary>
        /// Open the selected folder.
        /// </summary>
        /// <param name="folder">name of the folder</param>
        public void OpenFolder(string folder) {
            _currentFolder = new DirectoryInfo(_currentFolder.FullName + "/" + folder);
            if (_currentFolder.Parent != null) {
                _explorerBackButton.interactable = true;
            }

            _explorerCurrentFolderText.text = _currentFolder.Name;
            Debug.Log($"[{GetType().Name}] OpenFolder() | Opening folder {_currentFolder.FullName}");

            // Clear the explorer list
            foreach (Transform child in _explorerListContainerGO.transform) {
                Debug.Log($"[{GetType().Name}] OpenFolder() | Destroying child {child}");
                Destroy(child.gameObject);
            }

            SetElements();
        }

        /// <summary>
        /// Open the parent folder.
        /// </summary>
        public void OpenParent() {
            if (_currentFolder.Parent == null) {
                _explorerBackButton.interactable = false;
                return;
            }

            _currentFolder = new DirectoryInfo(_currentFolder.Parent.FullName);
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
