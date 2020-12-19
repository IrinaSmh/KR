using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Timers;

public class PlayerController : MonoBehaviour
{
    private int speed = 10;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI lifeCountText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI recordText;
    public GameObject winTextObject;
    public GameObject lostTextObject;

    private int count;
    private int lifeCount = 3;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private bool isNeedMove = true;
    private float startTime = 0;
    private bool isWin = false;

    public void setIsNeedMove(bool val)
    {
        isNeedMove = val;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        setLifeCountText();
        winTextObject.SetActive(false);
        lostTextObject.SetActive(false);
        setRecordText();
       
        if (Options.isChanged)
        {
            speed = Options.speed;
        }
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

        
    }

    void setRecordText()
    {
        if (Parametres.recosrIsNew)
        {
            Parametres.record = 1000;
            recordText.text = "Ваш рекорд: 0";
        }
        else recordText.text = "Ваш рекорд: " + Parametres.record.ToString();
    }

    void setTimerText()
    {
        if (isNeedMove)
        {
            startTime += Time.deltaTime;
            timerText.text = "Time: " + ((int)startTime).ToString();
        } else
        {
            int tmp = ((int)startTime);
            timerText.text = "Time: " + tmp.ToString();
            if (isWin)
            {
                if(Parametres.record > tmp)
                {
                    Parametres.record = tmp;
                    Parametres.recosrIsNew = false;
                }
            }
        }

    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString(); 
        if(count >= 30)
        {
            winTextObject.SetActive(true);
            isNeedMove = false;
            isWin = true;
        }
    }

    void setLifeCountText()
    {
        
        if (lifeCount < 0)
        {
            lostTextObject.SetActive(true);
            isNeedMove = false;
            lifeCountText.text = "Life: 0";
        } else
        {
            lifeCountText.text = "Life: " + lifeCount.ToString();
        }
    }

    void FixedUpdate()
    {
        setTimerText();

        if (isWin)
        {
            setRecordText();
        }

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

        if (rb.position.y < 0 || !isNeedMove)
        {
            if (isNeedMove) isNeedMove = false;
            if ((!isWin && lifeCount < 0) || (rb.position.y < 0 && !isWin)) lostTextObject.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
        else rb.constraints = RigidbodyConstraints.None;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;

            SetCountText();
        }

        if (other.gameObject.CompareTag("Danger"))
        {
            other.gameObject.SetActive(false);
            lifeCount -= 1;

            setLifeCountText();
        }

        if (other.gameObject.CompareTag("NewLife"))
        {
            other.gameObject.SetActive(false);
            lifeCount += 1;

            setLifeCountText();
        }
    }
}
