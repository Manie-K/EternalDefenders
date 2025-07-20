using System.Collections;
using UnityEngine;

namespace EternalDefenders
{
    public class ResourceTowerController : TowerBase
    {
        [SerializeField] ResourceSO resource;
        [SerializeField] int amountPerInterval;
        [SerializeField] float interval;

        bool _isGenerating;
        Coroutine _generationCoroutine;
        void Start()
        {
            _isGenerating = true;
            _generationCoroutine = StartCoroutine(GenerateResource());
        }

        void OnDestroy()
        {
            StopCoroutine(_generationCoroutine);
            _isGenerating = false;
        }
        
        public void StopGenerating() => _isGenerating = false;
        public void StartGenerating() => _isGenerating = true;
        
        IEnumerator GenerateResource()
        {
            while (_isGenerating)
            {
                yield return new WaitForSeconds(interval);
                Debug.Log($"Generated {amountPerInterval} of {resource.name}");
                PlayerResourceInventory.Instance.AddResource(resource, amountPerInterval);

                var pos = transform.position;
                pos.y = 1.75f;
                ResourcePopup.Create(pos, resource, amountPerInterval);
            }
        }
    }
}