using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // Necesario para las corrutinas (temporizador del banner)



public class MainMenuController : MonoBehaviour

{

public TextMeshProUGUI menuCoinText;
public TextMeshProUGUI saveStatusText; // Arrastra aquí tu nuevo texto de estado en el Inspector



	void Start()
	{

	if (GameManager.instance != null && menuCoinText != null)

	{

	menuCoinText.text = "Puntuación Total: " + GameManager.instance.totalCoins;
	}



	// Asegúrate de que el mensaje de guardado comience oculto o vacío

	if (saveStatusText != null)

	{

	saveStatusText.text = "";

	}

	}



	public void PlayGame()

	{

	SceneManager.LoadScene("Escena1");

	}



// Método que se ejecuta cuando el jugador presiona el botón de guardar

	public void TriggerSaveData()

	{

	if (GameManager.instance != null)

	{

	// Llama a tu función de guardado existente en el GameManager / FirestoreManager

	// GameManager.instance.SaveDataToFirebase();



	// Muestra el banner visual de éxito

	StartCoroutine(ShowSaveBanner("¡Progreso guardado en la nube con éxito!"));

	}

	else

	{

	StartCoroutine(ShowSaveBanner("Error: No se encontró el GameManager"));

	}

	}



	// Corrutina para mostrar el mensaje y ocultarlo automáticamente después de 3 segundos

	private IEnumerator ShowSaveBanner(string message)

	{

	if (saveStatusText != null)

	{

	saveStatusText.text = message;

	yield return new WaitForSeconds(3f); // Espera 3 segundos en pantalla

	saveStatusText.text = ""; // Limpia el texto

	}

	}



	public void QuitGame()

	{

	Debug.Log("Cerrando el juego...");

	Application.Quit();



	#if UNITY_EDITOR

	UnityEditor.EditorApplication.isPlaying = false;

	#endif

	}
	
	public void ShowSaveBannerExternal(string message)
{
    if (this != null && gameObject.activeInHierarchy)
    {
        StartCoroutine(ShowSaveBanner(message));
    }
}

}