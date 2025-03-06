using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [Header("# Loading UI Components")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text loadingText;
    
    [Header("# Information")]
    [SerializeField] private string[] loadingTextStrings;
    [SerializeField] private float delayTime;
 
    private bool isLoading;
    private WaitForSeconds wait;
    private Coroutine loadingCoroutine;
    private void Awake()
    {
        wait = new WaitForSeconds(delayTime);
        SentenceSimilarityController.Instance.OnMeasureFailEvent += EndLoadingUI;
    }
    
    public void StartLoadingUI()
    {
        isLoading = true;
        root.SetActive(true);
        if(loadingCoroutine != null) StopCoroutine(loadingCoroutine);
        loadingCoroutine = StartCoroutine(LoadingUICoroutine());
    }

    public void EndLoadingUI()
    {
        isLoading = false;
        root.SetActive(false);
    }

    private IEnumerator LoadingUICoroutine()
    {
        int idx = 0, oper = 1;
        loadingText.text = loadingTextStrings[idx++];
        while (isLoading)
        {
            yield return wait;
            loadingText.text = loadingTextStrings[idx];
            idx += oper;
            if (idx == loadingTextStrings.Length - 1)
                oper = -1;
            else if (idx == 0)
                oper = 1;
        }
    }
}
