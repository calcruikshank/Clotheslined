using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    int playerID = 0;
    [SerializeField] GameObject p1Spawn, p2Spawn;
    RopeAttach ropeattach;
    PlayerInputManager pim;
    [SerializeField] GameObject p1Prefab, p2Prefab;
    public int numOfZombiesKilled;
    public List<PlayerController> playersList = new List<PlayerController>();
    ZombieSpawner zombSpawner;
    PlayerInput player1, player2;
    public bool gameOver = false;
    public int numOfZombsOnScreen;
    public bool gameHasStarted;
    public float distanceBetweenPlayers;
    [SerializeField] TextMeshProUGUI scoreText, highScoreText, startText;
    public int score;

    [SerializeField] GameObject comboText;
    float comboTimer = 0f;
    float comboThreshold = 1f;
    Canvas canvas;
    public int comboCount;
    AudioManager audioManager;

    private void Awake()
    {

        pim = FindObjectOfType<PlayerInputManager>();
        ropeattach = FindObjectOfType<RopeAttach>();
        zombSpawner = FindObjectOfType<ZombieSpawner>();
        SpawnPlayersOnKeyboard();
        canvas = FindObjectOfType<Canvas>();
        comboTimer = 0f;
        comboCount = 1;
        audioManager = FindObjectOfType<AudioManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = ("High Score: " + PlayerPrefs.GetInt("HighScore").ToString());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ("Score: " + score);
        comboTimer += Time.deltaTime;
        if (playersList.Count >= 2)
        {
            distanceBetweenPlayers = (playersList[0].transform.position - playersList[1].transform.position).magnitude;
        }
        if (gameHasStarted) return;
        if (distanceBetweenPlayers >= 15f && !gameHasStarted)
        {
            startText.enabled = false;
            zombSpawner.StartSpawningZombs();
        }
    }
    public void AddPlayer(PlayerController player)
    {
        if (playerID == 0)
        {
            //pim.playerPrefab = p2Prefab;
            player.transform.position = p1Spawn.transform.position;
            ropeattach.Attach(playerID, player.transform);
            Destroy(p1Spawn);
        }
        if (playerID == 1)
        {
            //pim.playerPrefab = p2Prefab;
            player.transform.position = p2Spawn.transform.position;
            ropeattach.Attach(playerID, player.transform);

            pim.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            Destroy(p2Spawn);
        }

        playersList.Add(player);
        playerID++;

    }

    public List<PlayerController> GetPlayersList()
    {
        return playersList;
    }

    public void ResetGame()
    {
        StartCoroutine(ReloadGameScene(3f));
    }

    IEnumerator ReloadGameScene(float secs)
    {
        yield return new WaitForSeconds(secs);
        SceneManager.LoadScene("Main");
    }


    void SpawnPlayersOnKeyboard()
    {

        player1 = PlayerInput.Instantiate(p1Prefab, controlScheme: "wasd", pairWithDevice: Keyboard.current);
        player2 = PlayerInput.Instantiate(p2Prefab, controlScheme: "arrowkeys", pairWithDevice: Keyboard.current);
    }

    public void CheckForHighScore()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore");
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = ("High Score: " + PlayerPrefs.GetInt("HighScore").ToString());
        }
    }

    public void DisplayDamageText(int sentComboCount, Transform zombieDying)
    {
        if (canvas != null)
        {
            GameObject txt = Instantiate(comboText, new Vector3(zombieDying.transform.position.x, zombieDying.transform.position.y + 3, zombieDying.transform.position.z), canvas.transform.rotation, canvas.transform);
            txt.GetComponent<ComboTextBehaviour>().Setup(sentComboCount);
        }

    }

    public void ZombieDeath(ZombieAI zomb)
    {
        audioManager.Play("Pop");
        numOfZombiesKilled++;
        numOfZombsOnScreen--;
        if (comboTimer <= comboThreshold)
        {
            comboCount++;
            DisplayDamageText(comboCount, zomb.transform);
        }
        else
        {
            comboCount = 1;
        }
        comboTimer = 0f;
        score += comboCount;
    }

}
