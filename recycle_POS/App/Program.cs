using System;
using System.Windows.Forms;
using System.IO;
using MyFirstApp;

namespace MyFirstApp
{
    // --------主要頁面---------
    public class MainForm : Form
    {
        public MainForm()
        {
            //----------頁面資訊---------
            this.Text = "test";
            this.Width = 800;
            this.Height = 800;


            //--------有設定按鈕-----------
            Button setting = new Button();
            setting.Text = "物品、價格設定";
            setting.Location = new System.Drawing.Point(50, 50);
            setting.Size = new System.Drawing.Size(300, 70);
            setting.Font = new System.Drawing.Font(setting.Font.FontFamily, 20);
            setting.Click += new EventHandler(NameSetting_Click);
            this.Controls.Add(setting);

            //--------有設定按鈕-----------
            Button reset = new Button();
            reset.Text = "TEST";
            reset.Location = new System.Drawing.Point(350, 50);
            reset.Size = new System.Drawing.Size(300, 70);
            reset.Font = new System.Drawing.Font(reset.Font.FontFamily, 20);
            reset.Click += new EventHandler(Reset_Click);
            this.Controls.Add(reset);
            // 顯示當前 name.txt 的資訊
            DisplayCurrentNames();

        }

        //---------點擊設定按鈕-----------
        private void NameSetting_Click(object? sender, EventArgs e)
        {
            CustomDialog dialog = new CustomDialog(this);
            dialog.ShowDialog();
        }

        private void Reset_Click(object? sender, EventArgs e)
        {
            DisplayCurrentNames();
        }


        public void DisplayCurrentNames()
        {
            // 清除現有的商品和價格 Label
            foreach (var control in this.Controls.OfType<Label>().ToList())
            {
                this.Controls.Remove(control);
            }
            // 清除現有的數量 TextBox
            foreach (var control in this.Controls.OfType<TextBox>().ToList())
            {
                this.Controls.Remove(control);
            }
            // 清除總價 Label
            foreach (var control in this.Controls.OfType<Label>().Where(c => c.Name == "totalPriceLabel").ToList())
            {
                this.Controls.Remove(control);
            }
            if (File.Exists("name.txt"))
            {
                string[] names = File.ReadAllLines("name.txt");
                int yOffset = 150; // 設置初始的 Y 軸偏移量
                foreach (string line in names)
                {
                    string[] parts = line.Split(','); // 以逗號分隔名稱和價格
                    if (parts.Length >= 2) // 確保有名稱和價格
                    {
                        // 顯示商品名稱
                        Label nameLabel = new Label();
                        nameLabel.Text = $"物品: {parts[0]}"; // 顯示商品名稱
                        nameLabel.AutoSize = true; // 自動調整大小
                        nameLabel.Location = new System.Drawing.Point(50, yOffset); // 設置位置
                        this.Controls.Add(nameLabel); // 將商品名稱 Label 添加到主要頁面

                        // 顯示價格
                        Label priceLabel = new Label();
                        priceLabel.Text = $"價格: {parts[1]}   /kg"; // 顯示價格
                        priceLabel.AutoSize = true; // 自動調整大小
                        priceLabel.Location = new System.Drawing.Point(200, yOffset); // 設置位置
                        this.Controls.Add(priceLabel); // 將價格 Label 添加到主要頁面

                        // 新增數量 TextBox
                        TextBox quantityTextBox = new TextBox();
                        quantityTextBox.Location = new System.Drawing.Point(350, yOffset); // 設置位置
                        quantityTextBox.Size = new System.Drawing.Size(100, 30);
                        quantityTextBox.TextChanged += (s, e) => UpdateTotalPrice(); // 添加即時更新總價的事件
                        this.Controls.Add(quantityTextBox); // 將數量 TextBox 添加到主要頁面

                        yOffset += 30; // 更新 Y 軸偏移量，以便顯示下一組商品和價格
                    }
                }
                // 顯示總價 Label
                Label totalPriceLabel = new Label();
                totalPriceLabel.Name = "totalPriceLabel"; // 設置名稱以便於查找
                totalPriceLabel.Text = "總價: 0 元"; // 初始總價
                totalPriceLabel.AutoSize = true; // 自動調整大小
                totalPriceLabel.Location = new System.Drawing.Point(50, yOffset); // 設置位置
                this.Controls.Add(totalPriceLabel);
            }
        }
     
// 更新總價的方法
    private void UpdateTotalPrice()
    {
        double total = 0;
        
        // 遍歷所有的控件以計算總價
        foreach (var control in this.Controls)
        {
            if (control is TextBox quantityTextBox)
            {
                // 確保對應的價格 Label 在前面
                int index = this.Controls.IndexOf(quantityTextBox);
                if (index > 0 && this.Controls[index - 1] is Label priceLabel)
                {
                    // 解析價格
                    if (double.TryParse(priceLabel.Text.Replace("價格: ", "").Replace("   /kg", "").Trim(), out double price))
                    {
                        // 計算總價
                        if (double.TryParse(quantityTextBox.Text, out double quantity))
                        {
                            total += price * quantity; // 累加總價
                        }
                    }
                }
            }
        }

        //更新總價 Label
        var totalPriceLabel = this.Controls.OfType<Label>().FirstOrDefault(c => c.Name == "totalPriceLabel");
        if (totalPriceLabel != null)
        {
            totalPriceLabel.Text = $"總價: {total} 元"; // 更新顯示的總價
        }
    }
            //---------頁面初始化---------------
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }

//----------------設定彈窗--------------
public partial class CustomDialog : Form
{
    private MainForm mainForm; // 儲存 MainForm 的引用
    private Panel scrollPanel; // 新增一個 Panel

