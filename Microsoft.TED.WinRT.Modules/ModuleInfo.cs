//	Copyright (c) Max Knor, Microsoft
//	All rights reserved. 
//	http://blog.knor.net/
//
//	Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 
//	THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR NON-INFRINGEMENT. 
//
//	See the Apache Version 2.0 License for specific language governing permissions and limitations under the License.
namespace Microsoft.TED.WinRT.Modules
{
    public class ModuleInfo
    {
        public string Name { get; set; }
        public string EntryPoint { get; set; }
        public string[] DependsOn { get; set; }

        public ModuleInfo(string name, string entryPoint, string[] dependsOn)
        {
            Name = name;
            EntryPoint = entryPoint;
            DependsOn = dependsOn;
        }
    }
}