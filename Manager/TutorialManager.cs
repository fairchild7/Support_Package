using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*
 * Change suitable functions and variables name
 */
public class TutorialManager : Singleton_Normal<TutorialManager>
{
    [SerializeField] Step[] tutorialSteps;
    [SerializeField] Transform tutorialTf;
    [SerializeField] GameObject blockPanel;

    int currentStep;

    private void Awake()
    {
        blockPanel.SetActive(false);
        tutorialTf.gameObject.SetActive(false);
        RemoveAllBlocks();
    }

    public void StartTutorial()
    {
        tutorialTf.gameObject.SetActive(true);
        currentStep = 0;
        ShowStep(currentStep);
    }

    public void ShowStep(int step)
    {
        tutorialTf.transform.DOMove(tutorialSteps[step].tutorialPos.position, 0.25f).OnComplete(()=>
        {
            tutorialTf.localScale = tutorialSteps[step].scale;
            RemoveCurrentRayBlock(step);
        });
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length)
        {
            ShowStep(currentStep);
        }
        else
        {
            RemoveAllBlocks();
            tutorialTf.gameObject.SetActive(false);
            CS_DataManager.Ins.FinishTutorial();
        }
        blockPanel.SetActive(false);
    }

    public void NextStep(float time)
    {
        Invoke(nameof(NextStep), time);
    }

    public void EnableBlock()
    {
        blockPanel.SetActive(true);
    }

    public void RemoveCurrentRayBlock(int step)
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            if (tutorialSteps[i].notify != null)
            {
                tutorialSteps[i].notify.SetActive(i == step);
            }
            if (tutorialSteps[i].handNotify != null)
            {
                tutorialSteps[i].handNotify.SetActive(i == step);
            }
        }
    }

    public void RemoveAllBlocks()
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            if (tutorialSteps[i].notify != null)
            {
                tutorialSteps[i].notify.SetActive(false);
            }
            if (tutorialSteps[i].handNotify != null)
            {
                tutorialSteps[i].handNotify.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class Step
{
    public Transform tutorialPos;
    public Vector3 scale;
    public GameObject notify;
    public GameObject handNotify;
}
