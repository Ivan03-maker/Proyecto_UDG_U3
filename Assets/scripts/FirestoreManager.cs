using UnityEngine;

using UnityEngine.UI;

using Firebase;

using Firebase.Firestore;

using Firebase.Extensions;

using System.Collections.Generic;

using UnityEngine.SceneManagement;



public class FirestoreManager : MonoBehaviour

{

public static FirestoreManager instance;

private FirebaseFirestore db;

private bool firebaseInitialized = false;



void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Blindaje para evitar cierres repentinos al iniciar el .exe
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    else
    {
        Destroy(gameObject);
        return;
    }
}



void Start()

{

FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {

if (task.Result == DependencyStatus.Available)

{

db = FirebaseFirestore.DefaultInstance;

firebaseInitialized = true;

Debug.Log("Firebase Firestore inicializado correctamente.");

LoadGameData();

}

else

{

Debug.LogError("No se pudieron resolver las dependencias de Firebase: " + task.Result);

}

});

}



// Registramos los eventos de cambio de escena

void OnEnable()

{

SceneManager.sceneLoaded += OnSceneLoaded;

}



void OnDisable()

{

SceneManager.sceneLoaded -= OnSceneLoaded;

}



// Cada vez que cargamos una escena, vinculamos automáticamente el botón de guardar

void OnSceneLoaded(Scene scene, LoadSceneMode mode)

{

GameObject saveButtonObj = GameObject.Find("SaveButton");

if (saveButtonObj != null)

{

Button btn = saveButtonObj.GetComponent<Button>();

if (btn != null)

{

btn.onClick.RemoveAllListeners();

btn.onClick.AddListener(() => {

Debug.Log("¡Clic detectado en el botón de Guardar!");

SaveGameData();

});

Debug.Log("¡Botón 'SaveButton' vinculado exitosamente por código en la escena: " + scene.name + "!");

}

}

}



void Update()

{

// Conservamos la tecla S como respaldo opcional

if (Input.GetKeyDown(KeyCode.S))

{

Debug.Log("¡Tecla 'S' presionada! Sincronizando y sumando progreso...");

SaveGameData();

}

}



// Método para GUARDAR y SUMAR las monedas en la nube de forma acumulativa con aviso al banner
public void SaveGameData()
{
    if (db == null)
    {
        try { db = FirebaseFirestore.DefaultInstance; } catch { }
    }

    if (db == null)
    {
        Debug.LogWarning("Firestore no está listo todavía.");
        return;
    }

    DocumentReference docRef = db.Collection("player_progress").Document("jugador_1");

    docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
        int cloudCoins = 0;

        if (task.IsCompleted && task.Result != null && task.Result.Exists)
        {
            Dictionary<string, object> data = task.Result.ToDictionary();
            if (data.ContainsKey("totalCoins"))
            {
                int.TryParse(data["totalCoins"].ToString(), out cloudCoins);
            }
        }

        int finalTotalCoins = cloudCoins + GameManager.instance.totalCoins;

        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
            { "totalCoins", finalTotalCoins },
            { "currentLevel", GameManager.instance.currentLevel }
        };

        docRef.SetAsync(playerData).ContinueWithOnMainThread(saveTask => {
            if (saveTask.IsCompleted)
            {
                Debug.Log("¡Progreso acumulado y guardado en Firebase! Total en la nube: " + finalTotalCoins);

                // Verificación segura para activar el banner solo si estás en el menú principal
                if (SceneManager.GetActiveScene().name == "MainMenuScene")
                {
                    MainMenuController menuController = FindObjectOfType<MainMenuController>();
                    if (menuController != null)
                    {
                        menuController.ShowSaveBannerExternal("¡Progreso guardado en la nube con éxito!");
                    }
                }
            }
            else
            {
                Debug.LogError("Error al guardar en Firestore: " + saveTask.Exception);
            }
        });
    });
}



// Método para CARGAR los datos al iniciar

public void LoadGameData()

{

if (db == null)

{

try { db = FirebaseFirestore.DefaultInstance; } catch { return; }

}



DocumentReference docRef = db.Collection("player_progress").Document("jugador_1");



docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {

if (task.IsCompleted && task.Result != null)

{

DocumentSnapshot snapshot = task.Result;

if (snapshot.Exists)

{

Dictionary<string, object> data = snapshot.ToDictionary();


if (data.ContainsKey("totalCoins"))

{

GameManager.instance.totalCoins = int.Parse(data["totalCoins"].ToString());

}

if (data.ContainsKey("currentLevel"))

{

GameManager.instance.currentLevel = int.Parse(data["currentLevel"].ToString());

}



GameManager.instance.UpdateCoinUI();

Debug.Log("¡Progreso cargado desde Firebase Firestore!");

}

}

});

}


}

