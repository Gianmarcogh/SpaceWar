using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TextOutputHandler(string text);

public class GameSceneController : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(5,20)]
    public float playerSpeed;

    [Header("Screen Settings")]
    [Space]
    public Vector3 screenBounds;

    [Header("Prefab")]
    [SerializeField]
    private EnemyController enemyPrefab;
    private HUDController hUDController;
    private int totalPoints;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        hUDController = FindObjectOfType<HUDController>();
        player = FindObjectOfType<PlayerController>();
        playerSpeed = 10;
        screenBounds = GetScreenBounds();
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {     
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SetColor(Color.red);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.SetColor(Color.yellow);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(2);

        while(true)
        {
            float horizontalPosition = Random.Range(-screenBounds.x, screenBounds.x);
            Vector2 spawnPosition = new Vector2(horizontalPosition, screenBounds.y);

            EnemyController enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            enemy.EnemyEscaped += EnemyAtBottom;
            enemy.EnemyKilled += EnemyKilled;

            yield return wait;
        }
    }

    private Vector3 GetScreenBounds()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z);

        return mainCamera.ScreenToWorldPoint(screenVector);
    }

    private void EnemyAtBottom(EnemyController enemy)
    {
        Destroy(enemy.gameObject);
        Debug.Log("Enemy Escaped");
    }

    private void EnemyKilled(int pointsValue)
    {
        totalPoints += pointsValue;
        hUDController.scoreText.text = totalPoints.ToString();
    }

    public void KillObject(IKillable killable)
    {
        killable.Kill();
    }

    public void OutputText(string output)
    {
        Debug.LogFormat("{0} output by GameSceneController", output);
    }
}
