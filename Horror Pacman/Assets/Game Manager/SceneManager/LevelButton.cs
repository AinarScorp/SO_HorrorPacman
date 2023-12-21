using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Buttons for the levels in the level selection screen.
 */

public class LevelButton : MonoBehaviour
{
    int lvlIndex;

    public int LvlIndex { get => lvlIndex; }

    private void Start()
    {
        lvlIndex = transform.GetSiblingIndex();
    }
}
