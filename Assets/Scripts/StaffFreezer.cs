using UnityEngine;

// Скрипт, отвечающий за привязку объекта к указанной движущейся декорации
public class StaffFreezer : MonoBehaviour
{
    // Физический компонент для привязки
    public Rigidbody2D AttachedRigidbody;
    // Флаг необходимости осуществления корректировки положения по оси У
    public bool MakeCorrection = false;
    // Значение корректировки положения по оси У
    public float CorrectionValue = 0.04f;

    // Функция, вызываемая при столкновении коллайдера данного объекта
    // с коллайдером другого объекта
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (AttachedRigidbody != null)
        {
            // Добавления компонента для связки текущего объекта
            // с указанной декорацией
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = AttachedRigidbody;

            // Выключение всех коллайдеров текущего объекта
            foreach (Collider2D col in gameObject.GetComponents<Collider2D>())
            {
                col.enabled = false;
            }

            // Осуществление корректировки по оси У при необходимости
            if (MakeCorrection == true)
            {
                transform.position += new Vector3(0f, CorrectionValue, 0f);
            }
        }
    }
}
