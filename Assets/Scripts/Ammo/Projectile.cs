using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 target;
    public Transform targetObject;
    float speed = 10;
    public Action<Projectile> onHitTarget;
    public Damage damageInfo;
    
    void Update()
    {
        if (target == null || onHitTarget == null || targetObject == null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        else if (targetObject != null)
        {

            target = targetObject.position;

        }

        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetObject.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.localPosition, target) < 0.01f)
        {
            if (targetObject != null)
            {
                onHitTarget(this);
            }
            
            GameObject.Destroy(gameObject);
        }
    }
}

