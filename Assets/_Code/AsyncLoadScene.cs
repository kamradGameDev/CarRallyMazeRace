using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncLoadScene : MonoBehaviour
{
    public static AsyncLoadScene instance;
    private AsyncOperation asyncOperation;
    public Text loadText;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }
    public IEnumerator LoadScene(int _loadSceneId)
    {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync(_loadSceneId);
        while(!asyncOperation.isDone)
        {
            float _progress = asyncOperation.progress / 0.9f;
            loadText.text = "Loading.. "  + string.Format("{0:0}%", _progress * 100f);
            yield return 0;
        }
    }
}
