using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_BigBoy : BulletAmmo
{

    public override void UseSkill()
    {
        if (!usedSkill) { 
            rb.velocity = new Vector2(rb.velocity.x, (2f * bulletSkillMultiplier));
            usedSkill = true;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
