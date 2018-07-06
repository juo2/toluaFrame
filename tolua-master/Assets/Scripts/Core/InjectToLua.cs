using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;
using System.IO;
using System.Text;

public class InjectToLua : MonoBehaviour {
	string url = "http://{0}/res/Inject/LuaMain.lua";

	[SerializeField]
	InputField input;
	// Use this for initialization
	void Start () {
		Button btn = GetComponent<Button> ();
		btn.onClick.AddListener (() => {
			string ip = input.text;
			if (string.IsNullOrEmpty(ip) )
			{
				url = string.Format("http://{0}/res/Inject/LuaJuo2.lua","localhost");
			}
			else
			{
				url = string.Format("http://{0}/res/Inject/LuaJuo2.lua",ip);
			}

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?v=" + DateTime.Now.ToString("yyyymmddhhmmss"));
			request.Method = "GET";
			request.ContentType = "text/html;charset=UTF-8";

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream myResponseStream = response.GetResponseStream();
			StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
			string retString = myStreamReader.ReadToEnd();
			myStreamReader.Close();
			myResponseStream.Close();
			LuaManager.Instance.DoString(retString);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
