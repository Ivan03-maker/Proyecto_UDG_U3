using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private bool grounded;

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

        // Voltear el sprite según la dirección en la que se mueva
        if (horizontal < 0.0f) transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal > 0.0f) transform.localScale = new Vector3(1, 1, 1);

        // Detectar salto con la tecla W o Espacio cuando esté en el suelo
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Aplicar velocidad horizontal manteniendo la física en Y
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        // Comprobación de suelo mejorada (bajando ligeramente el origen del rayo o usando un offset)
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.2f);
        grounded = (hit.collider != null);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}