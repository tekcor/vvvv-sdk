#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "PID", 
				Category = "Windows", 
				Help = "Returns the process ID of a given process name.", 
				Tags = "process")]
	#endregion PluginInfo
	public class VVVVPIDNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Process Name", DefaultString="vvvv")]
		public ISpread<string> FProcessName;
		
		[Input("Update", IsBang = true, IsSingle = true)]
		public ISpread<bool> FUpdate;
		
		[Output("ID")]
        public ISpread<ISpread<int>> FOutput;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (FUpdate[0])
			{
				var processes = Process.GetProcesses();
				FOutput.SliceCount = SpreadMax;
				
				int i = 0;
				foreach (var name in FProcessName)
				{
					var requestedIDs = from p in processes
									   where p.ProcessName == name
	                       			   select p.Id;
					
					FOutput[i].SliceCount = requestedIDs.Count();
					FOutput[i].AssignFrom(requestedIDs);				
					i++;
				}
			}
		}
	}
}
