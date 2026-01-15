using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject HitboxPrefab;
    public float HitboxLifespan;
    public float HitboxStunDuration;

    public void SpawnPunchHitbox()
    {
        GameObject hitboxObject = Instantiate(HitboxPrefab, transform.Find("PunchHitboxLocation"));
        hitboxObject.transform.localPosition = Vector3.zero;
        Hitbox hitbox = hitboxObject.GetComponent<Hitbox>();
        hitbox.Instantiate(HitboxLifespan, HitboxStunDuration, gameObject);
    }
}
