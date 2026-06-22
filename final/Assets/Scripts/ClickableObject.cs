using UnityEngine;
using TMPro;

public class ClickableObject : MonoBehaviour
{
    public string message;
    public TextMeshProUGUI interactionText;
    public AudioSource audioSource;

    private void OnMouseDown()
    {
        if (interactionText != null)
        {
            interactionText.text = message;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}