using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboTextBehaviour : MonoBehaviour
{
    private static int sortingOrder;
    private TextMeshProUGUI textMesh;
    private float dissapearTimer;
    private Color textColor;
    private const float dissapearTimerMax = 1f;
    Vector3 moveVector;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Setup(int comboCount)
    {
        //textMesh.sortingOrder = sortingOrder;
        textMesh.SetText("x" + comboCount.ToString());
        textColor = textMesh.color;
        dissapearTimer = dissapearTimerMax;
        moveVector = new Vector3(1, 1) * 30f;
    }
    void Update()
    {
        transform.position -= moveVector * Time.deltaTime;
        moveVector -= moveVector * 15f * Time.deltaTime;
        if (dissapearTimer > dissapearTimerMax * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        dissapearTimer -= Time.deltaTime;


        if (dissapearTimer < 0)
        {
            float dissapearSpeed = 3f;
            textColor.a -= dissapearSpeed * Time.deltaTime;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
