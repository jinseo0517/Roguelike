using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game/Item", fileName = "NewItem")]
public class ItemOS : ScriptableObject
{
    [Header("Score Value")]
    public int point = 10;
    
}
