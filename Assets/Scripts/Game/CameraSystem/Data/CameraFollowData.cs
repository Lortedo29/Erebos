﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Erebos/Camera Follow")]
public class CameraFollowData : ScriptableObject
{
    [SerializeField] private float _maxOffset = 8f;
    [SerializeField] private float _speed = 3f;
    [Header("Focus Rect")]
    [SerializeField] private float _focusRectSpeed = 3f;
    [SerializeField, Range(0, 50)] private float _maxRectPositionPercent = 20;
    [Space]
    [SerializeField, Range(0, 100)] private float _widthPercent = 30f;
    [SerializeField, Range(0, 100)] private float _heightPercent = 30f;

    public float MaxOffset { get => _maxOffset; }
    public float Speed { get => _speed; }
    public float WidthPercent { get => _widthPercent / 100f; }
    public float HeightPercent { get => _heightPercent / 100f; }
    public float FocusRectSpeed { get => _focusRectSpeed;}
    public float MaxRectPositionPercent { get => _maxRectPositionPercent / 100; }
}
