using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComForm
{
    
    public partial class Form1 : Form
    {
        Connection com1 = new Connection();
        
        public Form1()
        {
            InitializeComponent();
            cb_PortNames.Items.AddRange(SerialPort.GetPortNames());
            com1.MainForm = this;
            com1.Display = richTextBox1;
            b_con.Enabled = false;
            b_ChooseFile.Enabled = false;
            richTextBox1.AppendText("Добро пожаловать!\nПеред началом работы выберите порт из списка и откройте его.\n\n");
        }

        /// <summary>
        /// Пишет в лог, есть ли соединение
        /// </summary>
        private void b_con_Click(object sender, EventArgs e)
        {
            if (com1.IsConnected())
            {
                richTextBox1.AppendText("[" + DateTime.Now + "] " + cb_PortNames.SelectedItem.ToString() + ": Соединение установлено\n");
            }
            else
            {
                richTextBox1.AppendText("[" + DateTime.Now + "] " + cb_PortNames.SelectedItem.ToString() + ": Соединение отсутствует\n");
            }
        }

        /// <summary>
        /// Открывает порт com1
        /// </summary>
        private void b_OpenPort_Click(object sender, EventArgs e)
        {
            if (cb_PortNames.SelectedItem != null)
            {
                com1.Log = richTextBox1;
                com1.setPortName(cb_PortNames.SelectedItem.ToString()); //давать список для выбора из существующих?
                com1.OpenPort();
                if (com1.Port.IsOpen)
                {
                    richTextBox1.AppendText("[" + DateTime.Now + "] Порт " + com1.Port.PortName + " открыт\n");
                }
                b_con.Enabled = true;
                b_ChooseFile.Enabled = true;
            }
            else
            {
                MessageBox.Show("Порт не выбран!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void b_ChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (com1.IsConnected())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    com1.WriteData(openFileDialog.FileName, Connection.FrameType.FILEOK);
                    richTextBox1.ScrollToCaret();
                }
            }
            else
            {
                MessageBox.Show("Соединение отсутствует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            com1.ClosePort();
        }

        private void b_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа реализует взаимодействие 2-х ПК, соединенных через интерфейс RS-232C,\n" +
                            "с функцией передачи файла и возможностью докачки после восстановления прерванной связи.\n\n" +
                            "Программу разработали студенты МГТУ им. Н.Э.Баумана группы ИУ5-63:\n\n" +
                            "Волков А.С. (прикладной уровень)\n" +
                            "Королев С.В. (канальный уровень)\n" +
                            "Беспалова У.А. (физический уровень)",
                            "О программе",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
      
    }
}
