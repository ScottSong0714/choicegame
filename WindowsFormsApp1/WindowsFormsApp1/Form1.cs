using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    /**
     * @author: Chenglin Song
     * “找相同”小游戏（匹配小游戏）
     * 程序设计思路：
     * 利用website字体生成图片，创建窗体，分出16个单元格，对每个单元格都定义click事件，定义计时器，
     * 如果两图标不匹配，则令图标消失，否则图标不消失。当所有图标都存在时（匹配），计时器停止。
     * 判断机制是：事先定义图标的颜色，通过检查每个图标的文本颜色与背景颜色是否相同来检查每个图标看是否匹配。
     * 找不到匹配的图标了，则玩家获胜。
     */
    public partial class 匹配小游戏 : Form
    {
        //计时器
        int time = 0;

        //添加图标（利用website字体生成图标，每个字母对应一个图标）
        Random random = new Random();
        List<string> icons = new List<string>
        {
            "a","a","h","h","u","u","w","w",
            "n","n","j","j","g","g","x","x"
        };

        //两个引用变量，跟踪第一次和第二次分别单击了那个Label控件
        Label firstClicked = null;//指向第一个Label控件
        Label secondClicked = null;//指向第二个Label控件

        public 匹配小游戏()
        {
            InitializeComponent();
            giveIcon();//若显示图标，调用方法giveIcon()
            timer3.Start();//开启全局计时
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //分配随机图标给label控件
        private void giveIcon()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNum = random.Next(icons.Count);//随机数
                    iconLabel.Text = icons[randomNum];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNum);
                }
            }
        }

        private void 匹配小游戏_Load(object sender, EventArgs e)
        {
            this.Text = "找相同";
        }

        private void label_Click(object sender, EventArgs e)
        {

            //只有当两个不匹配的图标出现时才计时，所以在计时器工作时要忽视任何点击
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;//定义专门的label控件

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    timer2.Start();
                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;
                timer1.Start();
                timer2.Stop();

                //调用CheckForWinner()程序，检查玩家是否胜利
                CheckForWinner();

                //检查两图标是否匹配
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;

                    //关闭定时器1和定时器2
                    timer1.Stop();
                    timer2.Stop();

                    return;
                }
                
            }
        }
        
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            //停止计时
            timer1.Stop();

            //隐藏前一个图标
            firstClicked.ForeColor = firstClicked.BackColor;

            //重置firstClicked和secondClicked
            firstClicked = secondClicked;

            timer2.Start();

            secondClicked = null;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            // timer1.Stop();
            if (secondClicked == null)
            {
               
                //隐藏前一个图标
                firstClicked.ForeColor = firstClicked.BackColor;

                //重置firstClicked
                firstClicked = null;

                return;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            time++;
        }

        private void CheckForWinner()
        {
            int minus = 0;
            int seconds = 0;
            //foreach遍历每个图标,获取控件内的集合元素
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            //停止计时
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();


            minus = time / 600;
            seconds = (time % 600) / 10;
            //如果没有跳出循环，则说明没有找到不匹配的图标，说明玩家赢了
            MessageBox.Show("恭喜您，成功通过游戏。您所用时为：" + minus.ToString() + "分" + seconds.ToString() + "秒");
            Close();//结束游戏
        }



    }
}
