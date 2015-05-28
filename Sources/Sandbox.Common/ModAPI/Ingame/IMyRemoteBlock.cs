using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandbox.Common.ModAPI.Ingame
{
    public interface IMyRemoteBlock
    {
        string CustomName { get; }
        string CustomNameWithFaction { get; }
        string DetailedInfo { get; }
        bool HasLocalPlayerAccess();
        bool HasPlayerAccess(long playerId);
        void RequestShowOnHUD(bool enable);
        void SetCustomName(string text);
        void SetCustomName(StringBuilder text);
        bool ShowOnHUD { get; }
        void GetActions(List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null);
        void SearchActionsOfName(string name, List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect = null);
        Sandbox.ModAPI.Interfaces.ITerminalAction GetActionWithName(string name);
        Sandbox.ModAPI.Interfaces.ITerminalProperty GetProperty(string id);
        void GetProperties(List<Sandbox.ModAPI.Interfaces.ITerminalProperty> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalProperty, bool> collect = null);
    }
}
