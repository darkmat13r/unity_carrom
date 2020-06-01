using System;
using System.Collections;
using System.Collections.Generic;
using Pieces;
using UnityEngine;

public class Striker : Piece
{
    private Vector3 _lastPosition;

    [SerializeField] private float moveSpeed = 0.5f;

    [SerializeField] private AudioClip _pieceHitClip;
    private bool hitClipPlayed = false;

    public void MoveTo(Vector3 position)
    {
        var currentPosition = transform.position;
        currentPosition = Vector3.Lerp(currentPosition, position , Time.deltaTime * moveSpeed);
        transform.position = currentPosition;
    }


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.identity;
    }


    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.collider.CompareTag("Piece") && !hitClipPlayed)
        {
            _audioSource.PlayOneShot(_pieceHitClip);
            hitClipPlayed = true;
            StartCoroutine(ResetHitClipPlayed());
        }


    }

    IEnumerator ResetHitClipPlayed()
    {
        yield return new WaitForSeconds(2);
        hitClipPlayed = false;
    }


    public void Shoot(Vector3 force)
    {
        Debug.Log("Shoot");
        Rigidbody.AddForce(force, ForceMode.Impulse);
    }
}