using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena
using TMPro; // Necesario para usar TextMeshPro

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI menuCoinText; // Arrastra aquí tu texto del menú en el Inspector

    void Start()
    {
        // Si el GameManager existe, muestra las monedas acumuladas en la UI del menú
        if (GameManager.instance != null && menuCoinText != null)
        {
            menuCoinText.text = "Puntuación Total: " + GameManager.instance.totalCoins;
        }
    }

    // Método que se llamará al hacer clic en el botón Jugar
    public void PlayGame()
    {
        // Carga la escena del nivel de juego
        SceneManager.LoadScene("Escena1");
    }

    // Método para salir del juego (opcional)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}