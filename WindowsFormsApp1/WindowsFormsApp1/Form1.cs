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
     * 利用Webdings字体生成图片，创建窗体，分出16个单元格，对每个单元格都定义click事件，定义计时器，
     * 如果两图标不匹配，则令图标消失，否则图标不消失。当所有图标都存在时（匹配），计时器停止。
     * 判断机制是：事先定义图标的颜色，通过检查每个图标的文本颜色与背景颜色是否相同来检查每个图标看是否匹配。
     * 找不到匹配的图标了，则玩家获胜。
     */
    public partial class 匹配小游戏 : Form
    {
        //全局计时器
        DateTime TimeNow = new DateTime();
        TimeSpan TimeCount = new TimeSpan();

        Random random = new Random();
        //添加图标（利用Webding字体生成图标，每个字母对应一个图标）
        List<string> icons = new List<string>
        {
            "a","a","h","h","u","u","w","w",
            "n","n","j","j","g","g","x","x"
        };

        //两个引用变量，跟踪第一次和第二次分别单击了那个Label控件
        Label firstClicked = null;//指向第一个Label控件
        Label secondClicked = null;//指向第二个Label控件

        /**
         * 匹配小游戏()函数：
         * 用于调用设计器所需的函数，显示图标的函数以及开启全局计时的函数。
         */
        public 匹配小游戏()
        {
            InitializeComponent();
            giveIcon();//若显示图标，调用方法giveIcon()
            timer3.Start();//开启全局计时
            TimeNow = DateTime.Now;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /**
         * giveIcon()函数：
         * 用于随机分配图标的函数。
         * 该函数遍历TableLayoutPanel中的每个label控件，并对每个控件
         * 执行相同的语句。
         */
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

        /**
         * 匹配小游戏_Load(object sender, EventArgs e)函数：
         * 用于窗体命名。
         */
        private void 匹配小游戏_Load(object sender, EventArgs e)
        {
            this.Text = "找相同";
            this.timer3.Interval = 1000;
        }

        /**
         * label_Click()函数：
         * 此函数用于点击label之后产生的显现图标的效果，
         * 以及检查两图标是否匹配，调用调用CheckForWinner()程序，
         * 以确认玩家是否胜利。
         */
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
                
                //clickedLabel.ForeColor = Color.Black;
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;
                timer1.Start();

                //调用CheckForWinner()程序，检查玩家是否胜利
                CheckForWinner();

                //检查两图标是否匹配
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;

                    //关闭定时器
                    timer1.Stop();

                    return;
                }
                
            }
        }

        /**
         * timer1_Tick_1(object sender, EventArgs e)函数：
         * 计时器1工作的函数。
         * 调用Stop()停止计时器，是两个引用变量firstClicked
         * 和secondClicked来捕捉玩家点击的两个标签，并使标签不可见。
         * 然后将firstClicked和secondClicked引用变量重置为null。
         */
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            //停止计时
            timer1.Stop();

            //隐藏前一个图标
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            TimeCount = DateTime.Now - TimeNow;
        }

        /**
         * CheckForWinner()函数：
         * 该函数使用foreach遍历每个图标，使用相等运算符检查每个图标
         * 颜色是否匹配。若匹配，图标将保持不可见，玩家还没有匹配所有
         * 剩余图标。这种情况下，将使用return语句跳过其余函数。如果遍
         * 历所有标签而不执行return语句，则意味着所有图标匹配。程序显
         * 示MessageBox，调用Close()结束游戏。
         */
        private void CheckForWinner()
        {
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
            timer3.Stop();
            
            //如果没有跳出循环，则说明没有找到不匹配的图标，说明玩家赢了
            MessageBox.Show("恭喜您，成功通过游戏。您所用时为："
                + string.Format("{0:00}:{1:00}:{2:00}", TimeCount.Hours, TimeCount.Minutes, TimeCount.Seconds));
            Close();//结束游戏
        }



    }
}
