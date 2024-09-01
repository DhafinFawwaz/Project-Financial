using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PieChartDraggable : MonoBehaviour
{
    [System.Serializable]
    public class Pie
    {
        public Image PieBaseImg;
        public Image PieArrowImg;
    }

    [SerializeField] float _pieRadius = 100;
    [SerializeField] Pie[] _pies;

    private Pie _draggedPie = null;
    private Vector3 initialUpVector;
    private Vector3 prevUpVector;
    private Vector3 nextUpVector;

    void Start()
    {
        for (int i = 0; i < _pies.Length; i++)
        {
            var pie = _pies[i];
            pie.PieBaseImg.fillAmount = 0;
            ButtonUI btn = pie.PieArrowImg.GetComponent<ButtonUI>();

            int idx = i;
            btn.OnPointerDownEvent.AddListener(() => OnButtonDown(idx));
            btn.OnPointerUpEvent.AddListener(OnButtonUp);

            // pie.PieArrowImg.transform.localEulerAngles = new Vector3(0, 0, 360 / _pies.Length * i);
        }
    }

    void OnButtonDown(int idx)
    {
        var pie = _pies[idx];
        _draggedPie = pie;
        initialUpVector = pie.PieArrowImg.transform.up;

        int nextIndex = (idx + 1) % _pies.Length;
        int prevIndex = idx - 1;
        if (prevIndex == -1) prevIndex = _pies.Length - 1;

        prevUpVector = _pies[prevIndex].PieArrowImg.transform.up;
        nextUpVector = _pies[nextIndex].PieArrowImg.transform.up;
    }

    void OnButtonUp()
    {
        _draggedPie = null;
    }

    public Action<float[]> OnPieValuesChanged;
    void Update()
    {
        RefreshBase();
        if (_draggedPie == null || _draggedPie.PieBaseImg == null) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 piePos = _draggedPie.PieBaseImg.transform.position;
        Vector3 dir = (mousePos - piePos).normalized;
        Vector3 clamped = ClampUpVector(dir, prevUpVector, nextUpVector, initialUpVector);
        _draggedPie.PieArrowImg.transform.up = clamped;
        // RefreshBase();
        ValidateChart();
        OnPieValuesChanged?.Invoke(GetPieValues());
    }

    void ValidateChart()
    {
        // check if _pies[0] is between _pies[1] and _pies[n-1]
        // check if _pies[1] is between _pies[2] and _pies[0]
        // check if _pies[2] is between _pies[3] and _pies[1]
        // if any of this is wrong, move all the pies to the correct position. like a few degrees after the previous pie
        // TODO: implement this

    }

    

    public void RefreshBase()
    {
        for(int i = 0; i < _pies.Length; i++)
        {
            var pie = _pies[i];
            var nextPie = _pies[(i + 1) % _pies.Length];
            pie.PieBaseImg.transform.rotation = pie.PieArrowImg.transform.rotation;
            
            Vector3 minUp = pie.PieArrowImg.transform.up;
            Vector3 maxUp = nextPie.PieArrowImg.transform.up;
            Vector3 mid = ((minUp + maxUp) / 2).normalized;
            float angle = Vector3.Angle(minUp, maxUp);
            if(Vector3.Dot(pie.PieArrowImg.transform.right, mid) < -0.1f)
            {
                angle = 360 - angle;
            }
            pie.PieBaseImg.fillAmount = angle / 360;
        }
    }

    public float[] GetPieValues()
    {
        float[] values = new float[_pies.Length];
        for (int i = 0; i < _pies.Length; i++)
        {
            values[i] = _pies[i].PieBaseImg.fillAmount;
        }
        return values;
    }

    public void SetPieValues(float[] values)
    {
        float total = 0; for (int i = 0; i < values.Length; i++) total += values[i];
        float currentTotal = 0;
        for (int i = 0; i < _pies.Length; i++)
        {
            if(i != 0) currentTotal += values[i - 1];
            float val = -currentTotal/total * 360;
            if(float.IsNaN(val)) val = -1f*i;
            Vector2 dir = Quaternion.Euler(0, 0, val) * Vector2.up;
            _pies[i].PieArrowImg.transform.up = dir;
        }
        // this.Invoke(RefreshBase, 0.05f);
        // RefreshBase();
    }

    public Vector3 GetPieBaseCenterDirection(int i)
    {
        var pie = _pies[i];
        var nextPie = _pies[(i + 1) % _pies.Length];
        pie.PieBaseImg.transform.rotation = pie.PieArrowImg.transform.rotation;
        
        Vector3 minUp = pie.PieArrowImg.transform.up;
        Vector3 maxUp = nextPie.PieArrowImg.transform.up;
        Vector3 mid = ((minUp + maxUp) / 2).normalized;
        if(Vector3.Dot(pie.PieArrowImg.transform.right, mid) < -0.1f)
        {
            mid = -mid;
        }
        return pie.PieBaseImg.transform.position + mid * _pieRadius/2;
    }

    float CalculateClockwiseAngle(Vector3 from, Vector3 to)
    {
        float angle = Vector3.Angle(from, to);
        // if (angle < 0) angle += 360;
        return angle;
    }

    bool IsBetween(Vector3 left, Vector3 target, Vector3 right)
    {
        float al = Vector3.Angle(left, target);
        float ar = Vector3.Angle(right, target);
        float lr = Vector3.Angle(left, right);
        // Debug.Log(al+ar + " - " + lr);
        return al + ar < lr + 0.1f;
    }

    Vector3 ClampUpVector(Vector3 currentUp, Vector3 minUp, Vector3 maxUp, Vector3 initialUpVector)
    {
        Vector3 mid = ((minUp + maxUp) / 2).normalized;
        Vector3 reverseMid = -mid;

        if (!IsBetween(minUp, initialUpVector, maxUp))
        {
            // if currentUp between minUp and reverseMid, return currentUp
            // if currentUp between reverseMid and maxUp, return reverseMid
            // if currentUp between maxUp and mid, return maxUp
            // if currentUp between mid and minUp, return minUp

            if(IsBetween(minUp, currentUp, reverseMid))
            {
                return currentUp;
            }
            else if (IsBetween(reverseMid, currentUp, maxUp))
            {
                return currentUp;
            }
            else if (IsBetween(maxUp, currentUp, mid))
            {
                return maxUp;
            }
            else if (IsBetween(mid, currentUp, minUp))
            {
                return minUp;
            }
            else
            {
                return currentUp;
            }
        }
        else
        {
            // if currentUp between minUp and mid, return currentUp
            // if currentUp between mid and maxUp, return currentUp
            // if currentUp between maxUp and reverseMid, return maxUp
            // if currentUp between reverseMid and minUp, return reverseMid

            if (IsBetween(minUp, currentUp, mid))
            {
                return currentUp;
            }
            else if (IsBetween(mid, currentUp, maxUp))
            {
                return currentUp;
            }
            else if (IsBetween(maxUp, currentUp, reverseMid))
            {
                return maxUp;
            }
            else if (IsBetween(reverseMid, currentUp, minUp))
            {
                return minUp;
            }
            else
            {
                return mid;
            }
        }   
    }





}
