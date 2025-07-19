using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace EternalDefenders
{
    public class DamagePopupText : MonoBehaviour
    {
        [SerializeField] float goingUpSpeed = 2f;
        float _timeToLive;
        public static void Create(Vector3 position, int damage, float timeToLive = 2f)
        {
            DamagePopupText prefab = GameAssetsManager.Instance.damagePopupTextPrefab;
            Assert.IsNotNull(prefab, "DamagePopupText prefab is not set!");
            
            var instance = Instantiate(prefab, position, Quaternion.identity);
            instance.Init(damage, timeToLive);
        }

        void Init(int damage, float ttl)
        {
            GetComponent<TextMeshPro>().SetText(damage.ToString());
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