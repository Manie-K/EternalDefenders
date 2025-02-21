using EternalDefenders;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public abstract class StorageView : MonoBehaviour
    {
        public Slot[] Slots;

        [SerializeField] protected UIDocument document;
        [SerializeField] protected StyleSheet styleSheet;

        protected VisualElement root;
        protected VisualElement container;

        IEnumerator Start()
        {
            yield return StartCoroutine(InitializeView());
        }

        public abstract IEnumerator InitializeView(int size = 20);
    }
}
