using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Health;
    public GameObject Outline;

    public void Damage(int damage) {
        Health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage");
    }

    void OnMouseEnter() {
        Outline?.SetActive(true);
    }
    void OnMouseExit() {
        Outline?.SetActive(false);
    }
}
