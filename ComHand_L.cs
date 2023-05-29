using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System;
using System.Threading;
using System.Collections;
using ONNXModel_RT;
using System.IO;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

[RequireComponent(typeof(FingerBone_L))]

public class ComHand_L : MonoBehaviour
{
    private string portName = "COM8";
    private int baudRate = 115200;
    private Parity parity = Parity.None;
    private int dataBits = 8;
    private StopBits stopBits = StopBits.One;

    SerialPort sp = null;
    private FingerBone_L fingerbone_L;
    private CancellationTokenSource cts;
    private Thread readThread;
    private string line;

    int num = 16;
    private string state_calibration="未标定";
    private string state_connection="未连接";
    private float[] maxValues = new float[12];  // 存储最大值
    private float[] minValues = new float[12];  // 存储最小值
    private float[] data_double = new float[12];
    private float[] angle = new float[14];
    private bool isDone = false;
    private bool isDone1 = false;
    private bool model_onnx = true;
    private string absolutePath = Path.Combine(Application.streamingAssetsPath, "model.onnx");
    private ONNXModel_Deployment ONNXModel_Deployment;
    void Start()
    {
        fingerbone_L = GetComponent<FingerBone_L>();
        print("开启");
        OpenPort();
        Find_MaxMin();
        ONNXModel_Deployment = new ONNXModel_Deployment(absolutePath);

    }
    void Awake()
    {
        //Application.targetFrameRate = 100; 
    }

    //打开串口
    public void OpenPort()
    {
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
        {
            ReadTimeout = 400000
        };
        sp.Open();
        cts?.Cancel();
        cts = new CancellationTokenSource();
        readThread = new Thread(ReadThread);
        readThread.Start();

    }

    //关闭串口
    public void ClosePort()
    {
        cts?.Cancel();
        sp?.Close();
    }
    // !cts.IsCancellationRequested && sp != null && sp.IsOpen
    private void ReadThread()
    {
        while (sp != null)
        {
            line = sp.ReadLine();
            state_connection = "已连接！";
        }
    }

