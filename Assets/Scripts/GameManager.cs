using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーン遷移の追加
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioClip sound4;
    public AudioClip sound5;
    AudioSource audioSource;

    //最大HPと現在のHP。
    int maxHp = 300;
    int currentHp;
    //Sliderを入れる
    public Slider slider;

    // スコア関連
    public Text scoreText;
    private int score;

    // 今回の追加
    public int currentScore;
    public int clearScore = 1500;
    // public int losspoint;

    //変数の作成//

    Spawner spawner;//スポナー
    Block activeBlock;//生成されたブロック格納

    //変数の作成//
    //入力受付タイマー（3種類）
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;

    //入力インターバル（3種類）
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;


    [SerializeField]
    private float dropInterval = 0.25f;//次にブロックが落ちるまでのインターバル時間
    float nextdropTimer;//次にブロックが落ちるまでの時間

    //変数の作成//
    //ボードのスクリプトを格納
    Board board;

    //変数の作成//
    //パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject gameClearPanel;

    //ゲームオーバー判定
    bool gameOver;
    
    //ゲームクリアー判定
    bool gameClear;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        
        //スポナーオブジェクトをスポナー変数に格納するコードの記述
        spawner = GameObject. FindObjectOfType<Spawner>();

         //ボードを変数に格納する
        board = GameObject. FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);


        //タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;


        //スポナークラスからブロッック生成関数を呼んで変数に格納する
        if(!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

        //Sliderを満タンにする。
        slider.value = 0;
        //現在のHPを最大HPと同じに。
        //currentHp = maxHp;
        //Debug.Log("Start currentHp : " + currentHp);

        //ゲームオーバーパネルの非表示設定
        if(gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }

        //ゲームクリアパネルの非表示設定
        if(gameClearPanel.activeInHierarchy)
        {
            gameClearPanel.SetActive(false);
        }

        Initialize();
    } 

    // ゲーム開始前の状態に戻す
    private void Initialize()
    {
        // スコアを0に戻す
        score = 0;

    }

    // //ColliderオブジェクトのIsTriggerにチェック入れること。
    // private void OnTriggerEnter(Collider collider)
    // {
    //     //Enemyタグのオブジェクトに触れると発動
    //     if (collider.gameObject.tag == "Enemy")
    //     {
    //         //ダメージは1～100の中でランダムに決める。
    //         DownScore = damage;
    //         Debug.Log("damage : " + damage);

    //         //現在のHPからダメージを引く
    //         currentHp = currentHp - damage;
    //         Debug.Log("After currentHp : " + currentHp);

    //         //最大HPにおける現在のHPをSliderに反映。
    //         //int同士の割り算は小数点以下は0になるので、
    //         //(float)をつけてfloatの変数として振舞わせる。
    //         slider.value = (float)currentHp / (float)maxHp; ;
    //         Debug.Log("slider.value : " + slider.value);
    //     }
    // }

    


    private void Update()
    {
        // if (Destroy)
        // {
        //      image.fillAmount -= losspoint;
        
        /*else 
        {
 
            image.fillAmount += Time.deltaTime;
        }*/


        // if(gameOver)
        // {
        //     return;
        // }

        // if(gameClear)
        // {
        //     return;
        // }

        PlayerInput();

        //Updateでの時間の判定をして判定次第で落下関数を呼ぶ

        if(Time.time > nextdropTimer)//落下速度の制限
        {
            nextdropTimer = Time.time + dropInterval;

            if(activeBlock)//中身ある時移動する関数を呼ぶ
            {
                activeBlock. MoveDown();//生成されたオブジェクトは下に移動する
            
            
                //UpdateでBoardクラスの関数を呼び出してボードから出ていないか確認
                if(!board. CheckPosition(activeBlock))
                {
                    activeBlock. MoveUp();

                    board. SaveBlockInGrid(activeBlock);

                    activeBlock = spawner. SpawnBlock();
                }
            }

        }
    }
    
    
    


    //関数の作成//
    //キーの入力を検知してブロックを動かす関数
    //ボードの底に着いた時に次のブロックを生成する関数

    void PlayerInput()
    {
        if(Input.GetKey(KeyCode.RightArrow) && (Time.time > nextKeyLeftRightTimer) ||  Input.GetKeyDown(KeyCode.RightArrow))
        {
            activeBlock.MoveRight();//右に動かす
            // audioSource.PlayOneShot(sound5);

            nextKeyLeftRightTimer = Time.time +nextKeyLeftRightInterval;

            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if(Input.GetKey(KeyCode.LeftArrow) && (Time.time > nextKeyLeftRightTimer) ||  Input.GetKeyDown(KeyCode.LeftArrow))
        {
            activeBlock.MoveLeft();//左に動かす
            // audioSource.PlayOneShot(sound5);

            nextKeyLeftRightTimer = Time.time +nextKeyLeftRightInterval;

            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if(Input.GetKey(KeyCode.UpArrow) && (Time.time > nextKeyRotateTimer))
        {
            activeBlock.RotateRight();
            // audioSource.PlayOneShot(sound5);
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if(!board.CheckPosition(activeBlock))//回転ではみ出ているか
            {
                activeBlock.RotateLeft();//はみ出ている場合逆の回転をさせる
            }
        }
        else if(Input.GetKey(KeyCode.DownArrow) && (Time.time > nextKeyDownTimer) 
            ||  (Time.time > nextdropTimer))
        //else if(Input.GetKeyDown(KeyCode.DownArrow))|| (Time.time - priviousTime >=falltime)
        {
            activeBlock.MoveDown();//下に動かす

            nextKeyDownTimer = Time.time +nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if(!board.CheckPosition(activeBlock))
            {
                if(board.OverLimit(activeBlock))
                {
                    GameOver();
                }
                else
                {
                    //底についた時の処理（ブロックが下についた時の関数）
                    BottomBoard();
                }
            }
        }
        

    }


    void BottomBoard()//ボードの底に着いた時に次のブロックを生成する関数
    {
        activeBlock.MoveUp();//枠からはみ出たものを一つ上に戻す
        board.SaveBlockInGrid(activeBlock);//二次元配列に今のブロックを認識させる（そのための関数を呼ぶ）

        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;//各タイマーの初期化
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board. ClearAllRows();//埋まっていれば削除する
    }

    //関数の作成//
    //ゲームオーバーになったらパネルを表示する
    void GameOver()
    {
        activeBlock.MoveUp();

        audioSource.PlayOneShot(sound4);

        //ゲームオーバーパネルの非表示設定
        if(!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true;
    }

    //ゲームクリアになったらパネルを表示する
    void GameClear()
    {
        activeBlock.MoveUp();

        audioSource.PlayOneShot(sound3);

        //ゲームオーバーパネルの非表示設定
        if(!gameClearPanel.activeInHierarchy)
        {
            gameClearPanel.SetActive(true);
        }

        gameClear = true;
    }

    //シーンを再読み込みする（ボタン押下で呼ぶ)
    public void Restart()
    {
        SceneManager.LoadScene(0);

        if (currentScore >= clearScore)
        {
             
           SceneManager.LoadScene("KaiwaScrean2");
        
        }

        if (gameOver)
        {
             
           SceneManager.LoadScene("KaiwaScrean2");
        
        }

    }

    // スコアの追加
    public void AddScore()
    {
        audioSource.PlayOneShot(sound1);

        // score += 100;
        // currentScore += score;
        currentScore += 300;
        scoreText.text = "Score: " + currentScore.ToString();

        Debug.Log(currentScore);

        if (currentScore >= clearScore)
        {
            GameClear();
            //Debug.Log(clearScore);
        }

    }

    public void  DownScore()
    {
        audioSource.PlayOneShot(sound2);

        // losspoint = 1;
        // image.fillAmount -= losspoint;

        currentScore -= 50;
        scoreText.text = "Score: " + currentScore.ToString();

        Debug.Log(currentScore);

        

        slider.value +=2;
        // slider.value = (float)currentScore / (float)maxHp; 
        Debug.Log("slider.value : " + slider.value);

        // if(slider.value = 1)
        // {
        //     GameOver();
        // }

        if (currentScore >= clearScore)
        {
            GameClear();
            //Debug.Log(clearScore);
        }

        if (slider.value >= 10)
        {
            GameOver();
            
        }
    }

}
