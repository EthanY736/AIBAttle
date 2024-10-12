using System.Collections;
using UnityEngine;
using TMPro;

public class FadeTextController : MonoBehaviour
{
    public static FadeTextController instance;
    public TextMeshProUGUI fadeText;
    public string message = "Hello, World!";
    public float fadeDuration = 2f;
    public float moveDistance = 50f;
    
    private Color originalColor;
    private Vector3 originalPosition;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        originalColor = fadeText.color;
        originalPosition = fadeText.transform.position;
    }

    public void TriggerText()
    {
        fadeText.gameObject.SetActive(true);
        fadeText.text = message;
        fadeText.color = originalColor;
        fadeText.transform.position = originalPosition;
        StartCoroutine(FadeAndMoveText());
    }

    IEnumerator FadeAndMoveText()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = originalPosition + new Vector3(0, moveDistance, 0);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            fadeText.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeText.text = "";
    }
}
