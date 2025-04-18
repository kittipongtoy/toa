﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NAudio.CoreAudioApi;

namespace TOAMediaPlayer.NAudioOutput
{
    public partial class WasapiOutSettingsPanel : UserControl
    {
        public WasapiOutSettingsPanel()
        {
            InitializeComponent();
            InitialiseWasapiControls();
        }

        class WasapiDeviceComboItem
        {
            public string Description { get; set; }
            public MMDevice Device { get; set; }
        }

        private void InitialiseWasapiControls()
        {
            var enumerator = new MMDeviceEnumerator();
            var endPoints = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            var comboItems = new List<WasapiDeviceComboItem>();
            var defaultEndPoint = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            
            foreach (var endPoint in endPoints)
            {
                var comboItem = new WasapiDeviceComboItem();
                comboItem.Description = string.Format("{0} ({1})", endPoint.FriendlyName, endPoint.DeviceFriendlyName);
                comboItem.Device = endPoint;
                comboItems.Add(comboItem);

            }
            comboBoxWaspai.DisplayMember = "Description";
            comboBoxWaspai.ValueMember = "Device";
            comboBoxWaspai.DataSource = comboItems;
            var defaultItemIndex = comboItems.FindIndex(ci => ci.Device.ID == defaultEndPoint.ID);
            if (defaultItemIndex != -1)
            comboBoxWaspai.SelectedIndex = defaultItemIndex;


        }

        public MMDevice SelectedDevice { get { return (MMDevice)comboBoxWaspai.SelectedValue; } }

        public AudioClientShareMode ShareMode
        {
            get
            {
                return checkBoxWasapiExclusiveMode.Checked ?
                    AudioClientShareMode.Exclusive :
                    AudioClientShareMode.Shared;
            }
        }

        public bool UseEventCallback { get { return checkBoxWasapiEventCallback.Checked; } }
    }
}
