using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EVA.UI {
    /// <summary>
    /// The <see cref="FileExplorerElementController"/> is used to control <b>ONE</b> element (file / folder) of the file explorer.
    /// </summary>
    public class FileExplorerElementController : MonoBehaviour {
        private FileSystemInfo _fileSystemInfo;

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
        /// Whether the element is a directory.
        /// </summary>
        public bool IsDirectory => Directory.Exists(FileSystemInfo.FullName);
        #endregion Properties

        #region Methods
        /// <summary>
        /// Loads the icon for the element.
        /// </summary>
        private void LoadIcon() {
            transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = FileSystemInfo.Name.ToLowerInvariant();
            string iconKey = (IsDirectory) ? "folder" : "file";

            if (IsDirectory
                && FileExplorerController.FileExplorerManager.FolderDefinitions
                    .ContainsKey(FileSystemInfo.Name.ToLowerInvariant())) {
                iconKey = FileExplorerController.FileExplorerManager
                    .FolderDefinitions[FileSystemInfo.Name.ToLowerInvariant()];
            } else if (!IsDirectory
                && FileExplorerController.FileExplorerManager.FileNameDefinitions
                    .ContainsKey(FileSystemInfo.Name.ToLowerInvariant())) {
                iconKey = FileExplorerController.FileExplorerManager
                    .FileNameDefinitions[FileSystemInfo.Name.ToLowerInvariant()];
            } else if (!IsDirectory
                && FileSystemInfo.Extension is not ""
                && FileExplorerController.FileExplorerManager.FileExtentionDefinitions
                    .ContainsKey(FileSystemInfo.Extension.ToLowerInvariant()[1..])) {
                iconKey = FileExplorerController.FileExplorerManager.FileExtentionDefinitions[
                    FileSystemInfo.Extension.ToLowerInvariant()[1..]];
            }

            // Change the sprite component of "Icon" GameObject to file icon
            Sprite sprite = Resources.Load<Sprite>(
                FileExplorerController.FileExplorerManager.IconDefinitions[iconKey]);
            transform.Find("Icon").GetComponent<SVGImage>().sprite = sprite;
            Debug.Log($"[{GetType().Name}] LoadIcon() | Set icon to: {sprite.name}");
        }

        /// <summary>
        /// Called when the user clicks on the element.
        /// If the element is a folder, it will open the it (using <see cref="FileExplorerController.OpenFolder" />).
        /// Otherwise, it will import the file (using <see cref="FileExplorerManager.Import" />).
        /// </summary>
        public void OnClick() {
            // Opens the folder if the current directory is a directory.
            if (IsDirectory) {
                FileExplorerController.OpenFolder(FileSystemInfo.Name);
                return;
            }

            Task<bool> importTask = FileExplorerController.FileExplorerManager.Import(FileSystemInfo.FullName);
            _ = importTask.ContinueWith((Task<bool> t) => {
                if (t.Result) {
                    Debug.Log($"[{GetType().Name}] OnClick() | Successfully imported: {FileSystemInfo.FullName}");
                } else {
                    Debug.LogWarning(
                        $"[{GetType().Name}] OnClick() | Error while importing: {FileSystemInfo.FullName}");
                }
            });

            FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        }
        #endregion Methods
    }
}
