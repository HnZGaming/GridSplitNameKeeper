﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Gui;
using Torch.Managers;
using Torch.Managers.PatchManager;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;

namespace GridSplitNameKeeper
{
    [PatchShim]
    public static class GridPatch
    {
        private static Logger Log = GridSplitNameKeeperCore.Instance.Log;
        //private static readonly MethodInfo NewNameRequest = typeof(MyCubeGrid).GetMethod("OnChangeDisplayNameRequest", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Patch(PatchContext ctx)
        {
            ctx.GetPattern(typeof(MyCubeGrid).GetMethod("MoveBlocks",  BindingFlags.Static|BindingFlags.NonPublic)).Suffixes
                .Add(typeof(GridPatch).GetMethod(nameof(OnGridSplit), BindingFlags.Static| BindingFlags.NonPublic));
        }

        private static void OnGridSplit(ref MyCubeGrid from, ref MyCubeGrid to)
        {
            if (!GridSplitNameKeeperCore.Instance.Config.Enable)return;
            var newName = GetName(from.DisplayName);
            var newGrid = to;


            if (GridSplitNameKeeperCore.Instance.Config.CleanSplits &&
                newGrid.BlocksCount < GridSplitNameKeeperCore.Instance.Config.SplitThreshold)
            {
                newGrid.Close();
                Log.Info($"Closing grid {newGrid.DisplayName} after splitting from {from.DisplayName}");
                return;
            }

            if (!GridSplitNameKeeperCore.Instance.Config.KeepSplitName) return;


            Task.Run(() =>
            {
                Thread.Sleep(100);
                newGrid.ChangeDisplayNameRequest(newName);
                //NetworkManager.RaiseEvent(newGrid, NewNameRequest, newName);
            });

        }

        private static string GetName(string current)
        {
            double count = 0;
            var grids = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList();
            foreach (var grid in grids)
            {
                if (!grid.DisplayName.Contains(current)) continue;
                count++;
            }

            return $"{current} {count:00}";
        }

    }
}