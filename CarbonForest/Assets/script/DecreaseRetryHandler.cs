using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecreaseRetryHandler : MonoBehaviour
{
    private int currentRetry = 0;
    private Text retryText;

    private void Awake()
    {
        retryText = GetComponent<Text>();
        print(retryText);
    }


    public void SetRetry(int i)
    {
        currentRetry = i;
        retryText.text = currentRetry.ToString();
    }

    public void DecreaseCurrentRetry()
    {
        currentRetry -= 1;
        retryText.text = currentRetry.ToString();
    }
}
