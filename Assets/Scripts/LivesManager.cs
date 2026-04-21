using UnityEngine;
using System;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance { get; private set; }

    [Header("Can Ayarları")]
    [SerializeField] private int maxLives = 3;

    private int _currentLives;
    public int currentLives 
    { 
        get => _currentLives; 
        private set 
        {
            _currentLives = Mathf.Clamp(value, 0, maxLives);
            OnLivesChanged?.Invoke(_currentLives);
        }
    }

    public static event Action<int> OnLivesChanged;
    public static event Action OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void ResetLivesToMax()
    {
        currentLives = maxLives;
        SaveData();
        Debug.Log("<color=cyan>LivesManager: Canlar 3'e sıfırlandı.</color>");
    }

    public bool HasLives()
    {
        return currentLives > 0;
    }

    public void UseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            SaveData();
        }

        if (currentLives <= 0)
        {
            Debug.Log("<color=red>LivesManager: CAN BİTTİ, OnGameOver tetikleniyor!</color>");
            OnGameOver?.Invoke();
        }
    }

    public void AddLife(int amount = 1)
    {
        currentLives += amount;
        Debug.Log("<color=green>LivesManager: Can " + amount + " artırıldı. Mevcut Can: " + currentLives + "</color>");
        SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("CurrentLives", currentLives);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        // Geçici Reset: Canları her açılışta 3'e zorluyoruz
        currentLives = 3;
        SaveData();
    }
}
