using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int totalCoins = 0;
    public int currentLevel = 1;
    public TextMeshProUGUI coinText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        UpdateCoinUI();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Limpiamos la referencia anterior para evitar MissingReferenceException
        coinText = null;

        // Buscamos el texto de monedas si estamos en una escena de nivel
        GameObject textObj = GameObject.Find("CoinText");
        if (textObj != null)
        {
            coinText = textObj.GetComponent<TextMeshProUGUI>();
        }

        UpdateCoinUI();
    }

    public void AddCoin()
    {
        totalCoins++;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Monedas: " + totalCoins;
        }

        // Actualizamos también el texto del menú principal si existe en la escena actual
        GameObject menuTextObj = GameObject.Find("MenuCoinText");
        if (menuTextObj != null)
        {
            TextMeshProUGUI menuText = menuTextObj.GetComponent<TextMeshProUGUI>();
            if (menuText != null)
            {
                menuText.text = "Puntuación Total: " + totalCoins;
            }
        }
    }
}