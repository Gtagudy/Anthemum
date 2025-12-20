using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Header("UI Overlay")]
    public GameObject promptPanel;          // your full-screen QTE prompt
    public float      barFillSpeed = 1f;    // 1 = empty in 1 second

    private UnityEvent onSuccess = new UnityEvent();
    private UnityEvent onFail    = new UnityEvent();

    /// <summary>
    /// Starts the QTE: after startDelay, shows the prompt and waits for input
    /// during the next window seconds. Returns true if succeeded.
    /// </summary>
    public IEnumerator RunQTE(
        float startDelay,
        float window,
        UnityEvent successCallback,
        UnityEvent failCallback
    ) {
        onSuccess = successCallback ?? new UnityEvent();
        onFail    = failCallback    ?? new UnityEvent();

        // 1) initial delay
        yield return new WaitForSeconds(startDelay);

        // 2) show prompt
        promptPanel.SetActive(true);
        float elapsed = 0f;
        bool  pressed = false;

        // 3) monitor input & fill bar
        var bar = promptPanel.GetComponentInChildren<UnityEngine.UI.Image>();  
        // assumes you have an Image whose fillAmount you drive

        while (elapsed < window)
        {
            // fillAmount goes from 1→0 over 'window' seconds:
            bar.fillAmount = 1f - (elapsed / window);

            if (!pressed && Input.GetButtonDown("Fire1"))  // or whatever your key
            {
                pressed = true;
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 4) hide prompt
        promptPanel.SetActive(false);

        // 5) invoke the correct event
        if (pressed)
            onSuccess.Invoke();
        else
            onFail.Invoke();
    }
}
