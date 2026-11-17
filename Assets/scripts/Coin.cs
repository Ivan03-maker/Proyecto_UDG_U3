using UnityEngine;

public class Coin : MonoBehaviour
{
    // Se ejecuta automáticamente cuando el jugador entra en el área de la moneda
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que chocó tiene la etiqueta "Player" (o el componente JohnMovement)
        if (collision.CompareTag("Player") || collision.GetComponent<JohnMovement>() != null)
        {
            // Aquí sumamos la moneda al contador global (crearemos este método en el GameManager o JohnMovement)
            GameManager.instance.AddCoin();

            // Destruimos la moneda de la escena
            Destroy(gameObject);
        }
    }
}