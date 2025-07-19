using MG_Utilities;
using System.Collections.Generic;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    public class Coin1 : MonoBehaviour
    {
        [SerializeField] public List<TowerBundle.ResourceCost> _resources;
        private PlayerResourceInventory _inventory;
        private Collider _collider;

        [SerializeField] int _minResourceAmount;
        [SerializeField] int _maxResourceAmount;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _inventory = PlayerResourceInventory.Instance;
            _collider = GetComponent<Collider>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.transform.root.name);
            if (other.transform.root.CompareTag("Player"))
            {
                int option = Random.Range(0, 3);
                int resourceAmount = Random.Range(_minResourceAmount, _maxResourceAmount);

                switch (option)
                {
                    case 0:
                        _inventory.AddResource(_resources[0].resource, resourceAmount);
                        CreatePopup(_resources[0].resource, resourceAmount);
                        break;
                    case 1:
                        _inventory.AddResource(_resources[1].resource, resourceAmount);
                        CreatePopup(_resources[0].resource, resourceAmount);
                        break;
                    case 2:
                        int resourceAmount2 = Random.Range(_minResourceAmount, _maxResourceAmount);
                        _inventory.AddResource(_resources[0].resource, resourceAmount);
                        CreatePopup(_resources[0].resource, resourceAmount);
                        _inventory.AddResource(_resources[1].resource, resourceAmount2);
                        CreatePopup(_resources[1].resource, resourceAmount, 2.25f);
                        break;
                }

                Destroy(gameObject);
            }
        }

        private void CreatePopup(ResourceSO resourceType, int resourceAmount, float y = 1.75f)
        {
            ResourcePopup.Create(_collider.transform.position.With(y: y), resourceType, resourceAmount);
        }

    }
}
