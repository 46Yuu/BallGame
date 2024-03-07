using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager instance;
    [SerializeField] public List<PlayerInput> _players = new List<PlayerInput>();
    public PlayerInputManager playerInputMan;
    [SerializeField] public int _playersRequired = 2;
    [SerializeField] private PlayerInput _playerOne;
    public GameObject[] _tmpLife;
    [SerializeField] private TextMeshProUGUI _playerLeft;
    [SerializeField] private Color[] _colorPlayer;

    #region Singleton
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerManager>();
            }
            return instance;
        }
    }
    #endregion
    private void Awake()
    {
        playerInputMan = GetComponent<PlayerInputManager>();
        // UpdatePlayerLeft();
    }

    private void Update()
    {
        if (_players.Count == _playersRequired)
        {
            playerInputMan.DisableJoining();
        }
    }
    void AddPlayer(PlayerInput playerInput)
    {
        if (_players.Count <= _playersRequired)
        {
            _players.Add(playerInput);
            switch (_players.Count)
            {
                case 1:
                    playerInput.gameObject.GetComponent<PlayerController>().myNumber = PlayerController.PlayerNbr.Player_1;
                    break;
                case 2:
                    playerInput.gameObject.GetComponent<PlayerController>().myNumber = PlayerController.PlayerNbr.Player_2;
                    break;
                case 3:
                    playerInput.gameObject.GetComponent<PlayerController>().myNumber = PlayerController.PlayerNbr.Player_3;
                    break;
                case 4:
                    playerInput.gameObject.GetComponent<PlayerController>().myNumber = PlayerController.PlayerNbr.Player_4;
                    break;
            }
            if (_playerOne == null)
            {
                _playerOne = playerInput;
            }
            _players[_players.Count - 1].gameObject.transform.position = UIManager.GetInstance().listPlateform[_players.Count - 1].transform.position;
        }

    }
    private void OnEnable()
    {
        playerInputMan.onPlayerJoined += AddPlayer;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "SelectScene")
        {
            UIManager.GetInstance().Init();
        }
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            GameController.GetInstance().Init();
            foreach (PlayerInput player in _players)
            {
                player.gameObject.GetComponent<PlayerController>().Init();
            }
        }
    }
    private void OnDisable()
    {
        playerInputMan.onPlayerJoined -= AddPlayer;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void UpdatePlayerLeft()
    {
        if (_playersRequired > 0 && _playersRequired <= 2)
            _playerLeft.text = "At least " + _playersRequired + "other players to start the game, up to 4 !";
        else
            _playerLeft.text = "Player 1 needs to press START to start the game !";
    }
    public void SetPlayerLeft() => _playerLeft.text = "";

    public List<PlayerInput> GetPlayerInput() => _players;
}
