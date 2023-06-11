using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Veery.Import {
    /**
    * <summary>
    * Manipulate an image in the scene.
    * </summary>
    */
    public class Image : ImportObject {
        #region Serialized Fields
        [SerializeField]
        private Renderer _renderer;
        #endregion Serialized Fields

        #region Methods
        /**
         * <summary>
         * Loads the image and activates all deferred <see cref="GameObject" />.
         * </summary>
         * <param name="url">The URL poiting to the image</param>
         * <returns><see langword="true" /> if loading succeeded, <see langword="false" /> otherwise.</returns>
         */
        public async Task<bool> Init(string url) {
            // Load the image
            using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
            UnityWebRequestAsyncOperation request = uwr.SendWebRequest();
            TaskCompletionSource<bool> tcs = new();
            request.completed += (AsyncOperation _) => tcs.SetResult(true);
            _ = await tcs.Task;

            if (uwr.result != UnityWebRequest.Result.Success) {
                Debug.LogWarning($"[{GetType().Name}] GetTexture() | Error while downloading: {uwr.error}");
                return false;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            _renderer.GetComponent<Renderer>().material.mainTexture = texture;

            Url = url;

            LoadDeferredGOs();

            return true;
        }
        #endregion Methods
    }
}
