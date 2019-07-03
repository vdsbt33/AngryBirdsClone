using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform objectivesParent;
    int enemyCount = -1;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (EnemyCount() <= 0)
        {

        }
    }

    public int EnemyCount()
    {
        return objectivesParent.childCount;
    }

}
