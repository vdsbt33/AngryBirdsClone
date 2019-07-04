using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAmmo : MonoBehaviour
{
    public string bulletName;
    public float bulletSkillMultiplier;
    protected bool usedSkill = false;

    protected Rigidbody2D rb;

    public virtual void UseSkill()
    {
        
    }
}
