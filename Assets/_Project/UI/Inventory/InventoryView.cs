using System.Collections;
using System.Reflection.Emit;
using UnityEngine;

namespace Systems.Inventory
{
    public class InventoryView : StorageView
    {
        [SerializeField] string panelName = "Inventory";

        public override IEnumerator InitializeView(int size = 20)
        {

            root = document.rootVisualElement;
            root.Clear();

            root.styleSheets.Add(styleSheet);

            yield return null;
        }
    }
}
