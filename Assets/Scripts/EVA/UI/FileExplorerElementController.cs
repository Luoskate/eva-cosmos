using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Veery.UI {
    /// <summary>
    /// The <see cref="FileExplorerElementController"/> is used to control <b>ONE</b> element (file / folder) of the file explorer.
    /// </summary>
    public class FileExplorerElementController : MonoBehaviour {
        private FileSystemInfo _fileSystemInfo;
        private EventSystem _eventSystem;

        #region Properties
        /// <summary>
        /// The parent <see cref="UI.FileExplorerController" />
        /// </summary>
        public FileExplorerController FileExplorerController { get; set; }

        /// <summary>
        /// The <see cref="System.IO.FileSystemInfo"/> associated to this element.
        /// </summary>
        public FileSystemInfo FileSystemInfo {
            get => _fileSystemInfo;

            set {
                _fileSystemInfo = value;
                LoadIcon();
            }
        }

        /// <summary>
        /// Indicates whether the element is a directory.
        /// </summary>
        public bool IsDirectory => Directory.Exists(FileSystemInfo.FullName);
        #endregion Properties

        #region Methods
        private void Start() {
            _eventSystem = FindObjectOfType<EventSystem>();
            if (_eventSystem == null) {
                Debug.LogError($"[{GetType().Name}] No EventSystem found in the scene.");
            }
        }

        /// <summary>
        /// Loads the icon for the element.
        /// </summary>
        private void LoadIcon() {
            transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = FileSystemInfo.Name.ToLowerInvariant();
            string iconKey = (IsDirectory) ? "folder" : "file";

            FileExplorerManager fileExplorerManager = FileExplorerController.FileExplorerManager;
            if (IsDirectory
                && fileExplorerManager.FolderDefinitions.ContainsKey(FileSystemInfo.Name.ToLowerInvariant())) {
                iconKey = fileExplorerManager.FolderDefinitions[FileSystemInfo.Name.ToLowerInvariant()];
            } else if (!IsDirectory
                && fileExplorerManager.FileNameDefinitions.ContainsKey(FileSystemInfo.Name.ToLowerInvariant())) {
                iconKey = fileExplorerManager.FileNameDefinitions[FileSystemInfo.Name.ToLowerInvariant()];
            } else if (!IsDirectory
                && FileSystemInfo.Extension is not ""
                && fileExplorerManager.FileExtensionDefinitions
                    .ContainsKey(FileSystemInfo.Extension.ToLowerInvariant()[1..])) {
                iconKey = fileExplorerManager.FileExtensionDefinitions[
                    FileSystemInfo.Extension.ToLowerInvariant()[1..]];
            }

            // Change the sprite component of "Icon" GameObject to file icon
            Sprite sprite = Resources.Load<Sprite>(
                fileExplorerManager.IconDefinitions[iconKey]);
            transform.Find("Icon").GetComponent<SVGImage>().sprite = sprite;
            Debug.Log($"[{GetType().Name}] LoadIcon() | Set icon to: {sprite.name}");
        }

        /// <summary>
        /// Called when the user clicks on the element.
        /// If the element is a folder, it will open the it (using <see cref="FileExplorerController.OpenFolder" />).
        /// Otherwise, it will import the file (using <see cref="FileExplorerManager.Import" />).
        /// </summary>
        public async void OnClick() {
            if (IsDirectory) {
                FileExplorerController.OpenFolder(FileSystemInfo.Name);
                return;
            }

            Task<Tuple<bool, GameObject>> importTask = FileExplorerController.FileExplorerManager
                .Import(FileSystemInfo.FullName);
            Tuple<bool, GameObject> importResult = await importTask.ConfigureAwait(false);

            if (importResult.Item1) {
                Debug.Log($"[{GetType().Name}] OnClick() | Successfully imported: {FileSystemInfo.FullName}");
            } else {
                Debug.LogWarning($"[{GetType().Name}] OnClick() | Error while importing: {FileSystemInfo.FullName}");
            }

            _eventSystem.SetSelectedGameObject(null);
        }
        #endregion Methods
    }
}
