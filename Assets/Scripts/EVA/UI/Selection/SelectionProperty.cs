using UnityEngine;
using Veery.Import;

namespace Veery.UI.Selection {
    public class SelectionProperty : MonoBehaviour {
        public SelectionProperties SelectionRootHandler { get; set; }
        public ImportObject Selection { get; set; }
        public ImportObject SecondarySelection { get; set; }
    }
}
