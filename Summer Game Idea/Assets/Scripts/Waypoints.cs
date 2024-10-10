using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] Lanes;

    private void Awake()
    {
        Lanes = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Lanes[i] = transform.GetChild(i);
        }
    }
}
