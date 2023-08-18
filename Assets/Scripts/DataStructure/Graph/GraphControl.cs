using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunnyAlgorithm;

public class GraphControl : MonoBehaviour
{
    public GraphView view;
    private bool play_or_pause = true;//开始、暂停
    private GraphModel demo;
    private static bool IsStart = false;

    private void OnEnable()
    {
        view.Btn_NextStep.onClick.AddListener(NextStep_Btn);
        view.Btn_LastStep.onClick.AddListener(LastStep_Btn);
        view.Btn_StartButton.onClick.AddListener(Auto_Btn);
        view.Btn_Restart.onClick.AddListener(Restart_Btn);
    }
    private void OnDisable()
    {
        view.Btn_NextStep.onClick.RemoveListener(NextStep_Btn);
        view.Btn_LastStep.onClick.RemoveListener(LastStep_Btn);
        view.Btn_StartButton.onClick.RemoveListener(Auto_Btn);
        view.Btn_Restart.onClick.RemoveListener(Restart_Btn);
        demo.Clean();
    }
    private void Start()
    {
        initialize();
    }
    private void initialize()
    {
        demo = new GraphModel();

    }


    #region BottomButton
    public void Restart_Btn()
    {
        StopAllCoroutines();
        demo.Clean();
        initialize();
        IsStart = false;
        play_or_pause = true;
        view.Btn_LastStep.interactable = false;
        view.Btn_NextStep.interactable = true;
        view.Btn_StartButton.interactable = true;
        view.Text_StartButton.text = "自动";
    }

    public void LastStep_Btn()
    {
        StartCoroutine(LastStep());
    }

    private IEnumerator LastStep()
    {
        view.Btn_NextStep.interactable = true;
        view.Btn_StartButton.interactable = true;
        if (demo.demoQueue.Count == 0)
        {
            play_or_pause = true;
            view.Text_StartButton.text = "自动";
        }

        if (demo.executedStack.Count > 0)
        {
            view.Btn_LastStep.interactable = false;
            bool flag;
            do
            {
                flag = demo.PlayBackward();
                if (!flag)
                {
                    yield return new WaitForSeconds(MoveTool.duration);
                }
            } while (flag);
            view.Btn_LastStep.interactable = true;
            if (demo.executedStack.Count == 0)
                view.Btn_LastStep.interactable = false;
        }
    }
    public void NextStep_Btn()
    {
        IsStart = true;
        StartCoroutine(NextStep());
    }

    private IEnumerator NextStep()
    {
        if (demo.demoQueue.Count > 0)
        {
            view.Btn_NextStep.interactable = false;
            bool flag;
            do
            {
                flag = demo.PlayForward();
                if (!flag)
                    yield return new WaitForSeconds(MoveTool.duration);
            } while (flag);
            view.Btn_NextStep.interactable = true;
            if (demo.demoQueue.Count == 0)
            {
                view.Btn_NextStep.interactable = false;
                view.Btn_StartButton.interactable = false;
                view.Text_StartButton.text = "结束";
            }
        }
        view.Btn_LastStep.interactable = true;
    }
    public void Auto_Btn()
    {
        IsStart = true;
        if (play_or_pause)
        {
            view.Btn_LastStep.interactable = false;
            view.Btn_NextStep.interactable = false;
            view.Text_StartButton.text = "暂停";
            StartCoroutine("AutoPlay");
        }
        else
        {
            view.Btn_LastStep.interactable = true;
            view.Btn_NextStep.interactable = true;
            view.Text_StartButton.text = "继续";
            StopCoroutine("AutoPlay");
        }
        play_or_pause = !play_or_pause;

    }
    private IEnumerator AutoPlay()
    {
        while (demo.demoQueue.Count > 0)
        {
            bool flag;
            do
            {
                flag = demo.PlayForward();
                if (!flag)
                    yield return new WaitForSeconds(MoveTool.duration);
            } while (flag);
        }
        view.Text_StartButton.text = "结束";
        view.Btn_StartButton.interactable = false;
        view.Btn_NextStep.interactable = false;
        view.Btn_LastStep.interactable = true;
    }
    #endregion
}
