﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Warden.Core.Exceptions;
using Warden.Core.Utils;
using Warden.Windows;
using Warden.Properties;

namespace Warden.Core.Launchers
{
    internal class UwpLauncher : ILauncher
    {
        public async Task<WardenProcess> Launch(string path, string arguments)
        {
            var pId = await Api.LaunchUwpApp(path, arguments);
            if (pId <= 0)
            {
                throw new WardenLaunchException(string.Format(Resources.Exception_Could_Not_Find_Process_Id, path));
            }
            var pName = string.Empty;
            ProcessState state;
            try
            {
                var process = Process.GetProcessById(pId);
                pName = process.ProcessName;
                state = process.HasExited ? ProcessState.Dead : ProcessState.Alive;
            }
            catch (Exception)
            {
                state = ProcessState.Dead;
            }
            return new WardenProcess(pName, pId, path, state, arguments?.SplitSpace(), ProcessTypes.Uwp, null);
        }

        public async Task<WardenProcess> Launch(string path, string token, string arguments)
        {
            return await Launch($"{path}{token}", arguments);
        }

        public Task<WardenProcess> LaunchUri(string uri, string path, string arguments)
        {
            throw new NotImplementedException();
        }
    }
}
