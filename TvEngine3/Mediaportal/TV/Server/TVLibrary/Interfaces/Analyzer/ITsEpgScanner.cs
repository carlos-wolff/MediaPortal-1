#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Runtime.InteropServices;

namespace Mediaportal.TV.Server.TVLibrary.Interfaces.Analyzer
{
  /// <summary>
  /// call back interface for the EPG
  /// </summary>
  [Guid("FFAB5D98-2309-4d90-9C71-E4B2F490CF5A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IEpgCallBack
  {
    /// <summary>
    /// Called when epg is received.
    /// </summary>
    /// <returns></returns>
    [PreserveSig]
    int OnEpgReceived();
  }

  /// <summary>
  /// Interface to the epg grabber com object
  /// </summary>
  [Guid("5CDAC655-D9FB-4c71-8119-DD07FE86A9CE"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  internal interface ITsEpgScanner
  {
    /// <summary>
    /// Start grabbing the EPG.
    /// </summary>
    /// <returns></returns>
    [PreserveSig]
    int GrabEPG();

    /// <summary>
    /// Determines whether EPG has been received
    /// </summary>
    /// <param name="yesNo">if set to <c>true</c> then epg is ready.</param>
    /// <returns></returns>
    [PreserveSig]
    int IsEPGReady([MarshalAs(UnmanagedType.Bool)] out bool yesNo);

    /// <summary>
    /// Gets the nummer of channels for which epg has been received
    /// </summary>
    /// <param name="channelCount">The channel count.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetEPGChannelCount(out uint channelCount);

    /// <summary>
    /// Gets the number of epg events for a channel
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="eventCount">The event count.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetEPGEventCount(uint channel, out uint eventCount);

    /// <summary>
    /// Gets the EPG channel details.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="networkId">The network id.</param>
    /// <param name="transportid">The transportid.</param>
    /// <param name="service_id">The service_id.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetEPGChannel(uint channel, out ushort networkId, out ushort transportid, out ushort service_id);

    /// <summary>
    /// Gets the EPG event details.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="eventid">The eventid.</param>
    /// <param name="languageCount">The language count.</param>
    /// <param name="date">The date.</param>
    /// <param name="time">The time.</param>
    /// <param name="duration">The duration.</param>
    /// <param name="genre">The genre.</param>
    /// <param name="starRating">The star rating</param>
    /// <param name="classification">The classification</param>
    /// <returns></returns>
    [PreserveSig]
    int GetEPGEvent(uint channel, uint eventid, out uint languageCount, out uint date,
                    out uint time, out uint duration, out IntPtr genre, out int starRating,
                    out IntPtr classification);

    /// <summary>
    /// Gets the EPG language.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="eventid">The eventid.</param>
    /// <param name="languageIndex">Index of the language.</param>
    /// <param name="language">The language.</param>
    /// <param name="eventText">The event text.</param>
    /// <param name="eventDescription">The event description.</param>
    /// <param name="parentalRating">The parental rating</param>
    /// <returns></returns>
    [PreserveSig]
    int GetEPGLanguage(uint channel, uint eventid, uint languageIndex, out uint language,
                       out IntPtr eventText, out IntPtr eventDescription, out int parentalRating);

    /// <summary>
    /// Start grabbing MGW
    /// </summary>
    /// <returns></returns>
    [PreserveSig]
    int GrabMHW();

    /// <summary>
    /// Determines whether MHW has been received or not
    /// </summary>
    /// <param name="yesNo">if MHW has been received then <c>true</c> .</param>
    /// <returns></returns>
    [PreserveSig]
    int IsMHWReady([MarshalAs(UnmanagedType.Bool)] out bool yesNo);

    /// <summary>
    /// Gets the number of MHW titles received.
    /// </summary>
    /// <param name="count">The count.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetMHWTitleCount(out uint count);

    /// <summary>
    /// Gets the details for a MHW title.
    /// </summary>
    /// <param name="program">The program.</param>
    /// <param name="id">The id.</param>
    /// <param name="transportId">The transport id.</param>
    /// <param name="networkId">The network id.</param>
    /// <param name="channelId">The channel id.</param>
    /// <param name="programId">The program id.</param>
    /// <param name="themeId">The theme id.</param>
    /// <param name="PPV">The PPV.</param>
    /// <param name="Summaries">The summaries.</param>
    /// <param name="duration">The duration.</param>
    /// <param name="dateStart">The date start.</param>
    /// <param name="timeStart">The time start.</param>
    /// <param name="title">The title.</param>
    /// <param name="programName">Name of the program.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetMHWTitle(uint program, out uint id, out uint transportId, out uint networkId, out uint channelId,
                    out uint programId, out uint themeId, out uint PPV, out byte Summaries, out uint duration,
                    out uint dateStart, out uint timeStart, out IntPtr title, out IntPtr programName);

    /// <summary>
    /// Gets the details for a MHW channel.
    /// </summary>
    /// <param name="channelNr">The channel nr.</param>
    /// <param name="channelId">The channel id.</param>
    /// <param name="networkId">The network id.</param>
    /// <param name="transportId">The transport id.</param>
    /// <param name="channelName">Name of the channel.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetMHWChannel(uint channelNr, out uint channelId, out uint networkId, out uint transportId,
                      out IntPtr channelName);

    /// <summary>
    /// Gets the MHW summary.
    /// </summary>
    /// <param name="programId">The program id.</param>
    /// <param name="summary">The summary.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetMHWSummary(uint programId, out IntPtr summary);

    /// <summary>
    /// Gets the MHW theme.
    /// </summary>
    /// <param name="themeId">The theme id.</param>
    /// <param name="theme">The theme.</param>
    /// <returns></returns>
    [PreserveSig]
    int GetMHWTheme(uint themeId, out IntPtr theme);

    /// <summary>
    /// Resets this MHW grabber.
    /// </summary>
    /// <returns></returns>
    [PreserveSig]
    int Reset();

    /// <summary>
    /// Aborts grabbing and calls the call back function
    /// </summary>
    /// <returns></returns>
    [PreserveSig]
    int AbortGrabbing();

    /// <summary>
    /// Sets the call back which will be called when MHW has been received
    /// </summary>
    /// <param name="callBack">The call back.</param>
    /// <returns></returns>
    [PreserveSig]
    int SetCallBack(IEpgCallBack callBack);
  }
}