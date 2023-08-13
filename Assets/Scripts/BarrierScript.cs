using UnityEngine;

// Скрипт, отвечающий за удаление со сцены любых объектов, которые
// столкнулись с коллайдером объекта, к которому данный скрипт привязан.
public class BarrierScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
