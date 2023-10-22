using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CanvasMouse : MonoBehaviour
{

    public GameManager gameManager;
    public Camera mainCamera;

    public GameObject playerCursor;

    private Vector3 prevPosition;

    private bool isDragging = false;

    private Quaternion targetRotation;
    private float rotationSpeed =500f;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame


    public void DragStart(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        prevPosition = pointerEventData.position;
        playerCursor.SetActive(true);
        Cursor.visible = false;
        gameManager.ActivePlayerCursor(true);

        isDragging = true;

        StartCoroutine(RotateFlash());
    }

    void LateUpdate()
    {
        if (isDragging)
        {
            playerCursor.transform.position = Input.mousePosition;
            Vector3 scenePositon = mainCamera.ScreenToWorldPoint(playerCursor.transform.position);
            gameManager.MoveInvisPlayerCursor(new Vector3(scenePositon.x, scenePositon.y, 0));
      
        }

    }

    IEnumerator RotateFlash()
    {
        while (isDragging)
        {
            yield return new WaitForSeconds(0.2f);
            Vector3 curDirection = (playerCursor.transform.position - prevPosition);

            //Debug.Log(curDirection);
            prevPosition = playerCursor.transform.position;
            if (curDirection != Vector3.zero)
            {
                float angleZ = Mathf.Atan2(curDirection.y, curDirection.x) * Mathf.Rad2Deg;
                //Debug.Log(angleZ);
                //playerCursor.transform.eulerAngles = new Vector3(0, 0, angleZ);
                targetRotation = Quaternion.Euler(0, 0, angleZ);
                playerCursor.transform.rotation = Quaternion.Slerp(playerCursor.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                gameManager.RotateInvisPlayerCursor(angleZ);
            }
        }
    }
    public void DragEnd(BaseEventData eventData)
    {
       
        playerCursor.SetActive(false);
        Cursor.visible = true;
        gameManager.ActivePlayerCursor(false);
        isDragging = false;
    }

}
