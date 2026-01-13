using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Timer lifeTimer;
    public float HitstunDuration;
    public GameObject Owner;

    public void Instantiate(float lifeSpan, float hitstunDuration, GameObject owner)
    {
        lifeTimer.ResetTime(lifeSpan);
        HitstunDuration = hitstunDuration;
        Owner = owner;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTimer.Update(Time.deltaTime))
        {
            Destroy(gameObject);
        }
    }
}
