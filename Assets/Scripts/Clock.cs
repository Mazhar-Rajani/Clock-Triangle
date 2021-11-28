using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Clock : MonoBehaviour
{
    [SerializeField] private Material matHands = default;
    [SerializeField] private TMP_Text timeText = default;
    [SerializeField] private GameObject linePrefab = default;
    [SerializeField] private Transform lineHolder = default;
    [SerializeField] private Transform hoursPoint = default;
    [SerializeField] private Transform minutesPoint = default;
    [SerializeField] private Transform secondsPoint = default;
    [SerializeField] private float pointRadius = default;
    [SerializeField] private float lineRadius = default;
    [SerializeField] private float moveSpeed = default;

    private DateTime dateTime;
    private Tweener hoursTweener;
    private Tweener minutesTweener;
    private Tweener secondsTweener;
    private Mesh mesh;

    private void Awake()
    {
        InitLines();
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        hoursPoint.position = GetPosOnClock(dateTime.Hour * 30, pointRadius);
        hoursTweener = hoursPoint.DOMove(GetPosOnClock(dateTime.Hour * 30, pointRadius), moveSpeed)
        .SetLoops(-1, LoopType.Incremental)
        .OnStepComplete(() => hoursTweener.ChangeValues(hoursPoint.position, GetPosOnClock(dateTime.Hour * 30, pointRadius)));

        minutesPoint.position = GetPosOnClock(dateTime.Minute * 6, pointRadius);
        minutesTweener = minutesPoint.DOMove(GetPosOnClock(dateTime.Minute * 6, pointRadius), moveSpeed)
        .SetLoops(-1, LoopType.Incremental)
        .OnStepComplete(() => minutesTweener.ChangeValues(minutesPoint.position, GetPosOnClock(dateTime.Minute * 6, pointRadius)));

        secondsPoint.position = GetPosOnClock(dateTime.Second * 6, pointRadius);
        secondsTweener = secondsPoint.DOMove(GetPosOnClock(dateTime.Second * 6, pointRadius), moveSpeed)
        .SetLoops(-1, LoopType.Incremental)
        .OnStepComplete(() => secondsTweener.ChangeValues(secondsPoint.position, GetPosOnClock(dateTime.Second * 6, pointRadius)));

        InitHands();
    }

    private void Update()
    {
        UpdateTime();
        UpdateHands();
    }

    private void UpdateTime()
    {
        dateTime = System.DateTime.Now;
        string hrs = dateTime.Hour > 9 ? dateTime.Hour.ToString() : $"0{dateTime.Hour}";
        string mins = dateTime.Minute > 9 ? dateTime.Minute.ToString() : $"0{dateTime.Minute}";
        string secs = dateTime.Second > 9 ? dateTime.Second.ToString() : $"0{dateTime.Second}";
        timeText.SetText($"{hrs} : {mins} : {secs}");
    }

    private void UpdateHands()
    {
        if (mesh == null)
            return;

        Vector3[] verts = new Vector3[3];
        verts[0] = hoursPoint.position;
        verts[1] = minutesPoint.position;
        verts[2] = secondsPoint.position;
        mesh.SetVertices(verts);
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, matHands, 0);
    }

    private void InitHands()
    {
        int[] tris = new int[3];
        Vector2[] uv = new Vector2[3];
        Vector3[] verts = new Vector3[3];
        Vector3[] normals = new Vector3[3];

        tris[0] = 0; tris[1] = 2; tris[2] = 1;
        uv[0] = new Vector2(0, 0); uv[1] = new Vector2(1, 0); uv[2] = new Vector2(1, 1);
        normals[0] = -Vector3.forward; normals[1] = -Vector3.forward; normals[2] = -Vector3.forward;

        verts[0] = hoursPoint.position;
        verts[1] = minutesPoint.position;
        verts[2] = secondsPoint.position;

        mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;
    }

    private void InitLines()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject line = Instantiate(linePrefab, lineHolder);
            line.transform.position = GetPosOnClock(i * 30, lineRadius);
            line.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -i * 30));
        }
    }

    public Vector3 GetPosOnClock(int angle, float radius)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);

        for (int i = 0; i < 12; i++)
        {
            Gizmos.DrawSphere(GetPosOnClock(i * 30, pointRadius), 0.3f);
            Gizmos.DrawLine(transform.position, GetPosOnClock(i * 30, lineRadius));
        }
    }
}