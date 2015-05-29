using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI.Ingame;
using Sandbox.Common.ModAPI.Ingame;
using Sandbox.Game.Multiplayer;

namespace Sandbox.Game.Entities
{
    class MyRemoteControlWrapper : IMyRemoteBlock
    {
        private static readonly double STOPWATCH_FREQUENCY = 1.0 / Stopwatch.Frequency;

        private IMyTerminalBlock m_block;
        private MyCubeGrid m_origin;
        private MyCubeGrid m_destination; //The grid containing the wrapped cube
        private long m_timeStamp;
        private long m_playerId;

        private bool m_connectedAndOwned = false;
        bool ConnectedAndOwned
        {
            get
            {
                var currentTimestamp = Stopwatch.GetTimestamp();
                var elapsedTime = ((currentTimestamp - m_timeStamp) * Sync.RelativeSimulationRatio) * STOPWATCH_FREQUENCY;

                if (elapsedTime <= 0.75) //To little time has passed since the last check. Return the same value as last time.
                    return m_connectedAndOwned;
                else
                {
                    m_connectedAndOwned = m_block.HasPlayerAccess(m_playerId);
                    if (m_connectedAndOwned) //If the player owns the block, check for the connection. Otherwise just go ahead with the false.
                        m_connectedAndOwned = ConnectivityTest();

                    return m_connectedAndOwned;
                }
            }
        }

        private bool ConnectivityTest()
        {
            return true;
        }

        MyRemoteControlWrapper(IMyTerminalBlock towrap, MyCubeGrid origin, MyCubeGrid destination, long playerid)
        {
            m_block = towrap;
            m_origin = origin;
            m_destination = destination;
            m_playerId = playerid;
            m_timeStamp = 0;
        }

        //Implementation of IMyRemoteBlock
        string CustomName                       { get { if (ConnectedAndOwned) return m_block.CustomName;                   return String.Empty; } }
        string CustomNameWithFaction            { get { if (ConnectedAndOwned) return m_block.CustomNameWithFaction;        return String.Empty; } }
        string DetailedInfo                     { get { if (ConnectedAndOwned) return m_block.DetailedInfo;                 return String.Empty; } }
        bool HasLocalPlayerAccess()             {       if (ConnectedAndOwned) return m_block.HasLocalPlayerAccess();       return false; }
        bool HasPlayerAccess(long playerId)     {       if (ConnectedAndOwned) return m_block.HasPlayerAccess(playerId);    return false; }
        void RequestShowOnHUD(bool enable)      {       if (ConnectedAndOwned) m_block.RequestShowOnHUD(enable); }
        void SetCustomName(string text)         {       if (ConnectedAndOwned) m_block.SetCustomName(text); }
        void SetCustomName(StringBuilder text)  {       if (ConnectedAndOwned) m_block.SetCustomName(text); }
        bool ShowOnHUD                          { get { if (ConnectedAndOwned) return m_block.ShowOnHUD;                    return false; } }

        void GetActions(List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
        {
            if (ConnectedAndOwned)
                m_block.GetActions(resultList, collect);
        }

        void SearchActionsOfName(string name, List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
        {
            if (ConnectedAndOwned)
                m_block.SearchActionsOfName(name, resultList, collect);
        }

        Sandbox.ModAPI.Interfaces.ITerminalAction GetActionWithName(string name)
        {
            if (ConnectedAndOwned)
                return m_block.GetActionWithName(name);
            else
                return null;
        }

        Sandbox.ModAPI.Interfaces.ITerminalProperty GetProperty(string id)
        {
            if (ConnectedAndOwned)
                return GetProperty(id);
            else
                return null;
        }

        void GetProperties(List<Sandbox.ModAPI.Interfaces.ITerminalProperty> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalProperty, bool> collect = null)
        {
            if (ConnectedAndOwned)
                GetProperties(resultList, collect);
        }
    }
}
