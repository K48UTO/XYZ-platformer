using UnityEngine;

namespace Scripts.Components
{
    public class ParentingControlComponent : MonoBehaviour
    {
        // Метод для установления родительской связи
        public void SetAsParent(GameObject child)
        {
            if (child != null)
            {
                // Устанавливаем этот объект в качестве родителя для входящего объекта
                child.transform.SetParent(transform);
            }
        }

        // Метод для разрыва родительской связи
        public void RemoveParent(GameObject child)
        {
            if (child != null && child.transform.parent == transform)
            {
                // Убираем родительскую связь, возвращая объект в корневую иерархию
                child.transform.SetParent(null);
            }
        }
    }
}
