using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private float direction = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Destruir la bala después de 3 segundos para liberar memoria
        Destroy(gameObject, 3f);
    }

    public void SetDirection(float dir)
    {
        direction = dir;
        // Si va hacia la izquierda, invertimos la escala en X de la bala
        if (dir < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }
}