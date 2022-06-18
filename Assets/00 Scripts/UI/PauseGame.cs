using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : PanelDropDownMono
{
    [SerializeField] private Button resumeBtn;
    private bool isPause = false;
    protected override void Awake()
    {
        base.Awake();

        isPause = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        resumeBtn.onClick.AddListener(HidePanel);
        openPanelBtn.onClick.AddListener(TogglePause);
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        resumeBtn.onClick.RemoveListener(HidePanel);
        openPanelBtn.onClick.RemoveListener(TogglePause);
    }

    protected override void HidePanel()
    {
        base.HidePanel();
        TogglePause();
    }
    private void TogglePause()
    {
        isPause = !isPause;
        EventDispatcher.Instance.PostEvent(EventID.Pause, isPause);
    }


}
