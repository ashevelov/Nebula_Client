﻿using Common;
using Game.Space;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula
{
    public class Settings
    {

        static Settings()
        {
            ChannelCount = 3;
            DiagnosticsChannel = 0;
            OperationChannel = 0;
            ItemChannel = 0;
        }

        public static Settings GetDefaultSettings()
        {
            const int IntervalSend = 30;
            const bool SendReliable = false;
            const bool UseTcp = false;
#if LOCAL_SERVER
        const string ServerAddress = "192.168.1.28:5055";
       //const string ServerAddress = "192.168.1.30:5055";
#else
            const string ServerAddress = "52.10.78.38:5055";
#endif
        const string ApplicationName = "SpaceServer";
            //const string WorldName = "Map";

            Vector3 cornerMin = new Vector3(-50000, -50000, -50000);
            Vector3 cornerMax = new Vector3(50000, 50000, 50000);
            Vector3 tileDimensions = new Vector3(10000, 10000, 10000);

            Settings result = new Settings();
            result.ServerAddress = ServerAddress;
            result.UseTcp = UseTcp;
            result.ApplicationName = ApplicationName;

            result.TileDimensions = tileDimensions;
            result.WorldCornerMin = cornerMin;
            result.WorldCornerMax = cornerMax;
            result.SendInterval = IntervalSend;
            result.SendReliable = SendReliable;

            string settingsText = Resources.Load<TextAsset>("settings").text;
            result.ParseSettings(settingsText);

            return result;
        }

        private int sendInterval;
        private bool useTcp;
        private string serverAddress;
        private string applicationName;
        private string worldName;
        private bool sendReliable;
        private Vector3 _tileDimensions;
        private Vector3 _worldCornerMin;
        private Vector3 _worldCornerMax;
        public static byte ChannelCount { get; set; }
        public static byte DiagnosticsChannel { get; set; }
        public static byte ItemChannel { get; set; }
        public static byte OperationChannel { get; set; }

        public bool UseSpectatorCamera { get; private set; }
        public string[] LogFilters { get; private set; }

        public Hashtable Inputs { get; private set; }

        public Dictionary<Race, string> DefaultZones { get; private set; }

        public void ParseSettings(string text)
        {
            this.Inputs = new Hashtable();

            sXDocument document = new sXDocument();
            document.ParseXml(text);
            sXElement settingsElement = document.GetElement("settings");
            this.UseSpectatorCamera = settingsElement.GetElement("spectator_camera").GetBool("value");
            this.LogFilters = settingsElement.GetElement("log_filters").GetString("value").Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var e in settingsElement.GetElement("inputs").GetElements("value")) {
                string key = e.GetString("key");
                string value = e.GetString("value");
                string type = e.GetString("type");
                this.Inputs.Add(key, CommonUtils.ParseValue(value, type));
            }

            this.DefaultZones = new Dictionary<Race, string>();
            foreach (var e in settingsElement.GetElement("default_zones").GetElements("zone")) {
                Race race = (Race)System.Enum.Parse(typeof(Race), e.GetString("race"));
                string id = e.GetString("id");
                this.DefaultZones.Add(race, id);
            }
        }



        public float[] ViewDistanceEnter
        {
            get
            {
                return new float[] { 1e6f, 1e6f, 1e6f };
            }
        }
        public float[] ViewDistanceExit
        {
            get
            {
                return new float[] { 2e6f, 2e6f, 2e6f };
            }
        }

        public Vector3 TileDimensions
        {
            get { return _tileDimensions; }
            set { _tileDimensions = value; }
        }

        public Vector3 WorldCornerMin
        {
            get { return _worldCornerMin; }
            set { _worldCornerMin = value; }
        }

        public Vector3 WorldCornerMax
        {
            get { return _worldCornerMax; }
            set { _worldCornerMax = value; }
        }

        public string ServerAddress
        {
            get
            {
                return serverAddress;
            }
            set
            {
                serverAddress = value;
            }
        }

        public string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public string WorldName
        {
            get
            {
                return worldName;
            }
            set
            {
                worldName = value;
            }
        }

        public bool SendReliable
        {
            get
            {
                return sendReliable;
            }
            set
            {
                sendReliable = value;
            }
        }

        public int SendInterval
        {
            get
            {
                return this.sendInterval;
            }

            set
            {
                this.sendInterval = value;
            }
        }

        public bool UseTcp
        {
            get
            {
                return this.useTcp;
            }

            set
            {
                this.useTcp = value;
            }
        }

        public float[] DefaultViewDistance
        {
            get
            {
                return new float[] { 
                TileDimensions[0] * .5f + 1,
                TileDimensions[1] * .5f + 1,
                TileDimensions[2] * .5f + 1
            };
            }
        }


    }
}