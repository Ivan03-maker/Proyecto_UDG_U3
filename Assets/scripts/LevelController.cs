using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class LevelController : MonoBehaviour
{
    // Método para regresar al menú principal conservando los datos del GameManager
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}