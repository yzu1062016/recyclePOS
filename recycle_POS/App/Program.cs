using System;
using System.Windows.Forms;
using System.IO;
using MyFirstApp;
using System.Drawing.Printing;

namespace MyFirstApp
{
    // --------主要頁面---------
    public class MainForm : Form
    {
        private Panel scrollPanel;
        public MainForm()
        {
            
            this.Icon = new Icon("ogjkf-nlbnm-001.ico");
            //----------頁面資訊---------
            this.Text = "仝興行";
            this.Width = 1200;
            this.Height = 1000;
            float defaultFontSize = 30f; // 設定預設字體大小
            this.Font = new System.Drawing.Font(this.Font.FontFamily, defaultFontSize); // 設定窗體的字體
            // 遍歷所有控件並設定字體

            //------------滾動功能----------
            scrollPanel = new Panel(); // 初始化 Panel
            scrollPanel.AutoScroll = true; // 啟用滾動
            scrollPanel.Dock = DockStyle.Fill; // 填滿對話框
            this.Controls.Add(scrollPanel); // 將 Panel 添加到對話框


            //--------有設定按鈕-----------
            Button setting = new Button();
            setting.Text = "物品、價格設定";
            setting.Location = new System.Drawing.Point(50, 50);
            setting.Size = new System.Drawing.Size(300, 70);
            setting.Font = new System.Drawing.Font(setting.Font.FontFamily, 20);
            setting.Click += new EventHandler(NameSetting_Click);
            this.Controls.Add(setting);
            scrollPanel.Controls.Add(setting);

            //--------有設定按鈕-----------
            Button reset = new Button();
            reset.Text = "重置";
            reset.Location = new System.Drawing.Point(350, 50);
            reset.Size = new System.Drawing.Size(300, 70);
            reset.Font = new System.Drawing.Font(reset.Font.FontFamily, 20);
            reset.Click += new EventHandler(Reset_Click);
            this.Controls.Add(reset);
            scrollPanel.Controls.Add(reset);

            //--------有設定按鈕-----------
            Button print = new Button();
            print.Text = "結果";
            print.Location = new System.Drawing.Point(650, 50);
            print.Size = new System.Drawing.Size(300, 70);
            print.Font = new System.Drawing.Font(print.Font.FontFamily, 20);
            print.Click += new EventHandler(Print_Click);
            this.Controls.Add(print);
            scrollPanel.Controls.Add(print);

            // 創建 PictureBox 控件
            PictureBox pictureBox = new PictureBox();
            
            // 設置圖片路徑
            pictureBox.Image = Image.FromFile("images.png"); // 替換為您的圖片路徑
            
            // 設置 PictureBox 的大小和位置
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // 設置圖片顯示模式
            pictureBox.Size = new Size(100, 100); // 設置大小
            pictureBox.Location = new Point(1000, 50); // 設置位置

            // 將 PictureBox 添加到窗體
            this.Controls.Add(pictureBox);
            scrollPanel.Controls.Add(pictureBox);
            
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
        private void Print_Click(object? sender, EventArgs e)
        {
            ShowResultDialog();
        }

        public void DisplayCurrentNames()
        {
            // 清除 scrollPanel 中的所有 Label 和 TextBox 控件
            foreach (var control in scrollPanel.Controls.OfType<Control>().ToList())
            {
                if (control is Label || control is TextBox)
                {
                    scrollPanel.Controls.Remove(control); // 從 scrollPanel 中移除控件
                }
            }
            if (File.Exists("name.txt"))
            {
                string[] names = File.ReadAllLines("name.txt");
                // 新增註記用 TextBox
                TextBox noteTextBox = new TextBox();
                noteTextBox.Location = new System.Drawing.Point(50, 130);
                noteTextBox.Size = new System.Drawing.Size(500, 30);
                scrollPanel.Controls.Add(noteTextBox);

                int yOffset = 200; // 設置初始的 Y 軸偏移量
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
                        scrollPanel.Controls.Add(nameLabel);

                        // 顯示價格
                        Label priceLabel = new Label();
                        priceLabel.Text = $"價格: {parts[1]}   /kg"; // 顯示價格
                        priceLabel.AutoSize = true; // 自動調整大小
                        priceLabel.Location = new System.Drawing.Point(350, yOffset); // 設置位置
                        this.Controls.Add(priceLabel); // 將價格 Label 添加到主要頁面
                        scrollPanel.Controls.Add(priceLabel);

                        // 新增數量 TextBox
                        TextBox first_TextBox = new TextBox();
                        first_TextBox.Location = new System.Drawing.Point(700, yOffset); // 設置位置
                        first_TextBox.Size = new System.Drawing.Size(100, 30);
                        this.Controls.Add(first_TextBox); // 將數量 TextBox 添加到主要頁面
                        scrollPanel.Controls.Add(first_TextBox);

                        // 顯示減號
                        Label minus_Label = new Label();
                        minus_Label.Text = $"-";
                        minus_Label.AutoSize = true; 
                        minus_Label.Location = new System.Drawing.Point(815, yOffset);
                        this.Controls.Add(minus_Label); // 將價格 Label 添加到主要頁面
                        scrollPanel.Controls.Add(minus_Label);

                         // 新增數量 TextBox
                        TextBox secend_TextBox = new TextBox();
                        secend_TextBox.Location = new System.Drawing.Point(860, yOffset); // 設置位置
                        secend_TextBox.Size = new System.Drawing.Size(100, 30);
                        this.Controls.Add(secend_TextBox); // 將數量 TextBox 添加到主要頁面
                        scrollPanel.Controls.Add(secend_TextBox);

                        // 新增顯示差值的 Label
                        Label resultLabel = new Label();
                        resultLabel.Location = new System.Drawing.Point(980, yOffset); // 設置位置
                        resultLabel.AutoSize = true; // 自動調整大小
                        resultLabel.Text = "=    0"; // 初始顯示
                        this.Controls.Add(resultLabel); // 將差值 Label 添加到主要頁面
                        scrollPanel.Controls.Add(resultLabel);

                        first_TextBox.TextChanged += (s, e) => UpdateDifference(first_TextBox, secend_TextBox, resultLabel); // 添加即時更新差值的事件
                        secend_TextBox.TextChanged += (s, e) => UpdateDifference(first_TextBox, secend_TextBox, resultLabel); // 添加即時更新差值的事件
                        first_TextBox.TextChanged += (s, e) => UpdateTotalPrice();
                        secend_TextBox.TextChanged += (s, e) => UpdateTotalPrice();
                        yOffset += 70; // 更新 Y 軸偏移量，以便顯示下一組商品和價格
                    }
                }
                // 顯示總價 Label
                Label totalPriceLabel = new Label();
                totalPriceLabel.Name = "totalPriceLabel"; // 設置名稱以便於查找
                totalPriceLabel.Text = "總價: 0 元"; // 初始總價
                totalPriceLabel.AutoSize = true; // 自動調整大小
                totalPriceLabel.Location = new System.Drawing.Point(50, yOffset); // 設置位置
                this.Controls.Add(totalPriceLabel);
                scrollPanel.Controls.Add(totalPriceLabel);
            }
        }
     
