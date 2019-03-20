using System;
using System.Windows;
using System.Windows.Forms;
namespace digitClock
{
    public partial class  MainWindow : Window
    {

        private ToolStripMenuItem 
            touchableItem,
            moveableItem;
     
        private void initializeNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Click +=(sender,e)=>
                {
                    if (!touchableItem.Checked)
                    {
                        var Event = e as MouseEventArgs;
                        touchableItem.Checked = (Event.Button == MouseButtons.Left);
                     }
                };
            notifyIcon.Text = "digitClock by NawaNawa";
            notifyIcon.Visible = true;
            notifyIcon.Icon = Properties.Resources.clockIcon;
            notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            ToolStripMenuItem closeItem = new ToolStripMenuItem("close");
            closeItem.Click += (sender, e) => { Close(); };
            notifyIcon.ContextMenuStrip.Items.Add(closeItem);
            touchableItem = new ToolStripMenuItem("touchable");
            touchableItem.Click += (snder, e) =>  {touchableItem.Checked = !touchableItem.Checked; };
            touchableItem.CheckedChanged += (sender, e) => 
            {
                touchableSet();
                if (touchableItem.Checked)
                {
                    if (mousePassThrough.Content.ToString() == "=")
                    {
                        mousePassThrough.Content = "≠";
                        controlPanel.Visibility = Visibility.Visible;
                    }
                }
                else
                     if (!Convert.ToBoolean(controlPanel.Visibility))
                    {
                        mousePassThrough.Content = "=";
                        controlPanel.Visibility = Visibility.Hidden;
                    }
            };
            
            notifyIcon.ContextMenuStrip.Items.Add(touchableItem);
            moveableItem = new ToolStripMenuItem("moveable");
            moveableItem.Click += (sender, e) => { moveableItem.Checked = !moveableItem.Checked; };
            moveableItem.CheckedChanged += (sender, e) =>{ moveableButtonContentChange(); };
            notifyIcon.ContextMenuStrip.Items.Add(moveableItem);


        }
        private void moveableButtonContentChange()
        {
            if (moveableItem.Checked)
            {
                moveableButton.Content = "🔒";
                moveableButton.ToolTip = "disable moveability";
            }
            else
            {
                moveableButton.Content = "🔓";
                moveableButton.ToolTip = "enable moveability";
            }
        }
        private void touchableSet()
        {
           
                Win32.SetWindowLong(hwnd.ToInt32(), -20, extendedStyle[Convert.ToInt32(touchableItem.Checked)]);
        }

    }
}
