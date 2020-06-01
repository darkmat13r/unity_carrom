using System;
using UnityEngine;

namespace Pieces
{
    public class Piece : MonoBehaviour
    {
        [SerializeField] protected int points = 10;
        [SerializeField] protected bool isQueen = false;

        protected Rigidbody Rigidbody;
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        protected AudioSource _audioSource;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            _audioSource = GetComponent<AudioSource>();
        }

        public virtual Boolean IsMoving()
        {
            return !Rigidbody.IsSleeping();
        }


        private void Update()
        {
            /*RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down * 120f, out hit))
            {
                if (!hit.collider.CompareTag("Rail") )
                {
                    transform.position = _startPosition;
                    transform.rotation = _startRotation;
                }
            }*/
        }


        public int Points => points;

        public bool IsQueen => isQueen;


        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Rail"))
            {
               _audioSource.PlayOneShot(_audioSource.clip);
            }
        }
    }
}