    public CustomDialog(MainForm form)
    {
        mainForm = form; // 保存引用
        //---------彈窗資訊----------
        this.Text = "物品、價格設定";
        this.Size = new System.Drawing.Size(800, 700); // 設置對話框大小

        //------------滾動功能----------
        scrollPanel = new Panel(); // 初始化 Panel
        scrollPanel.AutoScroll = true; // 啟用滾動
        scrollPanel.Dock = DockStyle.Fill; // 填滿對話框
        this.Controls.Add(scrollPanel); // 將 Panel 添加到對話框

        // -------讀取原先的設定，顯示在輸入框---------
        LoadNamesFromFile();

        //------------有新增欄位按鈕---------
        Button new_lable = new Button();
        new_lable.Text = "新增欄位";
        new_lable.Location = new System.Drawing.Point(50, 50);
        new_lable.Size = new System.Drawing.Size(200, 50);
        new_lable.Font = new System.Drawing.Font(new_lable.Font.FontFamily, 20);
        new_lable.Click += new EventHandler(NewLabel_Click);
        this.Controls.Add(new_lable);
        scrollPanel.Controls.Add(new_lable);

        //-----------有儲存按鈕--------------
        Button save = new Button();
        save.Text = "儲存";
        save.Location = new System.Drawing.Point(300, 50);
        save.Size = new System.Drawing.Size(200, 50);
        save.Font = new System.Drawing.Font(save.Font.FontFamily, 20);
        save.Click += new EventHandler(Save_Click);
        this.Controls.Add(save);
        scrollPanel.Controls.Add(save);

    }

    //----------點擊新增欄位按紐----------
     private void NewLabel_Click(object? sender, EventArgs e)
        {
            // 新增第一個 TextBox
            TextBox nameTextBox = new TextBox();
            nameTextBox.Location = new System.Drawing.Point(50, 120 + scrollPanel.Controls.OfType<TextBox>().Count() * 40);
            nameTextBox.Size = new System.Drawing.Size(200, 30);
            scrollPanel.Controls.Add(nameTextBox); // 將名稱 TextBox 添加到 scrollPanel

            // 新增價格 TextBox
            TextBox priceTextBox = new TextBox();
            priceTextBox.Location = new System.Drawing.Point(260, nameTextBox.Location.Y);
            priceTextBox.Size = new System.Drawing.Size(200, 30);
            scrollPanel.Controls.Add(priceTextBox); // 將價格 TextBox 添加到 scrollPanel

            // 新增刪除按鈕
            Button deleteButton = new Button();
            deleteButton.Text = "刪除";
            deleteButton.Location = new System.Drawing.Point(470, nameTextBox.Location.Y);
            deleteButton.Size = new System.Drawing.Size(80, 30);
            deleteButton.Click += (s, args) => 
            {
                scrollPanel.Controls.Remove(nameTextBox); // 刪除名稱 TextBox
                scrollPanel.Controls.Remove(priceTextBox); // 刪除價格 TextBox
                scrollPanel.Controls.Remove(deleteButton); // 刪除按鈕
            };
            scrollPanel.Controls.Add(deleteButton);
        }

    //---------------點擊儲存按鈕---------------
    private void Save_Click(object? sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("name.txt"))
            {
                foreach (var control in scrollPanel.Controls)
                {
                    if (control is TextBox nameTextBox)
                    {
                        // 確保有對應的價格 TextBox
                        int index = scrollPanel.Controls.IndexOf(nameTextBox);
                        if (index + 1 < scrollPanel.Controls.Count && scrollPanel.Controls[index + 1] is TextBox priceTextBox)
                        {
                            writer.WriteLine($"{nameTextBox.Text},{priceTextBox.Text}"); // 儲存名稱和價格，使用逗號分隔
                        }
                     }
                }
            }
            mainForm.DisplayCurrentNames();
            this.Close();
        }
    
    private void LoadNamesFromFile()
    {
        if (File.Exists("name.txt"))
        {
            string[] names = File.ReadAllLines("name.txt");
            foreach (string line in names)
            {
                string[] parts = line.Split(','); // 以逗號分隔名稱和價格
                if (parts.Length >= 2) // 確保有名稱和價格
                {
                    TextBox nameTextBox = new TextBox();
                    nameTextBox.Text = parts[0]; // 設置 TextBox 的文本為文件中的名稱
                    nameTextBox.Location = new System.Drawing.Point(50, 120 + scrollPanel.Controls.OfType<TextBox>().Count() * 40);
                    nameTextBox.Size = new System.Drawing.Size(200, 30);
                    scrollPanel.Controls.Add(nameTextBox); // 將名稱 TextBox 添加到 scrollPanel

                    TextBox priceTextBox = new TextBox();
                    priceTextBox.Text = parts[1]; // 設置 TextBox 的文本為文件中的價格
                    priceTextBox.Location = new System.Drawing.Point(260, nameTextBox.Location.Y);
                    priceTextBox.Size = new System.Drawing.Size(200, 30);
                    scrollPanel.Controls.Add(priceTextBox); // 將價格 TextBox 添加到 scrollPanel

                    // 新增刪除按鈕
                    Button deleteButton = new Button();
                    deleteButton.Text = "刪除";
                    deleteButton.Location = new System.Drawing.Point(470, nameTextBox.Location.Y);
                    deleteButton.Size = new System.Drawing.Size(80, 30);
                    deleteButton.Click += (s, args) => 
                    {
                        scrollPanel.Controls.Remove(nameTextBox); // 刪除名稱 TextBox
                        scrollPanel.Controls.Remove(priceTextBox); // 刪除價格 TextBox
                        scrollPanel.Controls.Remove(deleteButton); // 刪除按鈕
                    };
                    scrollPanel.Controls.Add(deleteButton);
                }
            }
        }
    }
}