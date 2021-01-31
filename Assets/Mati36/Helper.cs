using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour {
    
    public void Execute(Action action, float delay)
    {
        StartCoroutine(ExecuteRoutine(action, delay));
    }

    IEnumerator ExecuteRoutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
