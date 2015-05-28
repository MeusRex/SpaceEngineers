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

        private bool m_connected = false;
        bool Connected
        {
            get
            {
                var currentTimestamp = Stopwatch.GetTimestamp();
                var elapsedTime = ((currentTimestamp - m_timeStamp) * Sync.RelativeSimulationRatio) * STOPWATCH_FREQUENCY;

                if (elapsedTime <= 0.75)
                    return m_connected;
                else
                {
                    m_connected = ConnectivityTest();
                    return m_connected;
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
        string CustomName                       { get { return m_block.CustomName; } }
        string CustomNameWithFaction            { get { return m_block.CustomNameWithFaction; } }
        string DetailedInfo                     { get { return m_block.DetailedInfo; } }
        bool HasLocalPlayerAccess()             { return m_block.HasLocalPlayerAccess(); }
        bool HasPlayerAccess(long playerId)     { return m_block.HasPlayerAccess(playerId); }
        void RequestShowOnHUD(bool enable)      { m_block.RequestShowOnHUD(enable); }
        void SetCustomName(string text)         { m_block.SetCustomName(text); }
        void SetCustomName(StringBuilder text)  { m_block.SetCustomName(text); }
        bool ShowOnHUD                          { get { return m_block.ShowOnHUD; } }

        void GetActions(List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
        {
            m_block.GetActions(resultList, collect);
        }

        void SearchActionsOfName(string name, List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null)
        {
            m_block.SearchActionsOfName(name, resultList, collect);
        }

        Sandbox.ModAPI.Interfaces.ITerminalAction GetActionWithName(string name)
        {
            return m_block.GetActionWithName(name);
        }

        Sandbox.ModAPI.Interfaces.ITerminalProperty GetProperty(string id)
        {
            return GetProperty(id);
        }

        void GetProperties(List<Sandbox.ModAPI.Interfaces.ITerminalProperty> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalProperty, bool> collect = null)
        {
            GetProperties(resultList, collect);
        }
    }
}
