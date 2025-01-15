using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockState { HitPlayer, HitEnemy, HitNothing };
    public Rigidbody rb;
    public RockState rockState;

    [Header("Basic Settings")]
    public float force;
    public int damage;
    public GameObject target;
    private Vector3 direction;
    public GameObject breakEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;

        rockState = RockState.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
        {
            rockState = RockState.HitNothing;
        }
    }

    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerControler>().gameObject;

        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (rockState)
        {
            case RockState.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());

                    rockState = RockState.HitNothing;
                }
                break;

            case RockState.HitEnemy:
                // if (other.gameObject.GetComponent<Golem>())
                if (other.gameObject.CompareTag("Enemy"))
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
}
