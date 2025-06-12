using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    public class ResourcePopup : MonoBehaviour
    {
        [SerializeField] float goingUpSpeed = 2f;
        float _timeToLive;
        public static void Create(Vector3 position, ResourceSO resourceType, int resourceAmount, float timeToLive = 2f)
        {
            ResourcePopup prefab = GameAssetsManager.Instance.ResourcePopupPrefab;
            Assert.IsNotNull(prefab, "ResourcePopup prefab is not set!");
            
            var instance = Instantiate(prefab, position, Quaternion.identity);
            instance.Init(resourceType, resourceAmount, timeToLive);
        }

        void Init(ResourceSO resourceType, int resourceAmount, float ttl)
        {
            GetComponent<TextMeshPro>().SetText($"{resourceAmount}");
            GetComponentInChildren<Image>().sprite = resourceType.Sprite;
            _timeToLive = ttl;
        }

        void Update()
        {
            _timeToLive -= Time.deltaTime;
            transform.position += Vector3.up * (Time.deltaTime * goingUpSpeed);
            if(_timeToLive <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}