    public void Find_MaxMin()
    {
        Debug.Log("开始标定");
        for (int i = 0; i < (num - 4); i++)
        {
            maxValues[i] = 1;
            minValues[i] = 100000;
        }
        isDone = false;
    }
    public void Find_MaxMin_Stop()
    {
        Debug.Log("完成标定");
        isDone = true;
    }
    public void Model_Select()
    {
        Debug.Log("完成标定");
        model_onnx = !model_onnx;
        state_calibration = "请重新标定";
    }
    private void Update()
    {
        float deltaTimeScaled = Time.deltaTime * Time.timeScale;
        GameObject.Find("Canvas/Events/连接状态显示").GetComponent<Text>().text = state_connection;
        if (state_connection == "已连接！")
        {
            GameObject.Find("Canvas/Events/连接状态显示").GetComponent<Text>().color = Color.green;
        }
        else
        {
            GameObject.Find("Canvas/Events/连接状态显示").GetComponent<Text>().color = Color.white;           
        }
        
        if (line == null)
        {
            GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().text = "null";
            GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().color = Color.red;
            return;
        }
        if (line[line.Length - 1] != ';')
        {
            GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().text = "!=;";
            GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().color = Color.red;
            return;
        }
        string[] data = line.Substring(0, line.Length - 1).Split(',');
        if (data.Length != num)
        {
            GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().text = Convert.ToString(data.Length);
            GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().color = Color.red;
            return;
        }
        GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().text = "无";
        GameObject.Find("Canvas/Events/数据错误类型显示").GetComponent<Text>().color = Color.white;
        if (isDone)
        {
            state_calibration = "标定完成！";
            isDone1 = false;
            string floatString1 = string.Join(",", maxValues);
            string floatString2 = string.Join(",", minValues);

            for (int i = 0; i < 8; i++)
            {

                if (maxValues[i] <= minValues[i])
                {
                    state_calibration = "请重新标定";
                    isDone1 = true;
                }

            }
            
            if (model_onnx == false)
            {
                GameObject.Find("Canvas/Events/模型显示").GetComponent<Text>().text = "线性模型";
                GameObject.Find("Canvas/Events/模型显示").GetComponent<Text>().color = Color.green;
            }
            else
            {
                GameObject.Find("Canvas/Events/模型显示").GetComponent<Text>().text = "Onnx";
                GameObject.Find("Canvas/Events/模型显示").GetComponent<Text>().color = Color.green;
            }

            GameObject.Find("Canvas/Events/标定状态显示").GetComponent<Text>().text = state_calibration;
            if (state_calibration == "标定完成！")
            {
                GameObject.Find("Canvas/Events/标定状态显示").GetComponent<Text>().color = Color.green;
            }
            else
            {
                GameObject.Find("Canvas/Events/标定状态显示").GetComponent<Text>().color = Color.red;
            }
            if (isDone1)
            {
                return;
            }            

            if (model_onnx==true)
            {
                for (int i = 0; i < (num - 4); i++)
                {
                    if (!float.TryParse(data[i], out data_double[i]))
                    {
                        Debug.Log("Invalid value: " + data[i]);
                        break;
                    }
                    data_double[i] = (data_double[i] - minValues[i]) / (maxValues[i] - minValues[i]);
                    if (data_double[i] < 0)
                    {
                        data_double[i] = 0;
                    }
                    if (data_double[i] > 1)
                    {
                        data_double[i] = 1;
                    }
                    if (data_double[i] > 0.5)
                    {
                        data_double[i] = (float)(((Math.Sin(Math.PI * ((data_double[i]) - 0.5))) + 1) * 0.5);
                    }
                    
                }
                string floatString3 = string.Join(",", data_double);

                var input = data_double;
                string floatString4 = string.Join(",", input);

                int[] inputshape = { 1, 1, 1, 12 };

                var output = ONNXModel_Deployment.RunInference(input, inputshape);

                for (int i = 0; i < 14; i++)
                {
                    angle[i] = output[i] * 100f;
                }
                string floatString = string.Join(",", angle);

                fingerbone_L.ConThumb1(angle[2]);
                fingerbone_L.ConThumb11(0);
                fingerbone_L.ConThumb2(angle[1]);
                fingerbone_L.ConThumb22(0);
                fingerbone_L.ConThumb3(angle[0]);
                fingerbone_L.ConIndex1(angle[5]);
                fingerbone_L.ConIndex11(0);
                fingerbone_L.ConIndex2(angle[4]);
                fingerbone_L.ConIndex3(angle[4]);
                fingerbone_L.ConMiddle1(angle[8]);
                fingerbone_L.ConMiddle11(0);
                fingerbone_L.ConMiddle2(angle[7]);
                fingerbone_L.ConMiddle3(angle[7]);
                fingerbone_L.ConRing1(angle[11]);
                fingerbone_L.ConRing11(0);
                fingerbone_L.ConRing2(angle[10]);
                fingerbone_L.ConRing3(angle[10]);
                fingerbone_L.ConLittle1(angle[13]);
                fingerbone_L.ConLittle11(0);
                fingerbone_L.ConLittle2(angle[12]);
                fingerbone_L.ConLittle3(angle[12]);
            }
            if (model_onnx == false)
            {
                for (int i = 0; i < (num - 4); i++)
                {
                    if (!float.TryParse(data[i], out data_double[i]))
                    {
                        Debug.Log("Invalid value: " + data[i]);
                        break;
                    }
                    data_double[i] = (data_double[i] - minValues[i]) / (maxValues[i] - minValues[i]);
                    if (data_double[i] < 0)
                    {
                        data_double[i] = 0;
                    }
                    if (data_double[i] > 1)
                    {
                        data_double[i] = 1;
                    }
                    if (data_double[i] > 0.8)
                    {
                        data_double[i] = (float)(((Math.Sin(Math.PI * ((data_double[i]) - 0.5))) + 1) * 0.5);
                    }

                }
   
                fingerbone_L.ConThumb1(data_double[1]*20);
                fingerbone_L.ConThumb11(data_double[2] * 40);
                fingerbone_L.ConThumb2(data_double[0]*70);
                fingerbone_L.ConThumb22(data_double[2] * 70);
                fingerbone_L.ConThumb3(data_double[0] * 80);
                fingerbone_L.ConIndex1(data_double[3]*80);
                fingerbone_L.ConIndex11(data_double[5] * 10);
                fingerbone_L.ConIndex2(data_double[3] * 80);
                fingerbone_L.ConIndex3(data_double[3] * 60);
                fingerbone_L.ConMiddle1(data_double[6]*80);
                fingerbone_L.ConMiddle11((data_double[8]- data_double[5]) * 10);
                fingerbone_L.ConMiddle2(data_double[6] * 80);
                fingerbone_L.ConMiddle3(data_double[6] * 60);
                fingerbone_L.ConRing1(data_double[9] * 80);
                fingerbone_L.ConRing11(data_double[8] * 10);
                fingerbone_L.ConRing2(data_double[9] * 80);
                fingerbone_L.ConRing3(data_double[9] * 60);
                fingerbone_L.ConLittle1(data_double[11] * 80);
                fingerbone_L.ConLittle11(data_double[10] * 20);
                fingerbone_L.ConLittle2(data_double[11] * 80);
                fingerbone_L.ConLittle3(data_double[11] * 60);
            }

        }
        if (isDone != true)
        {

            for (int i = 0; i < (num-4); i++)
            {
                float value;

                if (!float.TryParse(data[i], out value))
                {
                    //Console.WriteLine("Invalid value: " + values[i]);
                    break;  // 数据无效
                }

                if (value > 1000 && value < 10000)
                {
                    //Debug.Log(string.Join(",", value));
                    // 找到最大值和最小值
                    maxValues[i] = Math.Max(maxValues[i], value);
                    minValues[i] = Math.Min(minValues[i], value);
                }

            }
            state_calibration = "正在进行标定";
            GameObject.Find("Canvas/Events/标定状态显示").GetComponent<Text>().text = state_calibration;
            GameObject.Find("Canvas/Events/标定状态显示").GetComponent<Text>().color = Color.yellow;

        }
    }
    void OnApplicationQuit()
    {
        ClosePort();
    }
}