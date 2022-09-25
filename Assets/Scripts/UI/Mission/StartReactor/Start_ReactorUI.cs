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

    // 랜덤 인덱스
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

    // 인덱스 맞춘 여부
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

    // 1 2 3 4번째까지 ㄱㄱ 해서 다 제대로 맞추면 통과
    // 첫번째
    // 랜덤넘버를 호출한다. 랜덤넘버의 인덱스를 ans 리스트에 올린다

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

    // 인덱스 지정 함수
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

    // 첫번째
    void ExecuteFirst()
    {
        // 랜덤넘버에 해당하는 인덱스의 디스플레이 활성화
        displayList[firstIndex].gameObject.SetActive(true);

        // 라이트 온
        light1.SetActive(true);

        // 시간 ++
        currentTime += Time.deltaTime; 

        // 현재시간이 1초 지나면 
        if (currentTime >= 0.5f)
        {
            // 해당 디스플레이 박스 비활성화
            displayList[firstIndex].gameObject.SetActive(false);
            // 라이트 오프
            light1.SetActive(false);            
            
            // 1초동안 꺼져있다가 인풋단계로 넘어감
            if (currentTime >= 1.5f)
            {
                // 첫번째 끝
                isFirst = false;
                // 첫번째 정답 누르는 상태
                isFirstInput = true;
                // 버튼 활성화
                ActivateDial();
                currentTime = 0;
            }
        }
    }

    void ExecuteSecond()
    {
        // 첫번째 활성화
        displayList[ansList[0]].gameObject.SetActive(true);
        // 1번 라이트 온
        light1.SetActive(true);
        // 시간 ++
        currentTime += Time.deltaTime;
        if (currentTime >= 0.5f)
        {
            // 첫번째 비활성화
            displayList[ansList[0]].gameObject.SetActive(false);
        }
        if (currentTime >= 1)
        {
            // 두번째 활성화
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }
        if (currentTime >= 1.5f)
        {
            // 두번째 비활성화
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
        // 시간 ++
        currentTime += Time.deltaTime;

        // 첫번째 활성화
        displayList[ansList[0]].gameObject.SetActive(true);
        light1.SetActive(true);

        // 첫번째 비활성화
        if (currentTime >= 0.5f) displayList[ansList[0]].gameObject.SetActive(false);

        // 두번째 활성화
        if (currentTime >= 1)
        {
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }

       
        // 두번째 비활성화
        if (currentTime >= 1.5f) displayList[ansList[1]].gameObject.SetActive(false);

        // 세번째 활성화
        if (currentTime >= 2)
        {
            displayList[ansList[2]].gameObject.SetActive(true);
            light3.SetActive(true);
        }


        // 세번째 비활성화
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
        // 시간 ++
        currentTime += Time.deltaTime;

        // 첫번째 활성화
        displayList[ansList[0]].gameObject.SetActive(true);
        light1.SetActive(true);

        // 첫번째 비활성화
        if (currentTime >= 0.5f) displayList[ansList[0]].gameObject.SetActive(false);

        // 두번째 활성화
        if (currentTime >= 1)
        {
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }

        // 두번째 비활성화
        if (currentTime >= 1.5f) displayList[ansList[1]].gameObject.SetActive(false);

        // 세번째 활성화
        if (currentTime >= 2)
        {
            displayList[ansList[2]].gameObject.SetActive(true);
            light3.SetActive(true);
        }

        // 세번째 비활성화
        if (currentTime >= 2.5f) displayList[ansList[2]].gameObject.SetActive(false);

        // 네번째 활성화
        if (currentTime >= 3) 
        { 
            displayList[ansList[3]].gameObject.SetActive(true);
            light4.SetActive(true);
        }

        // 네번째 비활성화
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
        // 시간 ++
        currentTime += Time.deltaTime;

        // 첫번째 활성화
        displayList[ansList[0]].gameObject.SetActive(true);
        light1.SetActive(true);

        // 첫번째 비활성화
        if (currentTime >= 0.5f) displayList[ansList[0]].gameObject.SetActive(false);

        // 두번째 활성화
        if (currentTime >= 1)
        {
            displayList[ansList[1]].gameObject.SetActive(true);
            light2.SetActive(true);
        }

        // 두번째 비활성화
        if (currentTime >= 1.5f) displayList[ansList[1]].gameObject.SetActive(false);

        // 세번째 활성화
        if (currentTime >= 2)
        {
            displayList[ansList[2]].gameObject.SetActive(true);
            light3.SetActive(true);
        }

        // 세번째 비활성화
        if (currentTime >= 2.5f) displayList[ansList[2]].gameObject.SetActive(false);

        // 네번째 활성화
        if (currentTime >= 3)
        {
            displayList[ansList[3]].gameObject.SetActive(true);
            light4.SetActive(true);
        }

        // 네번째 비활성화
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
        // 다이얼 버튼 전부 활성화
        for (int i = 0; i < dialList.Count; i++)
        {
            dialList[i].interactable = true;
        }
    }

    void DeActivateDial()
    {
        // 다이얼 버튼 전부 활성화
        for (int i = 0; i < dialList.Count; i++)
        {
            dialList[i].interactable = false;
        }
    }

    void ExecuteFirstInput()
    {              
        // 정답 맞추면
        if (isFirstCorrect)
        {
            // 버튼 비활성화하고
            DeActivateDial();

            currentTime += Time.deltaTime;
            // 1초 후에 2단계 실행
            if (currentTime >= 1)
            {
                // 두번째 인덱스 지정
                SetSecondIndex();

                ansLight1.SetActive(false);

                isFirstInput = false;
                isFirstCorrect = false;
                isSecond = true;
                currentTime = 0;
                // 인풋리스트 초기화
                ansInputList.Clear();
            }          
        }
    }

    void ExecuteSecondInput()
    {
        if (isFirstCorrect && isSecondCorrect)
        {
            // 버튼 비활성화하고
            DeActivateDial();
            
            currentTime += Time.deltaTime;
            // 1초 후에 3단계 실행
            if (currentTime >= 1)
            {
                // 세번째 숫자 지정
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
            // 버튼 비활성화하고
            DeActivateDial();
           
            currentTime += Time.deltaTime;
            // 1초 후에 3단계 실행
            if (currentTime >= 1)
            {
                // 네번째 숫자 지정
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
            // 버튼 비활성화하고
            DeActivateDial();

            currentTime += Time.deltaTime;
            // 1초 후에 3단계 실행
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
            // 버튼 비활성화하고
            DeActivateDial();
            isMissionComplete = true;
            JM_MissionStatus.instance.isMissionDone = true;
            taskCompletedUI.SetActive(true);

            currentTime += Time.deltaTime;
            // 1초 후에 3단계 실행
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
        // 누른 버튼 가져오기
        button = EventSystem.current.currentSelectedGameObject;

        if (isFirstInput)
        { 
            
            // 정답인풋리스트에 더하기
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
            // 정답인풋리스트에 더하기
            ansInputList.Add(Int32.Parse(button.transform.GetChild(0).GetComponent<Text>().text));

            if (ansInputList.Count == 1)
            {
                ansLight1.SetActive(true);
            }
            if (ansInputList.Count == 2)
            {
                ansLight2.SetActive(true);
            }


            // 첫번째 맞는지
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;

            // 두번째 맞는지
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;

            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();
            if (ansInputList.Count == 2 && ansInputList[1] != ansList[1]) Reset();
        }

        if (isThirdInput)
        {
            // 정답인풋리스트에 더하기
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

            // 첫번째 맞는지
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;

            // 두번째 맞는지
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;

            // 세번째 맞는지
            if (ansInputList.Count == 3 && ansInputList[2] == ansList[2]) isThirdCorrect = true;

            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();
            if (ansInputList.Count == 2 && ansInputList[1] != ansList[1]) Reset();
            if (ansInputList.Count == 3 && ansInputList[2] != ansList[2]) Reset();
        }

        if (isFourthInput)
        {
            // 정답인풋리스트에 더하기
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

            // 첫번째 맞는지
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;

            // 두번째 맞는지
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;

            // 세번째 맞는지
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

            // 첫번째 맞는지
            if (ansInputList.Count == 1 && ansInputList[0] == ansList[0]) isFirstCorrect = true;
            // 두번째 맞는지
            if (ansInputList.Count == 2 && ansInputList[1] == ansList[1]) isSecondCorrect = true;
            // 세번째 맞는지
            if (ansInputList.Count == 3 && ansInputList[2] == ansList[2]) isThirdCorrect = true;
            if (ansInputList.Count == 4 && ansInputList[3] == ansList[3]) isFourthCorrect = true;
            if (ansInputList.Count == 5 && ansInputList[4] == ansList[4]) isFifthCorrect = true;

            if (ansInputList.Count == 1 && ansInputList[0] != ansList[0]) Reset();
            if (ansInputList.Count == 2 && ansInputList[1] != ansList[1]) Reset();
            if (ansInputList.Count == 3 && ansInputList[2] != ansList[2]) Reset();
            if (ansInputList.Count == 4 && ansInputList[3] != ansList[3]) Reset();
            if (ansInputList.Count == 5 && ansInputList[4] != ansList[4]) Reset();

        }


        // 두번째 세번째 네번째는 ansList 하나씩 올라가면서 업데이트되야된다

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
