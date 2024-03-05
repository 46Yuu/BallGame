using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager instance;
    private List<PlayerInput> _players = new List<PlayerInput>();
    private PlayerInputManager _playerInputMan;
    [SerializeField] private int _playersRequired = 2;
    [SerializeField] private PlayerInput _playerOne;

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
        _playerInputMan = GetComponent<PlayerInputManager>();
    }


    void Update()
    {
/*        if (_playerOne != null)
            if (_playerOne.actions.FindActionMap("PlayerDanceMoves").FindAction("StartGame").ReadValue<float>() > 0 && !GameManager.Instance.GetGameStart())
            {
                //GameManager.Instance.SetGameStart(true);
                _playerInputMan.DisableJoining();
            }*/
    }
    void AddPlayer(PlayerInput playerInput)
    {
        _players.Add(playerInput);
        if (_playerOne == null)
            _playerOne = playerInput;
        _playersRequired -= 1;
        //UpdatePlayerLeft();
        //Transform playerParent = playerInput.transform;
        //SetPlayerColor(playerInput);
        //GameManager.Instance.SetPlayerStartPos(playerInput.transform, playerInput.playerIndex);
        //GameManager.Instance.SetCurrentPlayer();
        //playerInput.GetComponent<PlayerController>()._playerLifeGO = _tmpLife[playerInput.playerIndex];
        //_targetGroup.AddMember(playerInput.transform, 1f, 1f);
    }

    private void OnEnable()
    {
        _playerInputMan.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        _playerInputMan.onPlayerJoined -= AddPlayer;
    }
}
