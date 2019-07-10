using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System.Text.RegularExpressions;

public class ReadSerialPort : MonoBehaviour {
    
    public SerialPort sp = new SerialPort("COM5", 57600);
    string ReadFromSerialPort;
    Thread myThread;
    bool GetData = false;
    public MYSQL_connect mysql;
    void Start () {
        sp.Open();
        sp.ReadTimeout = 1;
       // myThread = new Thread(new ThreadStart(GetArduino));
       // myThread.Start();
        
        //sp.ReadTimeout = 1;
	}
    private void GetArduino()
    {
        while (myThread.IsAlive)
        {
            if (sp.IsOpen)
            {
                try //要是unity沒有跟上Arduino的Println 的話，ReadLine會LAG
                {
                    ReadFromSerialPort = sp.ReadLine();
                    if (ReadFromSerialPort != "")
                    {
                        if (ReadFromSerialPort.Contains("Reentry_"))
                        {
                            Debug.Log(ReadFromSerialPort);
                            GetData = true;
                            sp.Close(); //等待開啟
                        }
                    }
                }
                catch (System.Exception)
                {

                }

            }
           
        }
    }

    void LateUpdate () {
        
        if(GetData==true)
        {
            //   mysql.ParticipantID = Regex.Replace(ReadFromSerialPort, @"[\W_]+", "");
            mysql.ParticipantID = ReadFromSerialPort;
            mysql.OpenConnect = true;
            GetData = false;
           
        }
       
    }
    private void FixedUpdate()
    {
        if (sp.IsOpen)
        {
            try //要是unity沒有跟上Arduino的Println 的話，ReadLine會LAG
            {
                ReadFromSerialPort = sp.ReadLine();
                if (ReadFromSerialPort != "")
                {
                    if (ReadFromSerialPort.Contains("Reentry_"))
                    {
                        

                        GetData = true;
                        //sp.Close(); //等待開啟
                    }
                }
            }
            catch (System.Exception)
            {

            }

        }
    }
}
