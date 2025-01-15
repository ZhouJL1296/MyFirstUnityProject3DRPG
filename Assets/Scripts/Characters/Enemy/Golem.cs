using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyControler
{
    [Header("Skill")]
    public float kickForce = 25;
    public GameObject rockPrefab;
    public Transform handPos;
    
    // Animation Event
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            // transform.LookAt(attackTarget.transform);

            var direction = (attackTarget.transform.position - transform.position).normalized;
            // direction.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

            // 根据个人喜好添加以下代码
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    // Animation Event
    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
