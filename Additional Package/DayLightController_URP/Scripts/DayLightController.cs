using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class DayLightController : MonoBehaviour
{
    [SerializeField] GameObject sun, moon;
    [SerializeField] Light2D globalLight;
    [SerializeField] float daySession, nightSession, minimumIntensity, maximumIntensity;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Transform[] movePosition;

    private Vector3[] path;
    private bool isDay;
    private int intensityDirection;
    private float sunMinimumYPos = 999f, sunMaximumYPos = 0f;
    public bool IsDay => isDay;

    private void Start()
    {
        OnInit();
    }

    private void Update()
    {
        CalculateLightIntensity();
    }

    public void OnInit()
    {
        sun.SetActive(true);
        moon.SetActive(false);
        moon.transform.DOMove(spawnPosition.position, 0f);
        sun.transform.DOMove(spawnPosition.position, 0f);
        isDay = true;
        intensityDirection = 1;
        globalLight.intensity = 0.5f;

        path = new Vector3[movePosition.Length];
        for (int j = 0; j < movePosition.Length; j++)
        {
            path[j] = movePosition[j].localPosition;
        }

        for (int i = 0; i < path.Length; i++)
        {
            if (path[i].y < sunMinimumYPos)
            {
                sunMinimumYPos = path[i].y;
            }
            if (path[i].y > sunMaximumYPos)
            {
                sunMaximumYPos = path[i].y;
            }
        }
        SunMove();
    }

    private void SunMove()
    {
        sun.transform.DOLocalPath(path, daySession, PathType.CatmullRom).SetOptions(false).OnComplete(() => {
            sun.SetActive(false);
            moon.SetActive(true);
            sun.transform.DOMove(movePosition[0].position, 0f);
            isDay = false;
            MoonMove();
        });
    }

    private void MoonMove()
    {
        moon.transform.DOLocalPath(path, nightSession, PathType.CatmullRom).SetOptions(false).OnComplete(() => {
            moon.SetActive(false);
            sun.SetActive(true);
            moon.transform.DOMove(movePosition[0].position, 0f);
            isDay = true;
            SunMove();
        });
    }

    private void CalculateLightIntensity()
    {
        if (isDay)
        {
            globalLight.intensity = (sun.transform.position.y - sunMinimumYPos) / (sunMaximumYPos - sunMinimumYPos) * (1 - minimumIntensity / maximumIntensity) + minimumIntensity;
        }
    }

    /*
     * This function work but not very correctly with DOLocalPath or DOPath
     */
    private void AdjustLightIntensity()
    {
        if (isDay)
        {
            globalLight.intensity += Time.deltaTime * 2 / daySession * (1 - minimumIntensity / maximumIntensity) * intensityDirection;
            if (globalLight.intensity > maximumIntensity || globalLight.intensity < minimumIntensity)
            {
                intensityDirection *= -1;
                globalLight.intensity = Mathf.Clamp(globalLight.intensity, minimumIntensity, maximumIntensity);
            }
        }
    }
}
