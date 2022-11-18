using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PP : MonoBehaviour
{
    public static PP Instance { get; private set; }

    [SerializeField] Volume _pp;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ScreenShake! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PPTrigger()
    {
        _pp.enabled = true;
        StartCoroutine(PPOff());
        IEnumerator PPOff()
        {
            yield return new WaitForSeconds(0.1f);
            _pp.enabled = false;
        }
    }
}
