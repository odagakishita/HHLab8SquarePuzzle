using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MoveKeyInput()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        //Debug.Log("hogehoge");
        return horizontalValue;
    }

    public float RotateKeyInput()
    {
        float verticalValue = Input.GetAxis("Vertical");
        return verticalValue;
    }
}
