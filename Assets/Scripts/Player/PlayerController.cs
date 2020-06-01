using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using Input;
using Pieces;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PositionInput), typeof(StrikeInput))]
public class PlayerController : MonoBehaviour
{
    private PositionInput _positionInput;
    private StrikeInput _strikeInput;
    private Player _player;

    [SerializeField] private Striker striker;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CarromBoard carromBoard;
    [SerializeField] private float maxMovement = 2.2f;
    [SerializeField] private float minMovement = -2.2f;

    [SerializeField]
    private Player[] _players;
    [SerializeField]
    private Piece[] _pieces;
    private int _currentPlayerTurn = 0;
    void Start()
    {
        
        _positionInput = GetComponent<PositionInput>();
        _strikeInput = GetComponent<StrikeInput>();
        
        HandleMovement();
        HandlePullAndShoot();
    }

    private void OnEnable()
    {
        InitPlayers();
        InitCarromBoard();
    }


    public void SetCurrentPlayer(Player player)
    {
        if (_player != null)
        {
            _player.turnUpdated = null;
        }

        _player = player;
        _player.turnUpdated += b =>
        {
            if (b)
            {
                Debug.Log("ResetStrikerToPlayerPos " + _player.transform.name + " ----> " + _player.transform.position);
                ResetStrikerToPlayerPos(_player);
                if (_strikeInput != null)
                {
                    _strikeInput.AllowInput(true);
                }
            }
        };
        _player.HasTurn = true;
    }


    private void ResetStrikerToPlayerPos(Player player)
    {
        striker.SetPosition(player.transform.position);
        if (_positionInput != null)
            _positionInput.ResetPosition();
    }


    private IEnumerator CheckIfStrikerIsMoving()
    {
        while (striker.IsMoving())
        {
            yield return new WaitForSeconds(1);
        }

        if (_player.Striked && _player.HasTurn && !striker.IsMoving())
        {
            _player.HasTurn = false;
        }
    }

    private void HandlePullAndShoot()
    {
        _strikeInput.onShoot += force =>
        {
            if (_player.Striked) return;
            if (!_player.HasTurn) return;
            if (striker.IsMoving()) return;

            striker.Shoot(force);
            _player.Striked = true;
            StartCoroutine(CheckIfStrikerIsMoving());
        };
    }

    private void HandleMovement()
    {
        if (striker == null) return;
        _positionInput.onPositionChanged += vector3 =>
        {
            
            if (_player.Striked) return;
            if (!_player.HasTurn) return;
            var strikerPos = striker.transform.position;
            var newPos = strikerPos + vector3;
            switch (_player.BoardPosition)
            {
                case BoardPosition.POSTION_1:
                case BoardPosition.POSTION_3:
                {
                    striker.MoveTo(new Vector3(vector3.x, strikerPos.y, strikerPos.z));
                    break;
                }
                case BoardPosition.POSTION_2:
                case BoardPosition.POSTION_4:
                {
                    var posZ = Mathf.Clamp(newPos.y, minMovement, maxMovement);
                    striker.MoveTo(new Vector3(strikerPos.x, strikerPos.y, posZ));
                    break;
                }
            }
        };
    }
    
     private void InitCarromBoard()
    {
        if (carromBoard == null) return;

        carromBoard.onScorePoints += piece => { _players[_currentPlayerTurn].AddPiece(piece); };
        carromBoard.onFoul += piece => { };
    }

    private void InitPlayers()
    {
       
        UpdatePlayerTurn();
        foreach (var player in _players)
        {
            Debug.Log("Player " + player.transform.name + " - " + player.transform.position);
            player.turnFinished += finished =>
            {
                if (finished)
                {
                    if (!IsGameOver())
                    {
                        StartCoroutine(CheckIfPiecesAreMoving());
                    }
                    else
                    {
                        ShowPlayerScore();
                    }
                }
            };
        }
    }

    void ShowPlayerScore()
    {
        if (uiManager == null) return;
        uiManager.ShowGameOver(true);
        var scoreText = "";
        foreach (var player in _players)
        {
            scoreText += player.transform.name + " : " + player.Score + "\n";
        }

        scoreText = scoreText.Trim();
        if (uiManager != null)
            uiManager.ShowScore(scoreText);
    }

    private IEnumerator CheckIfPiecesAreMoving()
    {
        if (ArePiecesMoving())
        {
            yield return new WaitForSeconds(1);
        }

        //Give another player turn
        NextTurn();
        UpdatePlayerTurn();
    }

    private void UpdatePlayerTurn()
    {
        if (_players == null)
        {
            Debug.Log("Player Are null");
            return;
        }
        SetCurrentPlayer(_players[_currentPlayerTurn]);
    }

    private Boolean ArePiecesMoving()
    {
        foreach (var piece in _pieces)
        {
            if (piece.IsMoving()) return true;
        }

        return false;
    }

    private Boolean IsGameOver()
    {
        var pieces = FindObjectsOfType<Piece>();
        if (pieces.Length == 1) return true;
        foreach (var piece in pieces)
        {
            if (!piece.IsQueen)
            {
                return false;
            }
        }
        return true;
    }


    private void NextTurn()
    {
        var currentPlayer = _players[_currentPlayerTurn];
        if (currentPlayer.Scored)
        {
            return;
        }

        if (_currentPlayerTurn >= _players.Length - 1)
        {
            _currentPlayerTurn = 0;
            return;
        }

        _currentPlayerTurn++;
    }
}