using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum OPERATION { ADDITION, SUBTRACTION, MULTIPLICATION, DIVISION }

public class Area : MonoBehaviour
{
    [SerializeField] int operationValue;
    [SerializeField] OPERATION operation;
    [SerializeField] TextMeshProUGUI operationText;

    // Start is called before the first frame update
    void Start()
    {
        string op = GetOperationSign();
        operationText.text = op + operationValue.ToString();
    }

    private string GetOperationSign()
    {
        if (operation == OPERATION.ADDITION) return "+";
        else if (operation == OPERATION.SUBTRACTION) return "-";
        else if (operation == OPERATION.MULTIPLICATION) return "×";
        else return "÷";
    }

    public int GetOperationValue()
    {
        return operationValue;
    }

    public OPERATION GetOperation()
    {
        return operation;
    }
}
