using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using Input;
using JetBrains.Annotations;
using Pieces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int score = 0;
    [SerializeField]
    private Boolean _hasTurn = false;
    private Boolean _striked = false;
    private Boolean _scored = false;
    private List<Piece> _ownPieces;
    public Action<Boolean> turnFinished;
    public Action<Boolean> turnUpdated;

    [SerializeField] private Striker striker;
    [SerializeField] private BoardPosition boardPosition;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float maxMovement = 2.2f;
    [SerializeField] private float minMovement = -2.2f;
    
    private PositionInput _positionInput;
    private void Start()
    {
        _positionInput = GetComponent<PositionInput>();
        this.score = 0;
        _scored = false;
        _ownPieces =  new List<Piece>();
        UpdateScore(score);
        UpdateScoreColor(Color.gray);
        HandleMovement();
    }
    
    private void ResetStrikerToPlayerPos()
    {
        striker.SetPosition(transform.position);
        if (_positionInput != null)
            _positionInput.ResetPosition();
       

    }
    
    private void HandleMovement()
    {
        if (striker == null) return;
        _positionInput.onPositionChanged += vector3 =>
        {
            
            if (Striked) return;
            if (!HasTurn) return;
            var strikerPos = striker.transform.position;
            var newPos = strikerPos + vector3;
            switch (BoardPosition)
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

    private void UpdateScore(int score)
    {
        if (scoreText == null) return;
        scoreText.text = score.ToString();
    }


    public void AddPiece(Piece piece)
    {
        Debug.Log("Should Add Piece " + piece);
        if (piece != null)
        {
            AddPoints(piece.Points);
            _ownPieces.Add(piece);
            piece.gameObject.SetActive(false);
        }
    }
    
    public void AddPoints(int points)
    {
        _scored = true;
        score += points;
        UpdateScore(score);
    }

    public bool HasTurn
    {
        get => _hasTurn;
        set
        {
            _hasTurn = value;
            turnUpdated?.Invoke(value);
            
            if (_positionInput != null)
            {
                _positionInput.ShowSlider(_hasTurn);
            }
            
            if (value)
            {
                _scored = false;
                UpdateScoreColor(Color.white);
                _striked = false;
                ResetStrikerToPlayerPos();
            }
            else
            {
                UpdateScoreColor(Color.gray);
                turnFinished?.Invoke(_striked);
            }
        }
    }

    private void UpdateScoreColor(Color color)
    {
        if (scoreText == null) return;
        scoreText.color = color;
    }

    public bool Striked
    {
        get => _striked;
        set => _striked = value;
    }


    public bool Scored => _scored;

    public int Score => score;
    public BoardPosition BoardPosition => boardPosition;
}