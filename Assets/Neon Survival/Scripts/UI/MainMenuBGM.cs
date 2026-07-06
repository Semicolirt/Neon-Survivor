using System.Collections;
using UnityEngine;

/// <summary>
/// Phát nhạc nền cho MainMenu với hiệu ứng fade in/out mượt mà.
/// Gắn script này vào một GameObject trong MainMenuScene và kéo AudioClip vào Inspector.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MainMenuBGM : MonoBehaviour
{
    [Header("BGM Settings")]
    [SerializeField] private AudioClip bgmClip;

    [Range(0f, 1f)]
    [SerializeField] private float maxVolume = 0.4f;

    [SerializeField] private float fadeDuration = 1.5f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    private static MainMenuBGM instance;

    private void Awake()
    {
        // Singleton nhẹ trong scene — tránh trùng lặp khi reload
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        audioSource = GetComponent<AudioSource>();
        SetupAudioSource();
    }

    private void Start()
    {
        // Bắt đầu phát nhạc với fade in
        FadeIn();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void SetupAudioSource()
    {
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
        audioSource.priority = 0; // Ưu tiên cao nhất cho BGM
    }

    /// <summary>
    /// Fade in nhạc nền từ 0 lên maxVolume.
    /// </summary>
    public void FadeIn()
    {
        if (bgmClip == null)
        {
            Debug.LogWarning("MainMenuBGM: Chưa gán AudioClip cho nhạc nền!");
            return;
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        audioSource.clip = bgmClip;
        audioSource.Play();
        fadeCoroutine = StartCoroutine(FadeCoroutine(0f, maxVolume));
    }

    /// <summary>
    /// Fade out nhạc nền từ volume hiện tại xuống 0.
    /// </summary>
    public void FadeOut()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCoroutine(audioSource.volume, 0f, stopOnComplete: true));
    }

    /// <summary>
    /// Fade out nhạc nền rồi gọi callback khi xong.
    /// Dùng khi cần đợi nhạc tắt hẳn trước khi chuyển scene.
    /// </summary>
    public void FadeOutAndThen(System.Action onComplete)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCoroutine(audioSource.volume, 0f, stopOnComplete: true, onComplete));
    }

    private IEnumerator FadeCoroutine(float from, float to, bool stopOnComplete = false, System.Action onComplete = null)
    {
        float elapsed = 0f;
        audioSource.volume = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Dùng unscaledDeltaTime để fade vẫn chạy khi game pause
            audioSource.volume = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = to;

        if (stopOnComplete)
        {
            audioSource.Stop();
        }

        onComplete?.Invoke();
        fadeCoroutine = null;
    }
}
