using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages the light, which visualises the player's vision range.
 * It just moves the light to the location of the player + the additive light range.
 */

public class LightScript : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float lightRange = 12.0f;
    [SerializeField] bool hasFlashlight;
    public bool HasFlashlight { get { return hasFlashlight; } }

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + lightRange, target.transform.position.z);
    }
    public void ChangeLightRange(float lightRange)
    {
        this.lightRange = lightRange;
    }

    public void LightRanger()
    {
        lightRange = 16f;
        hasFlashlight = true;
    }

}
