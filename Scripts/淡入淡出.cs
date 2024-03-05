using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 淡入淡出 : MonoBehaviour
{
    public Texture img;//黑色图片

    private static string 场景;
    private static float alpha=0f;
    private static bool 淡出=false;//静态方法只能调用静态属性，所以加static
    private static bool 淡入=false;
    public float speed = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
        if(淡出){
            alpha+=speed*Time.deltaTime;
            if(alpha>=1){
                淡入=true;
                淡出=false;
                SceneManager.LoadScene(场景);//切换场景
            }
        }
        if(淡入){
            alpha-=speed*Time.deltaTime;
            if(alpha<=0){
                淡入=false;
            }
        }
        GUI.color=new Color(GUI.color.r,GUI.color.g,GUI.color.b,alpha);
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),img);//引用黑色图片设定宽高为屏幕大小
    }
    public static void 切换场景(string scene){//静态类能在其他脚本中直接引用，详情查找static
        淡出=true;
        场景 =scene;
    }
    
    public void 开始按钮弹起() 
    {
        淡入淡出.切换场景("游戏界面");
    }

    public void 结束游戏按钮()
    {
        Application.Quit();
    }
    
}

