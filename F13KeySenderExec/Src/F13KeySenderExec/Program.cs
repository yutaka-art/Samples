using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace F13KeySenderExec
{
    /// <summary>
    /// アプリケーションエントリポイント
    /// </summary>
    internal class Program
    {
        private static int SleepTime; // Will be initialized from settings
        private const string KeyToSend = "{F13}";

        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            SleepTime = Properties.Settings.Default.SleepTime; // Read from settings

            var currentScriptName = Process.GetCurrentProcess().MainModule.FileName;
            var runningProcesses = Process.GetProcessesByName("F13KeySender"); // Assuming your application name is F13KeySender.exe

            if (runningProcesses.Count(p => p.MainModule.FileName == currentScriptName) > 1)
            {
                var result = MessageBox.Show("送信処理を停止します。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (var process in runningProcesses)
                    {
                        if (process.Id != Process.GetCurrentProcess().Id)
                        {
                            process.Kill();
                        }
                    }
                }

                return;
            }

            var promptResult = MessageBox.Show($"{SleepTime / 1000}秒毎に{KeyToSend}を送信します。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (promptResult == DialogResult.Yes)
            {
                SendF13KeyPeriodically();
            }
        }

        private static void SendF13KeyPeriodically()
        {
            var simulator = new InputSimulator(); // Create an instance of InputSimulator

            while (true)
            {
                SendF13Key(simulator);
                Thread.Sleep(SleepTime);
            }
        }

        private static void SendF13Key(InputSimulator simulator)
        {
            // F13キーを送信
            simulator.Keyboard.KeyDown(VirtualKeyCode.F13);
            simulator.Keyboard.KeyUp(VirtualKeyCode.F13);
        }
    }
}
