using System;
using EVA.Import.Properties;
using UnityEngine;

namespace EVA.UI.Selection {
    public abstract class Parameter : SelectionProperty {
        public string Name { get; set; }
        public TriggerLink Link_ { get; set; }
        public Delegate AddLink { get; set; }
        public Delegate RemoveLink { get; set; }
        public Delegate GetLinks { get; set; }

        public abstract void SetUI();
    }
}