        // 更新差值的方法
        private void UpdateDifference(TextBox firstTextBox, TextBox secondTextBox, Label resultLabel)
        {
            // 嘗試解析數量
            if (double.TryParse(firstTextBox.Text, out double firstValue) && double.TryParse(secondTextBox.Text, out double secondValue))
            {
                double difference = firstValue - secondValue; // 計算差值
                resultLabel.Text = $"=    {difference}"; // 更新顯示的差值
            }
            else
            {
                resultLabel.Text = "=    0"; // 如果解析失敗，顯示為 0
            }
        }
        // 更新總價的方法
        private void UpdateTotalPrice()
        {
            double total = 0;
            
            // 遍歷所有的控件以計算總價
            foreach (var control in scrollPanel.Controls)
            {
                if (control is Label resultLabel)
                {
                    int resultY = resultLabel.Location.Y;
                    // 在 scrollPanel 中查找對應的 priceLabel
                    var priceLabel = scrollPanel.Controls.OfType<Label>().FirstOrDefault(p => p.Location.Y == resultY && p.Text.StartsWith("價格:"));
                    if (priceLabel != null)
                    {
                        // 解析價格
                        if (double.TryParse(priceLabel.Text.Replace("價格: ", "").Replace("   /kg", "").Trim(), out double price))
                        {
                            // 計算總價
                            if (double.TryParse(resultLabel.Text.Replace("=    ", "").Trim(), out double quantity))
                            {
                                total += price *quantity; // 累加總價
                            }
                        }
                    }
                }
            }

            //更新總價 Label
            var totalPriceLabel = scrollPanel.Controls.OfType<Label>().FirstOrDefault(c => c.Name == "totalPriceLabel");
            if (totalPriceLabel != null)
            {
                totalPriceLabel.Text = $"總價: {total:F1} 元"; // 更新顯示的總價
            }
            }
        private void ShowResultDialog()
        {
            // 假設這裡有一個獲取當前行的 nameLabel 和 resultLabel 的邏輯
            var nameLabels = scrollPanel.Controls.OfType<Label>().Where(p => p.Text.StartsWith("物品:")).ToList();
            var resultLabels = scrollPanel.Controls.OfType<Label>().Where(p => p.Text.StartsWith("=")).ToList();

            using (StreamWriter writer = new StreamWriter("result.txt"))
            {
                for (int i = 0; i < nameLabels.Count; i++)
                {
                    var nameLabel = nameLabels[i];
                    var resultLabel = resultLabels.ElementAtOrDefault(i); // 確保不會超出範圍

                    if (resultLabel != null && resultLabel.Text != "=    0")
                    {
                        // 將 nameLabel 和 resultLabel 的內容寫入 result.txt
                        writer.WriteLine($"{nameLabel.Text}, {resultLabel.Text}");
                    }
                }
                // 寫入 totalPriceLabel 的內容
                var totalPriceLabel = scrollPanel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "totalPriceLabel");
                if (totalPriceLabel != null)
                {
                    writer.WriteLine($"{totalPriceLabel.Text}"); // 寫入總價
                }
            }
            CustomDialog dialog = new CustomDialog();
            dialog.ShowDialog();
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
    private MainForm? mainForm; // 儲存 MainForm 的引用
    private Panel scrollPanel; // 新增一個 Panel

