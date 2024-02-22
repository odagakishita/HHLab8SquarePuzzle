using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentCollision : MonoBehaviour
{
    public bool isGround;
    public void ParentInit()
    {
        isGround = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string Tag = collision.gameObject.tag;
        if (Tag == "Ground" || Tag == "Red" || Tag == "Yellow" || Tag == "Blue" || Tag == "Green" || Tag == "Yellow")
        {
            isGround = true;
        }
    }
}
