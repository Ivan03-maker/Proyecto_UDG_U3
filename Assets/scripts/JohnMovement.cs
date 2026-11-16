using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 10f;
    public GameObject bulletPrefab; // Arrastra aquí el prefab de la bala desde el Inspector
    public Transform firePoint;     // Arrastra aquí el FirePoint

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private bool grounded;
    private float lastDirection = 1f; // 1 para derecha, -1 para izquierda

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Capturar input horizontal de teclado (A/D o flechas)
        horizontal = Input.GetAxisRaw("Horizontal");

        // Activar o desactivar la animación de correr en el Animator
        animator.SetBool("running", horizontal != 0.0f);

        // Voltear el sprite y actualizar la última dirección de disparo
        if (horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            lastDirection = -1f;
        }
        else if (horizontal > 0.0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            lastDirection = 1f;
        }

        // Detectar salto con la tecla W o Espacio cuando esté en el suelo
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            Jump();
        }

        // Detectar evento de teclado para disparar (Ejemplo: Tecla J o K, o Botón Fire1 / Espacio)
        if (Input.GetKeyDown(KeyCode.J))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // Aplicar velocidad horizontal manteniendo la física en Y
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        // Comprobación de suelo mediante un rayo adaptado a los pies
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.2f);
        grounded = (hit.collider != null);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // Instanciar la bala en la posición del FirePoint
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            
            // Asignar la dirección en la que debe viajar la bala según hacia dónde mira John
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (bulletController != null)
            {
                bulletController.SetDirection(lastDirection);
            }
        }
    }
}