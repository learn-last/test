using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AIMain : MonoBehaviour
{
    private const int boardWidth = 10;
    private const int boardHeight = 20;

    public GameObject[] myTetris;//俄罗斯方块预制体集合
    private GameObject nowT;//当前生成的方块
    private bool iscubeDrop;//方块是否下落？
    private Transform[,] tabTetris = new Transform[boardWidth, boardHeight];
    private int myScore = 0;//分数
    private TMP_Text scoreText;//分数显示
    private GameObject nextT;//下一个方块显示

    private bool isGameOver;//判断游戏结束
    private GameObject gameoverText;//“游戏结束”文本框；用于显示/隐藏
    private GameObject againGame;//“再来一局”按钮
    private GameObject exitGame;//“结束游戏"按钮
    
    private GameObject text暂停;

    //操作按钮
    private GameObject inputLeft;
    private GameObject inputRight;
    private GameObject inputUp;
    private GameObject inputDown;

    private int randomNum;


    private void createBoard()//创建背景板
    {
        for (int i = 0; i < boardWidth; i++)
            for (int j = 0; j < boardHeight; j++)
            {
                GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
                o.transform.localScale = new Vector3(0.95f, 0.95f, 1);//改变生成方块大小，使其有间隔；
                o.transform.position = new Vector3(i, j, 1);//根据“i”和“j”设定生成位置；
            }

    }

    bool 加速中断;//用于新创建的方块不受，之前输入下键影响
    int nextNum;
    //生成俄罗斯方块
    private void newTetris()
    {
        Destroy(nextT);
        if (isGameOver == true)//如果游戏结束，不再执行后续内容
        {
            return;
        }

        加速中断 = false;

        // nextNum = UnityEngine.Random.Range(0, myTetris.Length);//创建一个0到“myTetris”数组长度的随机数字；

        // nowT = Instantiate(myTetris[randomNum], new Vector3(4, 18, 0), Quaternion.identity) as GameObject;

        if (nextT != null)
        {
            randomNum = nextNum;
            nowT = Instantiate(myTetris[randomNum], new Vector3(4, 18, 0), Quaternion.identity) as GameObject;
            nextNum = UnityEngine.Random.Range(0, myTetris.Length);//创建一个0到“myTetris”数组长度的随机数字；
        }
        else
        {
            randomNum = UnityEngine.Random.Range(0, myTetris.Length);//创建一个0到“myTetris”数组长度的随机数字；
            nowT = Instantiate(myTetris[randomNum], new Vector3(4, 18, 0), Quaternion.identity) as GameObject;
        }
        
        checkGameOver();

        nextT = Instantiate(myTetris[nextNum], new Vector3(12, 15, 0), Quaternion.identity) as GameObject;
        nextT.transform.localScale /=2;

        // //显示下一个降落的方块
        // if (nextT != null)//不为空执行，避免第一次运行出现错误
        // {
        //     nowT = nextT;
        //     nowT.transform.position = new Vector3(4, 18, 0);
        // }
        // else
        // {
        //     nowT = Instantiate(myTetris[randomNum], new Vector3(4, 18, 0), Quaternion.identity) as GameObject;
        // }

        // checkGameOver();
        // nextT = Instantiate(myTetris[randomNum], new Vector3(11, 22, 0), Quaternion.identity) as GameObject;
    }

    //判断游戏是否结束，是：显示“游戏结束”时的UI界面
    private void checkGameOver()
    {
        if (isNextCaseBalid() == false)
        {
            isGameOver = true;

            //结束界面UI显示
            gameoverText.SetActive(true);
            againGame.SetActive(true);
            exitGame.SetActive(true);

            //操作UI隐藏
            inputLeft.SetActive(false);
            inputRight.SetActive(false);
            inputUp.SetActive(false);
            inputDown.SetActive(false);
        }
    }

    //方块下落
    private IEnumerator cubeDrop()//IEnumerator：迭代，计数器之类的；目前我简单的理解为逐步循环执行；
    {
        iscubeDrop = true;
        yield return new WaitForSeconds(0.45f);//相当于计时器，系统自动延迟0.5秒后执行后面内容；
        nowT.transform.position = new Vector3(nowT.transform.position.x, nowT.transform.position.y - 1, 0);

        if (isNextCaseBalid() == false)
        {
            nowT.transform.position = new Vector3(nowT.transform.position.x, nowT.transform.position.y + 1, 0);
            fixCubePos();
        }
        iscubeDrop = false;
    }

    /*
        //键盘上、下、左、右控制俄罗斯方块移动、旋转；
        private void cubeController()
        {
            if(Input.GetKeyDown("left"))
            {    
                nowT.transform.position = new Vector3(nowT.transform.position.x-1,nowT.transform.position.y,0);

                if(isNextCaseBalid() == false)
                    nowT.transform.position = new Vector3(nowT.transform.position.x+1,nowT.transform.position.y,0);

            }

            if(Input.GetKeyDown("right"))
            {
                nowT.transform.position = new Vector3(nowT.transform.position.x+1,nowT.transform.position.y,0);

                if(isNextCaseBalid() == false)
                    nowT.transform.position = new Vector3(nowT.transform.position.x-1,nowT.transform.position.y,0);
            }

            if(Input.GetKeyDown("up"))
            {
                nowT.transform.Rotate(new Vector3(0,0,-90));

                if(isNextCaseBalid() == false)
                    nowT.transform.Rotate(new Vector3(0,0,+90));
            }
            if(Input.GetKey("down"))
            {
                nowT.transform.position = new Vector3(nowT.transform.position.x,nowT.transform.position.y-1,0);

                if(isNextCaseBalid() == false)
                {    
                    nowT.transform.position = new Vector3(nowT.transform.position.x,nowT.transform.position.y+1,0);
                    fixCubePos();
                }
            }
        }
    */

    //按钮控制方块
    public void 左()
    {
        nowT.transform.position = new Vector3(nowT.transform.position.x - 1, nowT.transform.position.y, 0);

        if (isNextCaseBalid() == false)
            nowT.transform.position = new Vector3(nowT.transform.position.x + 1, nowT.transform.position.y, 0);
    }
    public void 右()
    {
        nowT.transform.position = new Vector3(nowT.transform.position.x + 1, nowT.transform.position.y, 0);

        if (isNextCaseBalid() == false)
            nowT.transform.position = new Vector3(nowT.transform.position.x - 1, nowT.transform.position.y, 0);
    }
    
        bool blpd = false;
    public void 上()
    {
        if(randomNum == 1 ||randomNum ==2 ||randomNum ==5)//判断生成的是不是旋转异常的方块，是的话再执行
        {
            nowT.transform.Rotate(new Vector3(0, 0, -90));
        }
        else if(randomNum != 4)//不是“O”型方块的话继续执行
        {
            blpd = !blpd;
            if(blpd == true)
            {
                nowT.transform.Rotate(new Vector3(0, 0, -90));
            }
            else
            {
                nowT.transform.Rotate(new Vector3(0, 0, +90));
            }
            
        }

        if (isNextCaseBalid() == false)
            nowT.transform.Rotate(new Vector3(0, 0, +90));
    }
    public void 下()
    {
        加速中断 = true;
    }
    public void 下_弹起()
    {
        加速中断 = false;
    }
    private void jiasuxialuo()
    {
        if(加速中断 == true)
        {
            nowT.transform.position = new Vector3(nowT.transform.position.x, nowT.transform.position.y - 1, 0);

            if (isNextCaseBalid() == false)
            {
                nowT.transform.position = new Vector3(nowT.transform.position.x, nowT.transform.position.y + 1, 0);
                fixCubePos();
                return;
            }
        }
    }
        bool 暂停判断 = true;
    //暂停按钮
    public void 暂停按钮()
    {
        if(暂停判断 == true)
        {
            暂停判断 = !暂停判断;
            Time.timeScale = 0;

            text暂停.SetActive(true);//“游戏暂停”显示;

            //操作UI隐藏
            inputLeft.SetActive(false);
            inputRight.SetActive(false);
            inputUp.SetActive(false);
            inputDown.SetActive(false);
        }
        else
        {
            暂停判断 = !暂停判断;
            Time.timeScale = 1;
            
            text暂停.SetActive(false);//“游戏暂停”隐藏;
            
            //操作UI隐藏
            inputLeft.SetActive(true);
            inputRight.SetActive(true);
            inputUp.SetActive(true);
            inputDown.SetActive(true);
        }
    }

    //将方块位置坐标取整，防止出现类似“0.9999”之类的坐标。
    private Vector3 roundVector(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
    }

    //判断方块的移动是否会超出边界，并返回bool值
    private bool isNextCaseBalid()
    {
        int tCount = nowT.transform.childCount;
        for (int i = 0; i < tCount; i++)
        {
            Vector3 rPos = roundVector(nowT.transform.GetChild(i).position);
            nowT.transform.GetChild(i).position = rPos;

            if (rPos.x < 0 || rPos.x > boardWidth - 1 || rPos.y < 0 || rPos.y > boardHeight - 1)
                return false;

            if (tabTetris[(int)rPos.x, (int)rPos.y] != null)
                return false;
        }
        return true;
    }

    private void fixCubePos()
    {
        int tCount = nowT.transform.childCount;
        Transform[] myCubes = new Transform[tCount];
        for (int i = 0; i < tCount; i++)
            myCubes[i] = nowT.transform.GetChild(i);

        //为每个子类方块设置信息
        foreach (Transform c in myCubes)
        {
            Vector3 rPos = roundVector(c.position);//将方块位置坐标整数化；
            c.position = rPos;

            tabTetris[(int)rPos.x, (int)rPos.y] = c;
            c.parent = null;
        }

        //刷新游戏物件并生成新方块（删除空白游戏物件）
        Destroy(nowT);

        deleteRow();
        newTetris();
    }

    //判断满行后消除行
    private void deleteRow()
    {
        for (int y = boardHeight - 1; y >= 0; y--)
        {
            if (isRowfull(y) == true)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    Destroy(tabTetris[x, y].gameObject);//删除此行方块
                    tabTetris[x, y] = null;//再将此行清理一遍
                }

                UpdateScore();//分数增加；

                rowDown(y);
            }
        }
    }
    //判断行是否有空缺，有返回true，无返回false
    private bool isRowfull(int y)
    {
        for (int i = 0; i < boardWidth; i++)
        {
            if (tabTetris[i, y] == null)
            {
                return false;
            }
        }
        return true;
    }
    //检测是否有一行全空，有的话下移一格
    private void rowDown(int y)
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = y; j < boardHeight; j++)
            {
                if (tabTetris[i, j] != null)
                {
                    tabTetris[i, j - 1] = tabTetris[i, j];
                    tabTetris[i, j] = null;
                    tabTetris[i, j - 1].position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    //分数计算，并显示
    private void UpdateScore()
    {
        myScore += 5;
        scoreText.text = myScore.ToString() + "分";
    }

    // //设置边框防止俄罗斯方块移动出去；
    // private void checkCubePos()
    // {
    //     int tCount =nowT.transform.childCount;

    //     for(int i = 0;i<tCount;i++)
    //     {
    //         if((int)nowT.transform.GetChild(i).transform.position.x<0)
    //             nowT.transform.position = new Vector3(nowT.transform.position.x+1,nowT.transform.position.y,0);

    //         if((int)nowT.transform.GetChild(i).transform.position.x>boardWidth-1)
    //             nowT.transform.position = new Vector3(nowT.transform.position.x-1,nowT.transform.position.y,0);
    //     }
    // }

    /**************************************************************** 游戏执行 ****************************************************************/
    void Start()
    {
        scoreText = GameObject.Find("score").GetComponent<TMP_Text>();//获得分数显示文本框;可编辑其中文字

        gameoverText = GameObject.Find("lose");//获得游戏结束显示文本框；不可编辑其中文字
        gameoverText.SetActive(false);//将其隐藏;

        text暂停 = GameObject.Find("暂停_文本");
        text暂停.SetActive(false);

        againGame = GameObject.Find("再来一局");
        againGame.SetActive(false);
        exitGame = GameObject.Find("结束游戏");
        exitGame.SetActive(false);

        inputLeft = GameObject.Find("左");
        inputLeft.SetActive(true);
        inputRight = GameObject.Find("右");
        inputRight.SetActive(true);
        inputUp = GameObject.Find("上");
        inputUp.SetActive(true);
        inputDown = GameObject.Find("下");
        inputDown.SetActive(true);

        createBoard();
        newTetris();
    }

    // Update is called once per frame
    void Update()
    {
        if (nowT == null || isGameOver == true)
            return;
        
        jiasuxialuo();//判断是否在按着下键，是：继续执行按下操作；

        // cubeController();//按键控制
        // checkCubePos();

        if (iscubeDrop == false)
            StartCoroutine(cubeDrop());//调用“IEnumerator”类的方法需要这个方法；
    }
}
