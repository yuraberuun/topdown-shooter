using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPointerController : MonoBehaviour
{
    [SerializeField] private Vector2 pointerBorder;
    [SerializeField] private float chestOffset = 60f;
    [SerializeField] private GameObject pointerPrefab;
    private List<GameObject> freePointers;
    private List<GameObject> pointersContainer;
    public int GetAngleFromVector(Vector3 dir)
    {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
    }
    void Start()
    {
        pointersContainer = new List<GameObject>();
        freePointers = new List<GameObject>();   
    }

    private void SetAllActivePointersToChests()
    {
        List<Vector3> allChests = new List<Vector3>();
        foreach(var chest in DropController.Instance.AllActiveChests())
            if (chest != null)
                allChests.Add(chest.position);

        if (allChests.Count == pointersContainer.FindAll(x => x.activeInHierarchy == true).Count)
            return;
            
        pointersContainer.FindAll(x => !allChests.Contains(x.GetComponent<PointerInfoContainer>().TargetPos)).ForEach(x => x.SetActive(false));               

        foreach (var chest in allChests)
            if (pointersContainer.FindAll(x => x.GetComponent<PointerInfoContainer>().TargetPos == chest).Count < 1)
            {
                if (freePointers.Count < 1)
                    FindFreePointers();   

                if (freePointers.Count > 0)
                    {
                        freePointers[0].GetComponent<PointerInfoContainer>().TargetPos = chest;
                        freePointers[0].SetActive(true);
                        freePointers.Remove(freePointers[0]);
                    }
                else
                    {
                        var pointer = Instantiate(pointerPrefab);
                        pointer.transform.SetParent(transform);
                        pointersContainer.Add(pointer);
                        pointer.GetComponentInChildren<PointerInfoContainer>().TargetPos = chest;
                    }
            }   
    }

    public void FindFreePointers() => freePointers.AddRange(pointersContainer.FindAll(x => x.activeInHierarchy == false && !freePointers.Contains(x)));

    private void Update()
    {
        SetAllActivePointersToChests();
        foreach (var pointer in pointersContainer)
            if (pointer.activeInHierarchy)
                SetProperPointerPositionOnScreen(pointer);
    }

    private void SetProperPointerPositionOnScreen(GameObject pointer)
    {
        Vector3 targetPosition = pointer.GetComponent<PointerInfoContainer>().TargetPos;
        RectTransform pointerRect = pointer.GetComponent<RectTransform>();
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= pointerBorder.x
            || targetPositionScreenPoint.x >= Screen.width - pointerBorder.x
            || targetPositionScreenPoint.y <= pointerBorder.y
            || targetPositionScreenPoint.y >= Screen.height - pointerBorder.y;

        if (isOffScreen)
        {
            RotatePointerTowardsTargetPosition(targetPosition, pointerRect);
            pointerRect.gameObject.SetActive(true);

            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

            if (targetPositionScreenPoint.x <= pointerBorder.x)
                cappedTargetScreenPosition.x = pointerBorder.x;
            if (targetPositionScreenPoint.x >= Screen.width - pointerBorder.x) 
                cappedTargetScreenPosition.x = Screen.width - pointerBorder.x;
            if (targetPositionScreenPoint.y <= pointerBorder.y) 
                cappedTargetScreenPosition.y = pointerBorder.y;
            if (targetPositionScreenPoint.y >= Screen.height - pointerBorder.y) 
                cappedTargetScreenPosition.y = Screen.height - pointerBorder.y;

            // Vector3 pointerWorldPosition = Camera.main.WorldToScreenPoint(cappedTargetScreenPosition);
            pointerRect.position = cappedTargetScreenPosition;
        }
        else
        {
            pointerRect.localEulerAngles = new Vector3(0f, 0f, -90f);
            pointerRect.position = targetPositionScreenPoint + new Vector3(0f, chestOffset, 0f);
            var tempPos = pointerRect.transform.position;
            pointerRect.transform.position = new Vector3(tempPos.x, tempPos.y, tempPos.z);
        }

        var localPos = pointerRect.localPosition;
        pointerRect.localPosition = new Vector3(localPos.x, localPos.y, 0f);
    }

    private void RotatePointerTowardsTargetPosition(Vector3 targetPosition, RectTransform pointerRect)
    {
        Vector3 dir = (targetPosition - Camera.main.transform.position).normalized;
        float angle = GetAngleFromVector(dir);
        pointerRect.localEulerAngles = new Vector3(0f, 0f, angle);
    }
}
