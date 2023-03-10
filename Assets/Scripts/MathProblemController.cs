using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class MathProblemController : MonoBehaviour
{
    private string[] keyArr = {"Maths_input_field", "Maths"};

    private int number = 0;

    private string key;

        AsyncOperationHandle<GameObject> mathAddresable;

    private Question answeredQuestion;

    float timeRemaining = 5;

    private float maxTimeRemaining = 5;

    private Button button;


    private void Start()
    {
        // throw new NotImplementedException();
    }

    public void StartProblem()
    {
        number = Random.Range(0, 2);
        key = keyArr[number];
        StartCoroutine(MathsProblemInstantiate());
        

    }

    void Update()
    {
        
        answeredQuestion = FindObjectOfType<Question>();

        if (answeredQuestion != null)
        {
            switch (answeredQuestion.HasAnswered)
            {
                case true:
                    timeRemaining = 0;
                    break;
                default:
                    timeRemaining -= Time.deltaTime;
                    break;
            }


            FindObjectOfType<Slider>().maxValue = maxTimeRemaining;
            FindObjectOfType<Slider>().value = timeRemaining;
        }
    }


    public IEnumerator MathsProblemInstantiate()
    {
        button = GetComponent<Button>();

        button.enabled = false;
        button.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);

        mathAddresable = Addressables.LoadAssetAsync<GameObject>(key);

        yield return mathAddresable;

        if (mathAddresable.Status == AsyncOperationStatus.Succeeded)
        {
            var gameObject = Addressables.InstantiateAsync(key);

            yield return new WaitUntil(() => timeRemaining <= 0);

            Addressables.Release(gameObject);

            timeRemaining = maxTimeRemaining;

            button.enabled = true;

            button.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        }
    }
}