using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousExplosionSpawner : MonoBehaviour
{
    public Transform[] explosionTransforms;
    public Transform FinalExplosionTransform;

    public GameObject smallExplosion;
    public GameObject FinalExplosion;

    public void PlayExplosion()
    {
        StartCoroutine(playExplosionAnimationSequence());
    }

    public float GetExplosionDuration()
    {
        return explosionTransforms.Length * 0.2f + 1.0f;
    }

    IEnumerator playExplosionAnimationSequence()
    {
        for (int i = 0; i < explosionTransforms.Length; i++)
        {
            Instantiate(smallExplosion, explosionTransforms[i].position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        Instantiate(FinalExplosion, FinalExplosionTransform.position, Quaternion.identity);
        
    }
}
