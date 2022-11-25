using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public static SoundScript Instance { get; private set; }

    [SerializeField] AudioSource _riffleSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one SoundScript! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SoundTrigger()
    {
        _riffleSound.enabled = true;
    }

    public void SoundOff()
    {
        _riffleSound.enabled = false;
    }
}
