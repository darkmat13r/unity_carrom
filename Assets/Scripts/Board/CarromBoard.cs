using System;
using System.Collections;
using System.Collections.Generic;
using Pieces;
using UnityEngine;

public class CarromBoard : MonoBehaviour
{
    private CarromHole[] carromHoles;

    public Action<Piece> onScorePoints;  
    public Action<Piece> onFoul;  

    private void Start()
    {
        carromHoles = GetComponentsInChildren<CarromHole>();
        InitCarronHoles();
    }

    private void InitCarronHoles()
    {
        foreach (var carromHole in carromHoles)
        {
            carromHole.onPieceEnterHole += piece =>
            {
                if (piece != null && onScorePoints != null)
                {
                    onScorePoints(piece);
                }
            };
            carromHole.onStrikerEnterHole += striker =>
            {
                if (striker != null && onFoul != null)
                {
                    onFoul(striker);
                }
            };
        }
    }
}
