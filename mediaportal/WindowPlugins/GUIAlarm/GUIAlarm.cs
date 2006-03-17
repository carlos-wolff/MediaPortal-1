/* 
 *	Copyright (C) 2005 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

using System;
using System.Collections;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using MediaPortal.Player;

namespace MediaPortal.GUI.Alarm 
{

	/// <summary>
	/// An Alarm Plugin for MediaPortal
	/// </summary>
	public class GUIAlarm : GUIWindow, IDisposable, IWakeable
	{
		public const int WindowAlarm = 5000;

		#region Private Variables		
			private static int _SelectedItemNumber;
			private System.Windows.Forms.Timer _SnoozeTimer = new System.Windows.Forms.Timer();
			private int	_SnoozeCount;
			private int _SnoozeTime;
		#endregion
		
		#region Private Enumerations
		enum Controls
		{
			NewButton = 3,
			SleepTimerButton = 4,
			AlarmList = 50	
		}
		#endregion

		#region Constructor
			public GUIAlarm()
			{
				GetID=(int)GUIAlarm.WindowAlarm;
			}
		#endregion
		
		#region Overrides
		
		public override bool Init()
		{
			LoadSettings();
			return Load (GUIGraphicsContext.Skin+@"\myalarm.xml");
		}

		public override void OnAction(Action action)
		{
			switch (action.wID)
			{
				case Action.ActionType.ACTION_PREVIOUS_MENU:
				{
					GUIWindowManager.ShowPreviousWindow();
					return;
				}
				case Action.ActionType.ACTION_KEY_PRESSED:
				{
					//space bar
					if(action.m_key.KeyChar == 32)
					{	
						//enable snooze timer
						_SnoozeCount = 0;
						_SnoozeTimer.Enabled = true;
					}
					break;
				}
			}
			
			base.OnAction(action);
		}

		public override bool OnMessage(GUIMessage message)
		{
			switch ( message.Message )
			{

				case GUIMessage.MessageType.GUI_MSG_WINDOW_INIT:
				{
					base.OnMessage(message);
					GUIPropertyManager.SetProperty("#currentmodule", GUILocalizeStrings.Get(850));
					AddAlarmsToList();
					return true;
				}
				case GUIMessage.MessageType.GUI_MSG_CLICKED:
				{
					//get sender control
					base.OnMessage(message);
					int iControl=message.SenderControlId;
							
					if (iControl==(int)Controls.AlarmList)
					{
						GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_ITEM_SELECTED,GetID,0,iControl,0,0,null);
						OnMessage(msg);
						int iItem=(int)msg.Param1;
						int iAction=(int)message.Param1;
						if (iAction == (int)Action.ActionType.ACTION_SELECT_ITEM)
						{
							OnClick(iItem);
						}
					}
					//new alarm
					if(iControl ==(int)Controls.NewButton)
					{
						//activate the new alarm window
						_SelectedItemNumber = -1;
						GUIWindowManager.ActivateWindow(GUIAlarmDetails.WindowAlarmDetails);
						return true;
					}
					if(iControl ==(int)Controls.SleepTimerButton)
					{
						GUIWindowManager.ActivateWindow(GUISleepTimer.WindowSleepTimer);
						return true;
					}
						
					
						
					return true;
				}
				case GUIMessage.MessageType.GUI_MSG_WINDOW_DEINIT:
				{
					
				}
					break;

			}
			return base.OnMessage(message);

		} 
		#endregion

		#region Private Methods
		/// <summary>
		/// Initializes the snooze timer object
		/// </summary>
		private void InitializeSnoozeTimer()
		{
			_SnoozeTimer.Tick += new EventHandler(OnTimer);
			_SnoozeTimer.Interval = 60000; //minute	
		}
		/// <summary>
		/// Executes on the interval of the timer object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTimer(Object sender, EventArgs e)
		{
			if(_SnoozeCount == _SnoozeTime)
			{
				_SnoozeTimer.Enabled= false;
				if (g_Player.Paused) g_Player.Pause();
			}
			else
			{
				_SnoozeCount++;
			}
		}
		/// <summary>
		/// Loads my alarm settings from the profile xml.
		/// </summary>
		private void LoadSettings()
		{ 
			InitializeSnoozeTimer();
			_SnoozeTime = Alarm.SnoozeTime;

			//Load all the alarms 
			Alarm.LoadAll();
		}

		/// <summary>
		/// Adds the alarms to the list control
		/// </summary>
		private void AddAlarmsToList()
		{
			foreach (Alarm objAlarm in Alarm.LoadedAlarms)
			{
					
				GUIListItem item = new GUIListItem();
				item.Label = objAlarm.Name;
				
				//set proper label
				if(objAlarm.AlarmOccurrenceType == Alarm.AlarmType.Recurring)
				{
					item.Label2 = objAlarm.DaysEnabled + "   " + objAlarm.Time.ToShortTimeString();
				}
				else
				{
					item.Label2 = objAlarm.Time.ToShortDateString() +  "   " + objAlarm.Time.ToShortTimeString();
				}
			
				item.IsFolder=false;
				item.MusicTag = true;

				//shade inactive alarms
				if (!objAlarm.Enabled)
					item.Shaded = true;

				item.IconImage = objAlarm.GetIcon;
				
				GUIControl.AddListItemControl(GetID,(int)Controls.AlarmList,item);
			}
			string strObjects =String.Format("{0} {1}",GUIControl.GetItemCount(GetID,(int)Controls.AlarmList).ToString(), GUILocalizeStrings.Get(632));
			GUIPropertyManager.SetProperty("#itemcount",strObjects);
		}

		void OnClick(int iItem)
		{
			_SelectedItemNumber = iItem;
			GUIWindowManager.ActivateWindow(GUIAlarmDetails.WindowAlarmDetails);				
		}

		#endregion
	
		#region Public Properties
			/// <summary>
			/// Gets the selected Item number from the alarm list
			/// </summary>
			public static int SelectedItemNo
			{
				get{return _SelectedItemNumber;}
			}
		#endregion

		#region IWakeable Members

		public bool DisallowShutdown()
		{
            return Alarm.DisallowShutdown;
		}

		public DateTime GetNextEvent(DateTime earliestWakeupTime)
		{
            return Alarm.GetNextAlarmDateTime(earliestWakeupTime);
		}
		public string PluginName()
		{
			return GUILocalizeStrings.Get(850);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_SnoozeTimer.Dispose();
		}

		#endregion
	}
}
