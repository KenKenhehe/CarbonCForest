using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TransformPair
{
    public Transform left;
    public Transform right; 
}

public class MechCannonExplosion : MonoBehaviour
{
    public TransformPair pair;
    public GameObject explosionFX;
    public float gapDistance = 1f;
    public int explosionNum = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PeformExplosionFX());
    }

    IEnumerator PeformExplosionFX()
    {
        for (int i = 0; i < explosionNum; i++)
        {
            Instantiate(explosionFX, pair.left.position - new Vector3( (i * gapDistance), 0, 0), Quaternion.identity);
            Instantiate(explosionFX, pair.right.position + new Vector3((i * gapDistance), 0, 0), Quaternion.identity);
            //SoundFXHandler.instance.Play("EnemyExplode");
            yield return new WaitForSeconds(.2f);
        }
    }
}
