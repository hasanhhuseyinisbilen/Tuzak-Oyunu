using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LivesUI : MonoBehaviour
{
    private void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    [Header("UI Elemanları")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button watchAdButton;

    private void Update()
    {
        if (gameOverPanel != null && gameOverPanel.activeInHierarchy)
        {
            UpdateAdButtonState();
        }
    }

    private void UpdateAdButtonState()
    {
        if (AdsManager.Instance != null && watchAdButton != null)
        {
            watchAdButton.interactable = AdsManager.Instance.IsRewardedAdReady();
        }
    }

    [Header("Kalp Görselleri (Opsiyonel)")]
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private void OnEnable()
    {
        LivesManager.OnLivesChanged += UpdateLivesUI;
        LivesManager.OnGameOver += ShowGameOver;
    }

    private void Start()
    {
        FixUILayout();
        Invoke(nameof(InitialCheck), 0.1f);
    }

    private void FixUILayout()
    {
        // 1. Can Metnini Sağ Üste Sabitle
        if (livesText != null)
        {
            RectTransform rt = livesText.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(1, 1);
            rt.anchoredPosition = new Vector2(-50, -50); 
        }

        // 2. Kalp Panelini Sağ Üste Sabitle
        if (heartImages != null && heartImages.Length > 0 && heartImages[0] != null)
        {
            Transform parent = heartImages[0].transform.parent;
            RectTransform parentRT = parent.GetComponent<RectTransform>();
            if (parentRT != null)
            {
                parentRT.anchorMin = new Vector2(1, 1);
                parentRT.anchorMax = new Vector2(1, 1);
                parentRT.pivot = new Vector2(1, 1);
                parentRT.anchoredPosition = new Vector2(-50, -120); 

                HorizontalLayoutGroup layout = parent.GetComponent<HorizontalLayoutGroup>();
                if (layout == null) layout = parent.gameObject.AddComponent<HorizontalLayoutGroup>();
                
                layout.spacing = 10f;
                layout.childControlWidth = false;
                layout.childControlHeight = false;
                layout.childForceExpandWidth = false;
                layout.childForceExpandHeight = false;
                layout.childAlignment = TextAnchor.MiddleRight;
            }
        }
    }

    private void InitialCheck()
    {
        if (LivesManager.Instance != null)
        {
            int currentLives = LivesManager.Instance.currentLives;
            UpdateLivesUI(currentLives);

            if (currentLives <= 0)
            {
                ShowGameOver();
            }
        }
        else
        {
            Debug.LogError("HATA: LivesManager BULUNAMADI! Sahneye 'LivesManager' objesini eklediğinizden emin olun.");
        }
    }

    private void OnDisable()
    {
        LivesManager.OnLivesChanged -= UpdateLivesUI;
        LivesManager.OnGameOver -= ShowGameOver;
    }

    private void UpdateLivesUI(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "Health: " + lives;
        }

        if (lives <= 0)
        {
            ShowGameOver();
        }

        if (heartImages != null && heartImages.Length > 0)
        {
            for (int i = 0; i < heartImages.Length; i++)
            {
                if (heartImages[i] != null)
                {
                    if (i < lives)
                    {
                        if (fullHeartSprite != null) heartImages[i].sprite = fullHeartSprite;
                        heartImages[i].color = Color.white; 
                    }
                    else
                    {
                        if (emptyHeartSprite != null) heartImages[i].sprite = emptyHeartSprite;
                        heartImages[i].color = new Color(0.2f, 0.2f, 0.2f, 0.15f); 
                    }
                }
            }
        }
    }



    private GameObject instantiatedPanel;

    private void ShowGameOver()
    {
        if (instantiatedPanel != null && instantiatedPanel.activeSelf) return;
        if (gameOverPanel != null && gameOverPanel.activeInHierarchy && gameOverPanel.activeSelf) return;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("LivesUI Hata: 'gameOverPanel' değişkeni atanmamış veya bulunamadı!");
        }
    }

    public void WatchAdAndGetLife()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.ShowRewardedAd(() => {
                if (LivesManager.Instance != null)
                {
                    Time.timeScale = 1; 
                    LivesManager.Instance.AddLife(1); 
                    
                    UnityEngine.SceneManagement.SceneManager.LoadScene(
                        UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                        
                    CloseGameOverPanel();
                }
                else
                {
                    Debug.LogError("LivesUI Hata: LivesManager.Instance bulunamadı!");
                }
            });
        }
        else
        {
            Debug.LogError("LivesUI Hata: AdsManager.Instance bulunamadı!");
        }
    }

    public void CloseGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
}
