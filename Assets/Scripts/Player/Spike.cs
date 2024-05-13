using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public PlayerStats ps;
    float Sp = 10.0f;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            ps.TakeDamage(Sp);
        }
    }
}