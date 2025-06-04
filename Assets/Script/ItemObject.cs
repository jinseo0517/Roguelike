using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemOS data;

    public int GetPoint()
    {
        return data.point;
    }
}
