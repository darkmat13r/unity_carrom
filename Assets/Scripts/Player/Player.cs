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

    [SerializeField] private BoardPosition boardPosition;
    [SerializeField] private TextMeshProUGUI scoreText;
    private PositionInput _positionInput;
    private void Start()
    {
        _positionInput = GetComponent<PositionInput>();
        this.score = 0;
        _scored = false;
        _ownPieces =  new List<Piece>();
        UpdateScore(score);
        UpdateScoreColor(Color.gray);
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
            if (value)
            {
                _scored = false;
                UpdateScoreColor(Color.white);
                _striked = false;
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