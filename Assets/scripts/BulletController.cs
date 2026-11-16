using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private float direction = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Destruir la bala después de 3 segundos por seguridad si no choca con nada
        Destroy(gameObject, 3f);
    }

    public void SetDirection(float dir)
    {
        direction = dir;
        if (dir < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    // Se activa cuando la bala entra en contacto con el colisionador de una pared u otro objeto
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con algo que NO sea el jugador (John), la bala se destruye al instante
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}