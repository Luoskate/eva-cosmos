using System.Threading.Tasks;
using EVA.Interaction;
using GLTFast;
using UnityEngine;

namespace EVA.Import {
    /**
    * <summary>
    * Manipulate a 3D model in the scene.
    * </summary>
    */
    public class Model3D : ImportObject {
        #region Serialized Fields
        [SerializeField]
        private GltfBoundsAsset _gltfBoundsAsset;

        [SerializeField]
        private InteractableOutline _interactableOutline;
        #endregion Serialized Fields

        #region Methods
        /**
         * <summary>
         * Loads the <see cref="GltfBoundsAsset" /> and activates all deferred <see cref="GameObject" />.
         * </summary>
         * <param name="url">The URL poiting to the 3D file</param>
         * <returns><see langword="true" /> if loading succeeded, <see langword="false" /> otherwise.</returns>
         */
        public async Task<bool> Init(string url) {
            // Load 3D model
            bool success = await _gltfBoundsAsset.Load(url);
            if (!success) {
                Debug.LogWarning($"[{GetType().Name}] Init() | Error while importing: {url} as {name}");
                return false;
            }

            Outline outline = _gltfBoundsAsset.gameObject.AddComponent<Outline>();
            _interactableOutline.InjectOutline(outline);

            LoadDeferredGOs();

            return true;
        }
        #endregion Methods
    }
}
