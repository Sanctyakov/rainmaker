using UnityEngine;

// Pone personajes en el campo de batalla, determina según x criterio quién se mueve primero (por ahora, random).

//- AttackComponent: Determina que un objeto puede atacar a otro.
//- PlayerController: Mueve un personaje jugable según input.
//- EnemyController: Elige una acción dependiendo de un behavior tree.

public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab del jugador
    public GameObject enemyPrefab; // Prefab del enemigo
    public Transform playerSpawnPoint; // Punto de spawn del jugador
    public Transform enemySpawnPoint; // Punto de spawn del enemigo

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
}
