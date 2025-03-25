using UnityEngine;

namespace EternalDefenders
{
    public class ItemTestInput : MonoBehaviour
    {
        void Update()
        {

            for (var i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
            {
                if (Input.GetKeyDown(i))
                {
                    ItemManager.Instance.AddItemByID(i - KeyCode.Alpha0);
                }
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                ItemManager.Instance.UseActiveItem();
            }

        }
    }
}
