using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Adds the glower's pickup-mimicking glow.
 * Also makes it follow the glower continuously.
 */

public class GlowerGlow : MonoBehaviour
{
    private Light disguiseLight;
    GameObject lightGameObject;

    [SerializeField] float glowerOffset = 0.3f;
    [SerializeField] float lightIntensity = 100;

    void Start()
    {
        lightGameObject = new GameObject("Glower's Light");
        disguiseLight = lightGameObject.AddComponent<Light>();

        disguiseLight.color = Color.yellow;
        disguiseLight.intensity = lightIntensity;
        disguiseLight.range = 0.5f;
    }

    void Update()
    {
        lightGameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - glowerOffset, gameObject.transform.position.z);
    }
}
