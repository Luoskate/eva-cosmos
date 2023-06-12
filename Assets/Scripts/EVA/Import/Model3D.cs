using System.Threading.Tasks;
using GLTFast;
using UnityEngine;
using Veery.Interaction;

namespace Veery.Import {
    /// <summary>
    /// Represents a 3D model that can be imported into the scene.
    /// </summary>
    public class Model3D : ImportObject {
        #region Serialized Fields
        [SerializeField]
        [Tooltip("The GLTFBoundsAsset component of the 3D model.")]
        /// <summary>
        /// The GLTFBoundsAsset component of the 3D model.
        /// </summary>
        private GltfBoundsAsset _gltfBoundsAsset;

        [SerializeField]
        [Tooltip("The InteractableOutline component of the 3D model.")]
        /// <summary>
        /// The InteractableOutline component of the 3D model.
        /// </summary>
        private InteractableOutline _interactableOutline;
        #endregion Serialized Fields

        #region Methods
        /// <summary>
        /// Initializes the 3D model by loading it from the specified URL and setting up its components.
        /// </summary>
        /// <param name="url">The URL of the 3D model to load.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the initialization was successful.</returns>
        public async Task<bool> Init(string url) {
            // Load 3D model
            bool success = await _gltfBoundsAsset.Load(url);
            if (!success) {
                Debug.LogWarning($"[{GetType().Name}] Init() | Error while importing: {url} as {name}");
                return false;
            }

            Outline outline = _gltfBoundsAsset.gameObject.AddComponent<Outline>();
            _interactableOutline.InjectOutline(outline);

            Url = url;

            LoadDeferredGOs();

            return true;
        }
        #endregion Methods
    }
}
