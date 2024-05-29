using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    PlayerStats ps;
    [SerializeField] float Sp = 5.0f;

    private void Awake()
    {
        ps = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        ps = FindAnyObjectByType<PlayerStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ps.TakeDamage(Sp);

        }
    }
}