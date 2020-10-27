using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    protected bool alive;
    protected int health;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}