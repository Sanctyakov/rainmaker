using UnityEngine;

// Pone personajes en el campo de batalla, determina según x criterio quién se mueve primero (por ahora, random).

public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab del jugador
    public GameObject enemyPrefab; // Prefab del enemigo
    public Transform playerSpawnPoint; // Punto de spawn del jugador
    public Transform enemySpawnPoint; // Punto de spawn del enemigo
    public Canvas battleMenu; // Canvas para mostrar la interfaz de batalla

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetRandomEncounter()
    {
   
    }

    void SetScriptedEncounter()
    {

    }

    void RollInitiative() 
    {
        // Lógica para determinar quién se mueve primero
        // Por ahora, simplemente selecciona al azar entre el jugador y el enemigo
        if (Random.value > 0.5f)
        {
            Debug.Log("El jugador se mueve primero.");
        }
        else
        {
            Debug.Log("El enemigo se mueve primero.");
        }
    }

    void SetPlayerTurn()
    {
        // Lógica para establecer el turno del jugador
        battleMenu.gameObject.SetActive(true); // Activa el menú de batalla para que el jugador pueda elegir su acción
    }

    void SetEnemyTurn()
    {
        // Lógica para establecer el turno del enemigo
        battleMenu.gameObject.SetActive(false); // Desactiva el menú de batalla durante el turno del enemigo
        // Aquí se podría agregar la lógica para que el enemigo tome su acción, como atacar al jugador o usar una habilidad
    }
}
