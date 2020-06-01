using System;
using System.Collections;
using System.Collections.Generic;
using Pieces;
using UnityEngine;

public class CarromHole : MonoBehaviour
{
    private AudioSource _audioSource;
    public Action<Piece> onPieceEnterHole;
    public Action<Striker> onStrikerEnterHole;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piece"))
        {
            var piece = other.gameObject.GetComponent<Piece>();
            if (piece != null && onPieceEnterHole != null)
            {
                onPieceEnterHole(piece);
                _audioSource.PlayOneShot(_audioSource.clip);
            }
        }
        if (other.CompareTag("Striker"))
        {
            var piece = other.gameObject.GetComponent<Striker>();
            if (piece != null && onStrikerEnterHole != null)
            {
                onStrikerEnterHole(piece);
            }
        }
    }
}