    public CustomDialog(MainForm form)
    {
        mainForm = form; // 保存引用'
        this.Icon = new Icon("C:\\Users\\user\\Desktop\\recycle_POS\\App\\1acrr-f3cca-001.ico");
        //---------彈窗資訊----------
        this.Text = "物品、價格設定";
        this.Size = new System.Drawing.Size(800, 700); // 設置對話框大小
        float defaultFontSize = 20f; // 設定預設字體大小
        this.Font = new System.Drawing.Font(this.Font.FontFamily, defaultFontSize); // 設定窗體的字體

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

    public CustomDialog()
    {
        this.Icon = new Icon("C:\\Users\\user\\Desktop\\recycle_POS\\App\\1acrr-f3cca-001.ico");
        this.Text = "result";
        this.Size = new System.Drawing.Size(500, 700); // 設置對話框大小
        float defaultFontSize = 20f; // 設定預設字體大小
        this.Font = new System.Drawing.Font(this.Font.FontFamily, defaultFontSize); // 設定窗體的字體

         //------------滾動功能----------
        scrollPanel = new Panel(); // 初始化 Panel
        scrollPanel.AutoScroll = true; // 啟用滾動
        scrollPanel.Dock = DockStyle.Fill; // 填滿對話框
        this.Controls.Add(scrollPanel); // 將 Panel 添加到對話框

        // 讀取 result.txt 的內容
        LoadResultsFromFile();

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
        deleteButton.Size = new System.Drawing.Size(80, 40);
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
        if (mainForm != null) // 檢查 mainForm 是否為 null
        {
            mainForm.DisplayCurrentNames(); // 調用方法
            this.Close();
        }
    }

    private void Print_Click(object? sender, EventArgs e)
    {
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage; // 註冊列印頁面事件

        PrintDialog printDialog = new PrintDialog();
        printDialog.Document = printDocument;

        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            // printDocument.Print(); // 開始列印
        }
    }
    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        
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
                    deleteButton.Size = new System.Drawing.Size(80, 40);
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
    private void LoadResultsFromFile()
    {
        using (StreamReader reader = new StreamReader("result.txt"))
        {
            string? line;
            int yOffset = 50; // 設定初始 Y 軸偏移量

            while ((line = reader.ReadLine()) != null)
            {
                // 假設每行格式為 "name, result"
                var parts = line.Split(',');

                if (parts.Length == 2)
                {
                    // 為每一行創建新的 nameLabel 和 resultLabel
                    Label nameLabel = new Label(); // 新建 nameLabel
                    Label resultLabel = new Label(); // 新建 resultLabel

                    // 確保 nameLabel 和 resultLabel 被正確初始化
                    nameLabel.Text = parts[0].Trim(); // 設定 nameLabel 的文本
                    nameLabel.Location = new System.Drawing.Point(50, yOffset);
                    nameLabel.Size = new System.Drawing.Size(200, 30); // 調整大小

                    resultLabel.Text = parts[1].Trim(); // 設定 resultLabel 的文本
                    resultLabel.Location = new System.Drawing.Point(250, yOffset);
                    resultLabel.Size = new System.Drawing.Size(200, 30); // 調整大小

                    // 將控件添加到 scrollPanel
                    scrollPanel.Controls.Add(nameLabel);
                    scrollPanel.Controls.Add(resultLabel);

                    // 更新 Y 軸偏移量以顯示下一行
                    yOffset += 40; // 調整偏移量以便顯示下一組
                }
                else if (parts.Length == 1 && parts[0].StartsWith("總價:")) // 檢查是否為總價行
                {
                    // 顯示總價資訊
                    Label totalPriceLabel = new Label();
                    totalPriceLabel.Text =parts[0].Trim(); // 設定總價文本，保留兩位小數
                    totalPriceLabel.Location = new System.Drawing.Point(50, yOffset); // 設定位置
                    totalPriceLabel.Size = new System.Drawing.Size(200, 30); // 調整大小
                    scrollPanel.Controls.Add(totalPriceLabel); // 將總價 Label 添加到 scrollPanel
                   
                }
            }
            //-----------有儲存按鈕--------------
            Button pinrt = new Button();
            pinrt.Text = "列印";
            pinrt.Location = new System.Drawing.Point(300, yOffset);
            pinrt.Size = new System.Drawing.Size(150, 40);
            pinrt.Font = new System.Drawing.Font(pinrt.Font.FontFamily, 16);
            pinrt.Click += new EventHandler(Print_Click);
            this.Controls.Add(pinrt);
            scrollPanel.Controls.Add(pinrt);
        }
    }
}
