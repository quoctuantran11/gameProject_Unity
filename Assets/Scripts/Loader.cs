using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{   
    public GameObject canvas;
    public Slider slider;
    public Text progressText;
    private float target;
    private string targetText;
    public static Loader Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Loading(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        target = 0;
        slider.value = 0;
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        canvas.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return new WaitForSeconds(5);
            target = progress;
            targetText = progress * 100f + "%";
        }

        operation.allowSceneActivation = true;
        canvas.SetActive(false);
    }

    void Update() {
        slider.value = Mathf.MoveTowards(slider.value, target, 3 * Time.deltaTime);
        progressText.text = targetText;
    }
}
