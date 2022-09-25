using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Start_ReactorUI : MonoBehaviour
{
    public GameObject startReactorUI;

    public List<Button> displayList;
    public List<Button> dialList;
    public List<int> questionList;
    public List<int> ansList;
    public List<int> ansInputList;

    public GameObject light1;
    public GameObject light2;
    public GameObject light3;
    public GameObject light4;
    public GameObject light5;

    public GameObject ansLight1;
    public GameObject ansLight2;
    public GameObject ansLight3;
    public GameObject ansLight4;
    public GameObject ansLight5;

    float currentTime;

    // ���� �ε���
    int firstIndex;
    int secondIndex;
    int thirdIndex;
    int fourthIndex;
    int fifthIndex;

    bool isStart;
    bool isFirst;
    bool isSecond;
    bool isThird;
    bool isFourth;
    bool isFifth;

    // �ε��� ���� ����
    bool isFirstCorrect;
    bool isSecondCorrect;
    bool isThirdCorrect;
    bool isFourthCorrect;
    bool isFifthCorrect;

    bool isFirstInput;
    bool isSecondInput;
    bool isThirdInput;
    bool isFourthInput;
    bool isFifthInput;

    public bool isMissionComplete;
    public GameObject taskCompletedUI;



    // Start is called before the first frame update

    // 1 2 3 4��°���� ���� �ؼ� �� ����� ���߸� ���
    // ù��°
    // �����ѹ��� ȣ���Ѵ�. �����ѹ��� �ε����� ans ����Ʈ�� �ø���

    void Start()
    {
        for (int i = 0; i < displayList.Count; i++)
        {
            displayList[i].gameObject.SetActive(false);
            dialList[i].interactable = false;
        }
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 1)
            {
                SetFirstIndex();

                isStart = false;
                isFirst = true;
                currentTime = 0;               
            }
        }

        if (isFirst)
        {
            ExecuteFirst();
        }

        if (isFirstInput)
        {
            ExecuteFirstInput();
        }

        if (isSecond)
        {
            ExecuteSecond();
        }

        if (isSecondInput)
        {
            ExecuteSecondInput();
        }

        if (isThird)
        {
            ExecuteThird();
        }

        if (isThirdInput)
        {
            ExecuteThirdInput();
        }

        if (isFourth)
        {
            ExecuteFourth();
        }

        if (isFourthInput)
        {
            ExecuteFourthInput();
        }

        if (isFifth)
        {
            ExecuteFifth();
        }

        if (isFifthInput)
        {
            ExecuteFifthInput();
        }

    }

    // �ε��� ���� �Լ�
    void SetFirstIndex()
    {
        int randomNum = UnityEngine.Random.Range(0, questionList.Count);
        firstIndex = questionList[randomNum];
        questionList.RemoveAt(randomNum);
        ansList.Add(firstIndex);
    }

    void SetSecondIndex()
    {
        int randomNum = UnityEngine.Random.Range(0, questionList.Count);
        secondIndex = questionList[randomNum];
        questionList.RemoveAt(randomNum);
        ansList.Add(secondIndex);
    }

    void SetThirdIndex()
    {
        int randomNum = UnityEngine.Random.Range(0, questionList.Count);
        thirdIndex = questionList[randomNum];
        questionList.RemoveAt(randomNum);
        ansList.Add(thirdIndex);
    }

    void SetFourthIndex()
    {
        int randomNum = UnityEngine.Random.Range(0, questionList.Count);
        fourthIndex = questionList[randomNum];
        questionList.RemoveAt(randomNum);
        ansList.Add(fourthIndex);
    }

    void setFifthIndex()
    {
        int randomNum = UnityEngine.Random.Range(0, questionList.Count);
        fifthIndex = questionList[randomNum];
        questionList.RemoveAt(randomNum);
        ansList.Add(fifthIndex);
    }

    // ù��°
    void ExecuteFirst()
    {
        // �����ѹ��� �ش��ϴ� �ε����� ���÷��� Ȱ��ȭ
        displayList[firstIndex].gameObject.SetActive(true);

        // ����Ʈ ��
        light1.SetActive(true);

        // �ð� ++
        currentTime += Time.deltaTime; 

        // ����ð��� 1�� ������ 
        if (currentTime >= 0.5f)
        {
            // �ش� ���÷��� �ڽ� ��Ȱ��ȭ
            displayList[firstIndex].gameObject.SetActive(false);
            // ����Ʈ ����
            light1.SetActive(false);            
            
            // 1�ʵ��� �����ִٰ� ��ǲ�ܰ�� �Ѿ
            if (currentTime >= 1.5f)
            {
                // ù��° ��
                isFirst = false;
                // ù��° ���� ������ ����
                isFirstInput = true;
                // ��ư Ȱ��ȭ
                ActivateDial();
                currentTime = 0;
            }
        }
    }

    void ExecuteSecond()
    {
        // ù��° Ȱ��ȭ
        displayList[ansList[0]].gameObject.SetActive(true);
        // 1�� ����Ʈ ��
        light1.SetActive(true);
        // �ð� ++
        currentTime += Time.deltaTime;
        if (currentTime >= 0.5f)
        {
            // ù��° ��Ȱ��ȭ
            displayList[ansList[0]].gameObject.SetActive(false);
        }
        if (currentTime >= 1)
        {
            // �ι�° Ȱ��ȭ
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }
        if (currentTime >= 1.5f)
        {
            // �ι�° ��Ȱ��ȭ
            displayList[ansList[1]].gameObject.SetActive(false);
            light1.SetActive(false);
            light2.SetActive(false);
        }
        if (currentTime >= 2)
        {
            isSecond = false;
            isSecondInput = true;
            ActivateDial();
            currentTime = 0;
        }
    }

    void ExecuteThird()
    {
        // �ð� ++
        currentTime += Time.deltaTime;

        // ù��° Ȱ��ȭ
        displayList[ansList[0]].gameObject.SetActive(true);
        light1.SetActive(true);

        // ù��° ��Ȱ��ȭ
        if (currentTime >= 0.5f) displayList[ansList[0]].gameObject.SetActive(false);

        // �ι�° Ȱ��ȭ
        if (currentTime >= 1)
        {
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }

       
        // �ι�° ��Ȱ��ȭ
        if (currentTime >= 1.5f) displayList[ansList[1]].gameObject.SetActive(false);

        // ����° Ȱ��ȭ
        if (currentTime >= 2)
        {
            displayList[ansList[2]].gameObject.SetActive(true);
            light3.SetActive(true);
        }


        // ����° ��Ȱ��ȭ
        if (currentTime >= 2.5f) 
        {
            displayList[ansList[2]].gameObject.SetActive(false);
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
        }

        if (currentTime >= 3)
        {
            isThird = false;
            isThirdInput = true;
            ActivateDial();
            currentTime = 0;
        }
    }

    void ExecuteFourth()
    {
        // �ð� ++
        currentTime += Time.deltaTime;

        // ù��° Ȱ��ȭ
        displayList[ansList[0]].gameObject.SetActive(true);
        light1.SetActive(true);

        // ù��° ��Ȱ��ȭ
        if (currentTime >= 0.5f) displayList[ansList[0]].gameObject.SetActive(false);

        // �ι�° Ȱ��ȭ
        if (currentTime >= 1)
        {
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }

        // �ι�° ��Ȱ��ȭ
        if (currentTime >= 1.5f) displayList[ansList[1]].gameObject.SetActive(false);

        // ����° Ȱ��ȭ
        if (currentTime >= 2)
        {
            displayList[ansList[2]].gameObject.SetActive(true);
            light3.SetActive(true);
        }

        // ����° ��Ȱ��ȭ
        if (currentTime >= 2.5f) displayList[ansList[2]].gameObject.SetActive(false);

        // �׹�° Ȱ��ȭ
        if (currentTime >= 3) 
        { 
            displayList[ansList[3]].gameObject.SetActive(true);
            light4.SetActive(true);
        }

        // �׹�° ��Ȱ��ȭ
        if (currentTime >= 3.5f)
        {
            displayList[ansList[3]].gameObject.SetActive(false);
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
            light4.SetActive(false);
        }

        if (currentTime >= 4)
        {
            isFourth = false;
            isFourthInput = true;
            ActivateDial();
            currentTime = 0;
        }
    }

    void ExecuteFifth()
    {
        // �ð� ++
        currentTime += Time.deltaTime;

        // ù��° Ȱ��ȭ
        displayList[ansList[0]].gameObject.SetActive(true);
        light1.SetActive(true);

        // ù��° ��Ȱ��ȭ
        if (currentTime >= 0.5f) displayList[ansList[0]].gameObject.SetActive(false);

        // �ι�° Ȱ��ȭ
        if (currentTime >= 1)
        {
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }

        // �ι�° ��Ȱ��ȭ
        if (currentTime >= 1.5f) displayList[ansList[1]].gameObject.SetActive(false);

        // ����° Ȱ��ȭ
        if (currentTime >= 2)
        {
            displayList[ansList[2]].gameObject.SetActive(true);
            light3.SetActive(true);
        }

        // ����° ��Ȱ��ȭ
        if (currentTime >= 2.5f) displayList[ansList[2]].gameObject.SetActive(false);

        // �׹�° Ȱ��ȭ
        if (currentTime >= 3)
        {
            displayList[ansList[3]].gameObject.SetActive(true);
            light4.SetActive(true);
        }

        // �׹�° ��Ȱ��ȭ
        if (currentTime >= 3.5f) displayList[ansList[3]].gameObject.SetActive(false);

        if (currentTime >= 4)
        {
            displayList[ansList[4]].gameObject.SetActive(true);
            light5.SetActive(true);
        }

        if (currentTime >= 4.5f)
        {
            displayList[ansList[4]].gameObject.SetActive(false);
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
            light4.SetActive(false);
            light5.SetActive(false);
        }

        if (currentTime >= 5)
        {
            isFifth = false;
            isFifthInput = true;
            ActivateDial();
            currentTime = 0;
        }
    }

    void ActivateDial()
    {
        // ���̾� ��ư ���� Ȱ��ȭ
        for (int i = 0; i < dialList.Count; i++)
        {
            dialList[i].interactable = true;
        }
    }

    void DeActivateDial()
    {
        // ���̾� ��ư ���� Ȱ��ȭ
        for (int i = 0; i < dialList.Count; i++)
        {
            dialList[i].interactable = false;
        }
    }

    void ExecuteFirstInput()
    {              
        // ���� ���߸�
        if (isFirstCorrect)
        {
            // ��ư ��Ȱ��ȭ�ϰ�
            DeActivateDial();

            currentTime += Time.deltaTime;
            // 1�� �Ŀ� 2�ܰ� ����
            if (currentTime >= 1)
            {
                // �ι�° �ε��� ����
                SetSecondIndex();

                ansLight1.SetActive(false);

                isFirstInput = false;
                isFirstCorrect = false;
                isSecond = true;
                currentTime = 0;
                // ��ǲ����Ʈ �ʱ�ȭ
                ansInputList.Clear();
            }          
        }
    }

    void ExecuteSecondInput()
    {
        if (isFirstCorrect && isSecondCorrect)
        {
            // ��ư ��Ȱ��ȭ�ϰ�
            DeActivateDial();
            
            currentTime += Time.deltaTime;
            // 1�� �Ŀ� 3�ܰ� ����
            if (currentTime >= 1)
            {
                // ����° ���� ����
                SetThirdIndex();

                ansLight1.SetActive(false);
                ansLight2.SetActive(false);

                isSecondInput = false;
                isFirstCorrect = false;
                isSecondCorrect = false;
                isThird = true;
                currentTime = 0;
                ansInputList.Clear();

            }
        }
    }

    void ExecuteThirdInput()
    {
        if (isFirstCorrect && isSecondCorrect && isThirdCorrect)
        {
            // ��ư ��Ȱ��ȭ�ϰ�
            DeActivateDial();
           
            currentTime += Time.deltaTime;
            // 1�� �Ŀ� 3�ܰ� ����
            if (currentTime >= 1)
            {
                // �׹�° ���� ����
                SetFourthIndex();

                ansLight1.SetActive(false);
                ansLight2.SetActive(false);
                ansLight3.SetActive(false);

                isThirdInput = false;
                isFirstCorrect = false;
                isSecondCorrect = false;
                isThirdCorrect = false;
                isFourth = true;
                currentTime = 0;
                ansInputList.Clear();

            }
        }
    }

    void ExecuteFourthInput()
    {
        if (isFirstCorrect && isSecondCorrect && isThirdCorrect && isFourthCorrect)
        {
            // ��ư ��Ȱ��ȭ�ϰ�
            DeActivateDial();

            currentTime += Time.deltaTime;
            // 1�� �Ŀ� 3�ܰ� ����
            if (currentTime >= 1)
            {
                setFifthIndex();

                ansLight1.SetActive(false);
                ansLight2.SetActive(false);
                ansLight3.SetActive(false);
                ansLight4.SetActive(false);

                isFourthInput = false;
                isFirstCorrect = false;
                isSecondCorrect = false;
                isThirdCorrect = false;
                isFourthCorrect = false;
               
                isFifth = true;
                currentTime = 0;
                ansInputList.Clear();
                // startReactorUI.SetActive(false);
            }
        }
    }

    void ExecuteFifthInput()
    {
        if (isFirstCorrect && isSecondCorrect && isThirdCorrect && isFourthCorrect && isFifthCorrect)
        {
            // ��ư ��Ȱ��ȭ�ϰ�
            DeActivateDial();
            isMissionComplete = true;
            JM_MissionStatus.instance.isMissionDone = true;
            taskCompletedUI.SetActive(true);

            currentTime += Time.deltaTime;
            // 1�� �Ŀ� 3�ܰ� ����
            if (currentTime >= 1)
            {
                ansLight1.SetActive(false);
                ansLight2.SetActive(false);
                ansLight3.SetActive(false);
                ansLight4.SetActive(false);
                ansLight5.SetActive(false);

                isFifthInput = false;
                isFirstCorrect = false;
                isSecondCorrect = false;
                isThirdCorrect = false;
                isFourthCorrect = false;
                isFifthCorrect = false;
                

                currentTime = 0;
                ansInputList.Clear();
                startReactorUI.SetActive(false);
            }
        }
    }

    public GameObject button;

    public void OnClickDial()
    {
        // ���� ��ư ��������
        button = EventSystem.current.currentSelectedGameObject;

        if (isFirstInput)
        { 
            
            // ������ǲ����Ʈ�� ���ϱ�
            ansInputList.Add(Int32.Parse(button.transform.GetChild(0).GetComponent<Text>().text));

            if (ansInputList.Count == 1)
            {
                ansLight1.SetActive(true);
            }

            if (ansInputList[0] == ansList[0]) isFirstCorrect = true;
            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();

        }

        if (isSecondInput)
        {
            // ������ǲ����Ʈ�� ���ϱ�
            ansInputList.Add(Int32.Parse(button.transform.GetChild(0).GetComponent<Text>().text));

            if (ansInputList.Count == 1)
            {
                ansLight1.SetActive(true);
            }
            if (ansInputList.Count == 2)
            {
                ansLight2.SetActive(true);
            }


            // ù��° �´���
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;

            // �ι�° �´���
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;

            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();
            if (ansInputList.Count == 2 && ansInputList[1] != ansList[1]) Reset();
        }

        if (isThirdInput)
        {
            // ������ǲ����Ʈ�� ���ϱ�
            ansInputList.Add(Int32.Parse(button.transform.GetChild(0).GetComponent<Text>().text));

            if (ansInputList.Count == 1)
            {
                ansLight1.SetActive(true);
            }
            if (ansInputList.Count == 2)
            {
                ansLight2.SetActive(true);
            }
            if (ansInputList.Count == 3)
            {
                ansLight3.SetActive(true);
            }

            // ù��° �´���
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;

            // �ι�° �´���
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;

            // ����° �´���
            if (ansInputList.Count == 3 && ansInputList[2] == ansList[2]) isThirdCorrect = true;

            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();
            if (ansInputList.Count == 2 && ansInputList[1] != ansList[1]) Reset();
            if (ansInputList.Count == 3 && ansInputList[2] != ansList[2]) Reset();
        }

        if (isFourthInput)
        {
            // ������ǲ����Ʈ�� ���ϱ�
            ansInputList.Add(Int32.Parse(button.transform.GetChild(0).GetComponent<Text>().text));

            if (ansInputList.Count == 1)
            {
                ansLight1.SetActive(true);
            }
            if (ansInputList.Count == 2)
            {
                ansLight2.SetActive(true);
            }
            if (ansInputList.Count == 3)
            {
                ansLight3.SetActive(true);
            }
            if (ansInputList.Count == 4)
            {
                ansLight4.SetActive(true);
            }

            // ù��° �´���
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;

            // �ι�° �´���
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;

            // ����° �´���
            if (ansInputList.Count == 3 && ansInputList[2] == ansList[2]) isThirdCorrect = true;

            if (ansInputList.Count == 4 && ansInputList[3] == ansList[3]) isFourthCorrect = true;

            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();
            if (ansInputList.Count == 2 && ansInputList[1] != ansList[1]) Reset();
            if (ansInputList.Count == 3 && ansInputList[2] != ansList[2]) Reset();
            if (ansInputList.Count == 4 && ansInputList[3] != ansList[3]) Reset();
        }

        if (isFifthInput)
        {
            ansInputList.Add(Int32.Parse(button.transform.GetChild(0).GetComponent<Text>().text));

            if (ansInputList.Count == 1)
            {
                ansLight1.SetActive(true);
            }
            if (ansInputList.Count == 2)
            {
                ansLight2.SetActive(true);
            }
            if (ansInputList.Count == 3)
            {
                ansLight3.SetActive(true);
            }
            if (ansInputList.Count == 4)
            {
                ansLight4.SetActive(true);
            }
            if (ansInputList.Count == 5)
            {
                ansLight5.SetActive(true);
            }

            // ù��° �´���
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;
            // �ι�° �´���
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;
            // ����° �´���
            if (ansInputList.Count == 3 && ansInputList[2] == ansList[2]) isThirdCorrect = true;
            if (ansInputList.Count == 4 && ansInputList[3] == ansList[3]) isFourthCorrect = true;
            if (ansInputList.Count == 5 && ansInputList[4] == ansList[4]) isFifthCorrect = true;

            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();
            if (ansInputList.Count == 2 && ansInputList[1] != ansList[1]) Reset();
            if (ansInputList.Count == 3 && ansInputList[2] != ansList[2]) Reset();
            if (ansInputList.Count == 4 && ansInputList[3] != ansList[3]) Reset();
            if (ansInputList.Count == 5 && ansInputList[4] != ansList[4]) Reset();

        }


        // �ι�° ����° �׹�°�� ansList �ϳ��� �ö󰡸鼭 ������Ʈ�Ǿߵȴ�

    }

    private void Reset()
    {
        questionList.Clear();
        ansList.Clear();
        ansInputList.Clear();

        ansLight1.SetActive(false);
        ansLight2.SetActive(false);
        ansLight3.SetActive(false);
        ansLight4.SetActive(false);
        ansLight5.SetActive(false);

        isFirstInput = false;
        isSecondInput = false;
        isThirdInput = false;
        isFourthInput = false;
        isFifthInput = false;

        isFirstCorrect = false;
        isSecondCorrect = false;
        isThirdCorrect = false;
        isFourthCorrect = false;
        isFifthCorrect = false;

        for (int i = 0; i < displayList.Count; i++)
        {
            questionList.Add(i);
        }

        isStart = true;
    }

}
