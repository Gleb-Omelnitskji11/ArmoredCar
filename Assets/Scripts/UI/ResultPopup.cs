using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private TMP_Text _loseText;
    [SerializeField] private Button _repeatButton;
    
    private void Awake()
    {
        _repeatButton.onClick.AddListener(StartGame);
    }

    public void ShowResult(bool win)
    {
        gameObject.SetActive(true);
        _loseText.gameObject.SetActive(!win);
        _winText.gameObject.SetActive(win);
    }

    // Update is called once per frame
    private void StartGame()
    {
        gameObject.SetActive(false);
        OnStartClicked?.Invoke();
    }

    public event Action OnStartClicked;
}
