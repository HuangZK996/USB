using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace USB
{
    class USBTool
    {
        #region MyRegion
        private FileStream DeviceIo = null;//异步IO流
        private bool is_open = false;
        private IntPtr device = new IntPtr(-1);

        private const int MAX_USB_DEVICES = 64;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        //常用设备接口类GUID
        private const string HidGuid = "{4D1E55B2-F16F-11CF-88CB-001111000030}";
        private const string UsbDevGuid = "{A5DCBF10-6530-11D2-901F-00C04FB951ED}";
        private const string UsbComPort = "{86E0D1E0-8089-11D0-9CE4-08003E301F73}";
        private const string Printer = "{4d36e979-e325-11ce-bfc1-08002be10318}";
        private const string Print = "{28d78fad-5a12-11d1-ae5b-0000f803a8c2}";

        public static void GetAllUsbDevice(ref List<string> UsbDeviceList)
        {
            UsbDeviceList.Clear();
            Guid guid = Guid.Parse(Print);
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref guid,0,IntPtr.Zero,DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);
            if(deviceInfoSet != IntPtr.Zero)
            {
                SP_DEVICE_INTERFACE_DATA interfaceInfo = new SP_DEVICE_INTERFACE_DATA();
                interfaceInfo.cbSize = Marshal.SizeOf(interfaceInfo);
                for(uint index = 0; index < 64;index++)
                {
                    if(SetupDiEnumDeviceInterfaces(deviceInfoSet,IntPtr.Zero,ref guid,index,ref interfaceInfo))
                    {
                        //取得接口详细信息:第一次读取错误,但可以取得信息缓冲区的大小
                        int buffsize = 0;
                        SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref interfaceInfo, IntPtr.Zero, buffsize, ref buffsize, null);
                        //构建接收缓冲
                        IntPtr pDetail = Marshal.AllocHGlobal(buffsize);
                        SP_DEVICE_INTERFACE_DETAIL_DATA detail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                        //detail.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
                        if (IntPtr.Size == 8)
                            detail.cbSize = 8;//for 64 bit operating systems
                        else
                            detail.cbSize = 4 + Marshal.SystemDefaultCharSize;//for 32 bit operation systems
                        Marshal.StructureToPtr(detail, pDetail, false);
                        if(SetupDiGetDeviceInterfaceDetail(deviceInfoSet,ref interfaceInfo,pDetail,buffsize,ref buffsize, null))
                        {
                            UsbDeviceList.Add(Marshal.PtrToStringAuto((IntPtr)((int)pDetail + 4)));

                        }
                            //
                        Marshal.FreeHGlobal(pDetail);
                    }
                }
            }
            SetupDiDestroyDeviceInfoList(deviceInfoSet);
        }

        public int OpenUsbDevice(UInt16 vID ,UInt16 pID)
        {
            List<string> deviceList = new List<string>();
            GetAllUsbDevice(ref deviceList);
            if (deviceList.Count == 0)
                return 0;

            string VID = string.Format("{0:X4}", vID);
            string PID = string.Format("{0:X4}", pID);
            foreach(string item in deviceList)
            {
                if(item.ToLower().Contains(VID.ToLower()) && item.ToLower().Contains(PID.ToLower())) 
                {
                    //指定设备
                    Debug.WriteLine(item);
                    if(is_open == false) {
                        device = CreateFile(item, DESIREDACCESS.GENERIC_READ | DESIREDACCESS.GENERIC_WRITE, 0, 0, CREATIONDISPOSITION.OPEN_EXISTING, 0x40000000, 0);
                        if(device!= INVALID_HANDLE_VALUE )
                        {
                            Debug.WriteLine("open");
                            DeviceIo = new FileStream(new SafeFileHandle(device, false), FileAccess.ReadWrite, 40, true);
                            //DeviceIo = new FileStream(new SafeFileHandle(device,false),FileAccess.ReadWrite);
                            this.is_open = true;
                            return 1;
                        }
                        CloseHandle(device);
                    }
                }

            }
            return 0; 
        }
        public void CloseDevice()
        {
            if(is_open == true)
            {
                is_open = false;
                DeviceIo.Close();
                CloseHandle(device);
            }
        }
        public void Send(string dataString)
        {
            if(DeviceIo == null)
            {
                Debug.WriteLine("USB Device not open");
                return;
            }
            byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(dataString);//打印机支持UTF-8编码
            // byte[] data = System.Text.Encoding.ASCII.GetBytes(dataString);
            DeviceIo.Write(data,0,data.Length);
        }
        public void Read()
        {
            //DeviceIo.Read();
        }

        public bool GetDeviceState()
        {
            return is_open;
        }
        #endregion

        #region Win32_api
        public enum DIGCF
        {
            DIGCF_DEFAULT = 0x00000001,//只返回与系统默认设备相关的设备
            DIGCF_PRESENT = 0x00000002,//只返回当前存在的设备
            DIGCF_ALLCLASSES = 0x00000004,//返回所有已安装的设备.如果这个标志设置了,ClassGuid参数将被忽略
            DIGCF_PROFILE = 0x00000008,//只返回当前硬件配置文件中的设备
            DIGCF_DEVICEINTERFACE = 0x00000010//返回所有支持的设备
        }
        /// <summary>
        /// 接口数据定义
        /// </summary>
        public struct SP_DEVICE_INTERFACE_DATA {
            public int cbSize;
            public Guid interfaceClassGuid;
            public int flags;
            public int reserved;
        }
        public class SP_DEVINFO_DATA
        {
            public int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
            public Guid classGuid = Guid.Empty;//temp
            public int devInst = 0;//dumy
            public int reserved = 0;

        }
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            internal int cbSize;
            internal short devicePath;
        }
        /// <summary>
        /// 获取USB-HID设备的设备接口类GUID,即{4D1E55B2-F16F-11CF-88CB-001111000030}
        /// </summary>
        /// <param name="HidGuid"></param>
        [DllImport("hid.dll")]
        private static extern void HidD_GetHidGuid(ref Guid HidGuid);
        /// <summary>
        /// 获取对应GUID的设备信息集(句柄)
        /// </summary>
        /// <param name="ClassGuid">设备设置类或设备接口类的guid</param>
        /// <param name="Enumerator">指向以空结尾的字符串的指针,改字符串提供PNP枚举器或PNP设备实例标识符的名称</param>
        /// <param name="HwndParent">用于用户界面的顶级窗口的句柄</param>
        /// <param name="Flags">一个变量,指定用于筛选添加到设备信息集中的设备信息元素的控制选项</param>
        /// <returns>设备信息集的句柄</returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr HwndParent, DIGCF Flags);
        /// <summary>
        /// 根据句柄,枚举设备信息集中包含的设备接口
        /// </summary>
        /// <param name="deviceInfoSet"></param>
        /// <param name="deviceInfoData"></param>
        /// <param name="interfaceClassGuid"></param>
        /// <param name="memberIndex"></param>
        /// <param name="deviceInterfaceData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, UInt32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        /// <summary>
        /// 获取接口详细信息,在第一次主要是读取缓存信息,第二次获取详细信息(必须调用两次)
        /// </summary>
        /// <param name="deviceInfoset">指向设备信息集的指针,他包含了所要接收信息的借口</param>
        /// <param name="deviceInterfaceData">返回数据</param>
        /// <param name="deviceInterfaceDetailData"></param>
        /// <param name="deviceInterfaceDetailDataSize"></param>
        /// <param name="requiredSize"></param>
        /// <param name="deviceInfoData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoset, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, SP_DEVINFO_DATA deviceInfoData);
        /// <summary>
        /// 删除设备信息并释放内存
        /// </summary>
        /// <param name="deviceInfoSet"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);
        #endregion

        #region Open_Device
        /// <summary>
        /// 访问权限
        /// </summary>
        static class DESIREDACCESS
        {
            public const uint GENERIC_READ = 0x80000000;
            public const uint GENERIC_WRITE = 0x40000000;
            public const uint GENERIC_EXECUTE = 0x20000000;
            public const uint GENERIC_ALL = 0x10000000;
        }
        /// <summary>
        /// 如何创建
        /// </summary>
        static class CREATIONDISPOSITION
        {
            public const uint CREATE_NEW = 1;
            public const uint CREATE_ALWAYS = 2;
            public const uint OPEN_EXISTING = 3;
            public const uint OPEN_ALWAYS = 4;
            public const uint TRUNCATE_EXISTING = 5;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFilename">普通文件名或设备文件名</param>
        /// <param name="desiredAccess">访问模式(写/读) GENERIC_READ、GENERIC_WRITE</param>
        /// <param name="shareMode">共享模式</param>
        /// <param name="securityAttributes">指向安全属性的指针</param>
        /// <param name="creationDisposition">如何创建</param>
        /// <param name="flagsAndAttributes">文件属性</param>
        /// <param name="templateFile">用于复制文件句柄</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFilename, uint desiredAccess, uint shareMode, uint securityAttributes, uint creationDisposition, uint flagsAndAttributes, uint templateFile);

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="hObject">Handle to an open object</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(IntPtr hObject);
        #endregion
    }
}
