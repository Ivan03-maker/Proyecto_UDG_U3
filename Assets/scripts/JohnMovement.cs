using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 10f;
    public GameObject bulletPrefab; 
    public Transform firePoint;     

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private bool grounded;
    private float lastDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Capturar movimiento horizontal (A/D)
        horizontal = Input.GetAxisRaw("Horizontal");

        // Animación de correr
        animator.SetBool("running", horizontal != 0.0f);

        // Voltear sprite y dirección
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

        // Detectar salto con la tecla W cuando esté en el suelo
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            Jump();
        }

        // Disparar con la tecla J
        if (Input.GetKeyDown(KeyCode.J))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // Aplicar velocidad horizontal manteniendo la física en Y
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        // Controlar la animación de salto según la velocidad vertical (Y)
        // Si va hacia arriba o hacia abajo en el aire, activamos "jumping"
        if (!grounded && rb.linearVelocity.y != 0)
        {
            animator.SetBool("jumping", true);
        }
        else
        {
            animator.SetBool("jumping", false);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        grounded = false; 
    }

    // Detectar cuando John está tocando el suelo
    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D hit in collision.contacts)
        {
            if (hit.normal.y > 0.5f)
            {
                grounded = true;
                return;
            }
        }
    }

    // Detectar cuando John se separa del suelo
    void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (bulletController != null)
            {
                bulletController.SetDirection(lastDirection);
            }
        }
    }
}