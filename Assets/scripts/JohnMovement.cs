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

    // Variables para el control del ritmo de disparo (cadencia)
    private float lastShoot = 0f;
    public float fireRate = 0.25f; // Tiempo en segundos entre cada bala (ajusta este valor si quieres que dispare más rápido o más lento)

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

        // Disparar de forma automática o ráfaga al mantener presionada la tecla J respetando el fireRate
        if (Input.GetKey(KeyCode.J) && Time.time > lastShoot + fireRate)
        {
            Shoot();
            lastShoot = Time.time; // Actualiza el tiempo del último disparo
        }
    }

    void FixedUpdate()
    {
        // Aplicar velocidad horizontal manteniendo la física en Y
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
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