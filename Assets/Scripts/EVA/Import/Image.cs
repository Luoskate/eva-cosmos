using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Veery.Import {
    /// <summary>
    /// Represents an image that can be imported into the game.
    /// </summary>
    public class Image : ImportObject {
        #region Serialized Fields
        [SerializeField]
        [Tooltip("The Renderer component of the image.")]
        /// <summary>
        /// The Renderer component of the image.
        /// </summary>
        private Renderer _renderer;
        #endregion Serialized Fields

        #region Methods
        /// <summary>
        /// Initializes the image by downloading it from the specified URL and setting it as the main texture of the Renderer component.
        /// </summary>
        /// <param name="url">The URL of the image to download.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the initialization was successful.</returns>
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
