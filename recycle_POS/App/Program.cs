using System;
using System.Windows.Forms;
using System.IO;
using MyFirstApp;
using System.Drawing.Printing;
using System.Drawing;
using OfficeOpenXml;

namespace MyFirstApp
{
    // --------主要頁面---------
    public class MainForm : Form
    {
        private Panel scrollPanel; //右側滾動功能
        public MainForm()
        {
            
            //----------頁面資訊---------
            this.Icon = new Icon("ogjkf-nlbnm-001.ico");
            this.Text = "仝興行";
            this.Width = 950;
            this.Height = 1000;
            float defaultFontSize = 25f;
            this.Font = new System.Drawing.Font(this.Font.FontFamily, defaultFontSize);

            //------------滾動功能----------
            scrollPanel = new Panel();
            scrollPanel.AutoScroll = true; 
            scrollPanel.Dock = DockStyle.Fill; 
            this.Controls.Add(scrollPanel);

            //--------設定按鈕-----------
            Button setting = new Button();
            setting.Text = "品項、價格設定";
            setting.Location = new System.Drawing.Point(25, 25);
            setting.Size = new System.Drawing.Size(250, 70);
            setting.Font = new System.Drawing.Font(setting.Font.FontFamily, 20);
            setting.Click += new EventHandler(NameSetting_Click);
            this.Controls.Add(setting);
            scrollPanel.Controls.Add(setting);

            //--------重製按鈕-----------
            Button reset = new Button();
            reset.Text = "重置";
            reset.Location = new System.Drawing.Point(300, 25);
            reset.Size = new System.Drawing.Size(150, 70);
            reset.Font = new System.Drawing.Font(reset.Font.FontFamily, 20);
            reset.Click += new EventHandler(Reset_Click);
            this.Controls.Add(reset);
            scrollPanel.Controls.Add(reset);

            //--------結果按鈕-----------
            Button result = new Button();
            result.Text = "結果";
            result.Location = new System.Drawing.Point(475, 25);
            result.Size = new System.Drawing.Size(250, 70);
            result.Font = new System.Drawing.Font(result.Font.FontFamily, 20);
            result.Click += new EventHandler(Result_Click);
            this.Controls.Add(result);
            scrollPanel.Controls.Add(result);

            // ------右邊青蛙圖片顯示---------
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile("images.png");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; 
            pictureBox.Size = new Size(100, 100); 
            pictureBox.Location = new Point(750, 10); 
            this.Controls.Add(pictureBox);
            scrollPanel.Controls.Add(pictureBox);
            
            //----顯示當前 name.txt 的資訊-----------
            DisplayCurrentNames();

        }

        //---------點擊設定按鈕-----------
        private void NameSetting_Click(object? sender, EventArgs e)
        {
            CustomDialog dialog = new CustomDialog(this);
            dialog.ShowDialog();
        }
        //--------點擊重製按鈕------------
        private void Reset_Click(object? sender, EventArgs e)
        {
            DisplayCurrentNames();
        }
        //--------點擊結果按鈕---------
        private void Result_Click(object? sender, EventArgs e)
        {
            ShowResultDialog();
        }

        //--------顯示主頁面資訊----------
        public void DisplayCurrentNames()
        {
            //--------清除 scrollPanel 中的所有 Label 和 TextBox 控件-------------
            foreach (var control in scrollPanel.Controls.OfType<Control>().ToList())
            {
                if (control is Label || control is TextBox)
                {
                    scrollPanel.Controls.Remove(control);
                }
            }
            if (File.Exists("name.txt"))
            {
                string[] names = File.ReadAllLines("name.txt");

                //--------新增註記用 TextBox---------
                TextBox noteTextBox = new TextBox();
                noteTextBox.Name = "noteTextBox";
                noteTextBox.Location = new System.Drawing.Point(25, 100);
                noteTextBox.Size = new System.Drawing.Size(500, 30);
                scrollPanel.Controls.Add(noteTextBox);

                int yOffset = 170;
                foreach (string line in names)
                {
                    string[] parts = line.Split(','); //以逗號分隔名稱和價格
                    if (parts.Length >= 2) 
                    {
                        //---------顯示商品名稱----------
                        Label nameLabel = new Label();
                        nameLabel.Text = $"品項: {parts[0]}";
                        nameLabel.AutoSize = true;
                        nameLabel.Location = new System.Drawing.Point(25, yOffset);
                        this.Controls.Add(nameLabel);
                        scrollPanel.Controls.Add(nameLabel);

                        //------顯示價格----------
                        Label priceLabel = new Label();
                        priceLabel.Text = $"價格: {parts[1]}";
                        priceLabel.AutoSize = true; 
                        priceLabel.Location = new System.Drawing.Point(300, yOffset); 
                        this.Controls.Add(priceLabel); 
                        scrollPanel.Controls.Add(priceLabel);

                        //-------新增數量 TextBox----------
                        TextBox first_TextBox = new TextBox();
                        first_TextBox.Location = new System.Drawing.Point(500, yOffset); 
                        first_TextBox.Size = new System.Drawing.Size(100, 15);
                        this.Controls.Add(first_TextBox);
                        scrollPanel.Controls.Add(first_TextBox);

                        //--------顯示減號---------
                        Label minus_Label = new Label();
                        minus_Label.Text = $"-";
                        minus_Label.AutoSize = true; 
                        minus_Label.Location = new System.Drawing.Point(610, yOffset);
                        this.Controls.Add(minus_Label);
                        scrollPanel.Controls.Add(minus_Label);

                         //---------新增數量 TextBox-----------
                        TextBox secend_TextBox = new TextBox();
                        secend_TextBox.Location = new System.Drawing.Point(650, yOffset); 
                        secend_TextBox.Size = new System.Drawing.Size(100, 15);
                        this.Controls.Add(secend_TextBox);
                        scrollPanel.Controls.Add(secend_TextBox);

                        //---------新增顯示差值的 Label------------
                        Label resultLabel = new Label();
                        resultLabel.Location = new System.Drawing.Point(770, yOffset); 
                        resultLabel.AutoSize = true; 
                        resultLabel.Text = "=    0"; 
                        this.Controls.Add(resultLabel);
                        scrollPanel.Controls.Add(resultLabel);

                        //-------即時顯示差值、總價----------
                        first_TextBox.TextChanged += (s, e) => UpdateDifference(first_TextBox, secend_TextBox, resultLabel); 
                        secend_TextBox.TextChanged += (s, e) => UpdateDifference(first_TextBox, secend_TextBox, resultLabel);
                        first_TextBox.TextChanged += (s, e) => UpdateTotalPrice();
                        secend_TextBox.TextChanged += (s, e) => UpdateTotalPrice();

                        yOffset += 50; 
                    }
                }
                // 顯示總價 Label
                Label totalPriceLabel = new Label();
                totalPriceLabel.Name = "totalPriceLabel";
                totalPriceLabel.Text = "總價: 0 元"; 
                totalPriceLabel.AutoSize = true; 
                totalPriceLabel.Location = new System.Drawing.Point(25, yOffset); 
                this.Controls.Add(totalPriceLabel);
                scrollPanel.Controls.Add(totalPriceLabel);
            }
        }
     
        //---------及時更新差值-------
        private void UpdateDifference(TextBox firstTextBox, TextBox secondTextBox, Label resultLabel)
        {
            //-------解析數量--------
            if (double.TryParse(firstTextBox.Text, out double firstValue) && double.TryParse(secondTextBox.Text, out double secondValue))
            {
                double difference = firstValue - secondValue; 
                resultLabel.Text = $"=    {difference}"; 
            }
            else
            {
                resultLabel.Text = "=    0";
            }
        }
        //---------及時更新總價--------
        private void UpdateTotalPrice()
        {
            double total = 0;
            
            //------遍歷所有的控件以計算總價---------
            foreach (var control in scrollPanel.Controls)
            {
                if (control is Label resultLabel)
                {
                    int resultY = resultLabel.Location.Y;
                    var priceLabel = scrollPanel.Controls.OfType<Label>().FirstOrDefault(p => p.Location.Y == resultY && p.Text.StartsWith("價格:"));
                    if (priceLabel != null)
                    {
                        //-----解析價格---------
                        if (double.TryParse(priceLabel.Text.Replace("價格: ", "").Trim(), out double price))
                        {
                            //------計算總價-------
                            if (double.TryParse(resultLabel.Text.Replace("=    ", "").Trim(), out double quantity))
                            {
                                total += price *quantity; //累加總價
                            }
                        }
                    }
                }
            }

            //-------更新總價 Label----------
            var totalPriceLabel = scrollPanel.Controls.OfType<Label>().FirstOrDefault(c => c.Name == "totalPriceLabel");
            if (totalPriceLabel != null)
            {
                totalPriceLabel.Text = $"總價: {total:F1} 元";
            }
        }

        //--------將結果儲存到result.txt----------
        private void ShowResultDialog()
        {
            var nameLabels = scrollPanel.Controls.OfType<Label>().Where(p => p.Text.StartsWith("品項:")).ToList();
            var resultLabels = scrollPanel.Controls.OfType<Label>().Where(p => p.Text.StartsWith("=")).ToList();
            var noteTextBox = scrollPanel.Controls.OfType<TextBox>().FirstOrDefault(l => l.Name == "noteTextBox");
            
            using (StreamWriter writer = new StreamWriter("result.txt"))
            { 

                for (int i = 0; i < nameLabels.Count; i++)
                {
                    var nameLabel = nameLabels[i];
                    var resultLabel = resultLabels.ElementAtOrDefault(i);
                    if (resultLabel != null)
                    {
                        writer.WriteLine($"{nameLabel.Text}, {resultLabel.Text}");
                    }
                }

                var totalPriceLabel = scrollPanel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "totalPriceLabel");
                if (totalPriceLabel != null)
                {
                    writer.WriteLine($"{totalPriceLabel.Text}"); 
                }

                if (noteTextBox != null && !string.IsNullOrEmpty(noteTextBox.Text))
                {
                    writer.WriteLine($"註記: {noteTextBox.Text}"); 
                }
            }
            CustomDialog dialog = new CustomDialog();
            dialog.ShowDialog();
        }

        //---------頁面初始化---------------
        [STAThread]
        static void Main()
        {
            #pragma warning disable CS0618
            ExcelPackage.License.SetNonCommercialPersonal("仝興行");
            #pragma warning restore CS0618
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

//--------------彈出的視窗--------------
public partial class CustomDialog : Form
{
    private MainForm? mainForm; //儲存 MainForm 的引用
    private Panel scrollPanel; //右側滾動功能

    //----------品項、價格設定視窗----------
    public CustomDialog(MainForm form)
    {
        mainForm = form;
        this.Icon = new Icon("ogjkf-nlbnm-001.ico");
        //---------彈窗資訊----------
        this.Text = "品項、價格設定";
        this.Size = new System.Drawing.Size(800, 700);
        float defaultFontSize = 20f;
        this.Font = new System.Drawing.Font(this.Font.FontFamily, defaultFontSize);

        //------------滾動功能----------
        scrollPanel = new Panel(); 
        scrollPanel.AutoScroll = true;
        scrollPanel.Dock = DockStyle.Fill; 
        this.Controls.Add(scrollPanel);

        // -------讀取原先的設定，顯示在輸入框---------
        LoadNamesFromFile();

        //----------新增欄位按鈕---------
        Button new_lable = new Button();
        new_lable.Text = "新增欄位";
        new_lable.Location = new System.Drawing.Point(50, 50);
        new_lable.Size = new System.Drawing.Size(200, 50);
        new_lable.Font = new System.Drawing.Font(new_lable.Font.FontFamily, 20);
        new_lable.Click += new EventHandler(NewLabel_Click);
        this.Controls.Add(new_lable);
        scrollPanel.Controls.Add(new_lable);

        //---------儲存按鈕------------
        Button save = new Button();
        save.Text = "儲存";
        save.Location = new System.Drawing.Point(300, 50);
        save.Size = new System.Drawing.Size(200, 50);
        save.Font = new System.Drawing.Font(save.Font.FontFamily, 20);
        save.Click += new EventHandler(Save_Click);
        this.Controls.Add(save);
        scrollPanel.Controls.Add(save);

    }

    //---------結果視窗---------
    public CustomDialog()
    {
        //---------彈窗資訊----------
        this.Icon = new Icon("ogjkf-nlbnm-001.ico");
        this.Text = "result";
        this.Size = new System.Drawing.Size(500, 700);
        float defaultFontSize = 20f;
        this.Font = new System.Drawing.Font(this.Font.FontFamily, defaultFontSize); 

         //------------滾動功能----------
        scrollPanel = new Panel(); 
        scrollPanel.AutoScroll = true; 
        scrollPanel.Dock = DockStyle.Fill;
        this.Controls.Add(scrollPanel);

        //-----顯示 result.txt 的內容----------
        LoadResultsFromFile();

    }

    //----------點擊新增欄位按紐----------
     private void NewLabel_Click(object? sender, EventArgs e)
    {
        //-------新增品項 TextBox----------
        TextBox nameTextBox = new TextBox();
        nameTextBox.Location = new System.Drawing.Point(50, 120 + scrollPanel.Controls.OfType<TextBox>().Count() * 40);
        nameTextBox.Size = new System.Drawing.Size(200, 30);
        scrollPanel.Controls.Add(nameTextBox); 

        //------新增價格 TextBox---------
        TextBox priceTextBox = new TextBox();
        priceTextBox.Location = new System.Drawing.Point(260, nameTextBox.Location.Y);
        priceTextBox.Size = new System.Drawing.Size(200, 30);
        scrollPanel.Controls.Add(priceTextBox);

        //-------新增刪除按鈕---------
        Button deleteButton = new Button();
        deleteButton.Text = "刪除";
        deleteButton.Location = new System.Drawing.Point(470, nameTextBox.Location.Y);
        deleteButton.Size = new System.Drawing.Size(80, 40);
        deleteButton.Click += (s, args) => 
        {
            scrollPanel.Controls.Remove(nameTextBox); 
            scrollPanel.Controls.Remove(priceTextBox); 
            scrollPanel.Controls.Remove(deleteButton); 
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
                    int index = scrollPanel.Controls.IndexOf(nameTextBox);
                    if (index + 1 < scrollPanel.Controls.Count && scrollPanel.Controls[index + 1] is TextBox priceTextBox)
                    {
                        writer.WriteLine($"{nameTextBox.Text},{priceTextBox.Text}");
                    }
                    }
            }
        }
        if (mainForm != null)
        {
            mainForm.DisplayCurrentNames();
            this.Close();
        }
    }

    //--------點擊列印按鈕---------
    private void Print_Click(object? sender, EventArgs e)
    {
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;

        PrintDialog printDialog = new PrintDialog();
        printDialog.Document = printDocument;

        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            printDocument.Print();
            MessageBox.Show($"success");
            
        }
    }
    //-------要列印的資訊內容--------
    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        if (e?.Graphics == null) return;
        
        float yPos = 50; 
        float leftMargin = 50; 
        Font printFont = new Font("Arial", 12);

        if (File.Exists("result.txt"))
        {
            string[] lines = File.ReadAllLines("result.txt");

            e.Graphics.DrawString("仝興行", printFont, Brushes.Black, leftMargin, yPos);
            yPos += printFont.GetHeight(e.Graphics) + 20;

            foreach (string line in lines)
            {
                if (!line.Contains("=    0")){
                    string lineWithoutComma = line.Replace(",", "");
                    e.Graphics.DrawString(lineWithoutComma, printFont, Brushes.Black, leftMargin, yPos);
                    yPos += printFont.GetHeight(e.Graphics) + 10;
                }
            }
        }
    }

    //----------儲存日報表---------
    private void SaveExecl_Click(object? sender, EventArgs e)
    {

       //-----獲取當天日期-------
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string fileName = $"Report_{today}.xlsx";
        string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

        //--------讀取 name.txt 和 result.txt 文件-----------
        string[] nameLines = File.ReadAllLines("name.txt"); 
        string[] resultLines = File.ReadAllLines("result.txt");

        //---------讀取舊檔資料（若存在）---------------
        Dictionary<string, (double price, double number)> dataDict = new Dictionary<string, (double, double)>();
        if (File.Exists(filePath))
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets["報表"];
                int row = 2;
                while (!string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                {
                    string name = worksheet.Cells[row, 1].Text.Trim();
                    double price = double.Parse(worksheet.Cells[row, 2].Text);
                    double number = double.Parse(worksheet.Cells[row, 3].Text);

                    dataDict[name] = (price, number);
                    row++;
                }
            }
        }
        //----------加入新的資料----------
        for (int i = 0; i < nameLines.Length; i++)
        {
            var nameParts = nameLines[i].Trim().Split(',');
            var resultParts = resultLines[i].Trim().Split('=');

            string name = nameParts[0];
            double price = double.Parse(nameParts[1]);
            double number = double.Parse(resultParts[1]);

            if (dataDict.ContainsKey(name))
            {
                var old = dataDict[name];
                dataDict[name] = (price, old.number + number);
            }
            else
            {
                dataDict[name] = (price, number);
            }
        }

        //--------寫入報表--------
        using (ExcelPackage package = new ExcelPackage())
        {

            var worksheet = package.Workbook.Worksheets.Add("報表");

            worksheet.Cells[1, 1].Value = "品項"; 
            worksheet.Cells[1, 2].Value = "價格";
            worksheet.Cells[1, 3].Value = "公斤數";

            int row = 2;
            foreach (var entry in dataDict)
            {
                worksheet.Cells[row, 1].Value = entry.Key;
                worksheet.Cells[row, 2].Value = entry.Value.price;
                worksheet.Cells[row, 3].Value = entry.Value.number;
                worksheet.Cells[row, 4].Value = entry.Value.price * entry.Value.number;
                row++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            FileInfo fi = new FileInfo(filePath);
            package.SaveAs(fi);
        }

        MessageBox.Show($"數據已保存到 {fileName}");
    }
    
    //------------載入name.txt資訊-----------
    private void LoadNamesFromFile()
    {
        if (File.Exists("name.txt"))
        {
            string[] names = File.ReadAllLines("name.txt");
            foreach (string line in names)
            {
                string[] parts = line.Split(','); 
                if (parts.Length >= 2)
                {
                    //---------新增品名TextBox----------
                    TextBox nameTextBox = new TextBox();
                    nameTextBox.Text = parts[0]; 
                    nameTextBox.Location = new System.Drawing.Point(50, 120 + scrollPanel.Controls.OfType<TextBox>().Count() * 40);
                    nameTextBox.Size = new System.Drawing.Size(200, 30);
                    scrollPanel.Controls.Add(nameTextBox); 

                    //---------新增價格TextBox----------
                    TextBox priceTextBox = new TextBox();
                    priceTextBox.Text = parts[1]; 
                    priceTextBox.Location = new System.Drawing.Point(260, nameTextBox.Location.Y);
                    priceTextBox.Size = new System.Drawing.Size(200, 30);
                    scrollPanel.Controls.Add(priceTextBox); 

                    //---------新增刪除按鈕----------
                    Button deleteButton = new Button();
                    deleteButton.Text = "刪除";
                    deleteButton.Location = new System.Drawing.Point(470, nameTextBox.Location.Y);
                    deleteButton.Size = new System.Drawing.Size(80, 40);
                    deleteButton.Click += (s, args) => 
                    {
                        scrollPanel.Controls.Remove(nameTextBox); 
                        scrollPanel.Controls.Remove(priceTextBox);
                        scrollPanel.Controls.Remove(deleteButton);
                    };
                    scrollPanel.Controls.Add(deleteButton);
                }
            }
        }
    }

    //-----------載入result.txt資訊-----------
    private void LoadResultsFromFile()
    {
        using (StreamReader reader = new StreamReader("result.txt"))
        {
            string? line;
            int yOffset = 50;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                if (parts.Length == 2 && parts[1].Replace(" ", "") != "=0")
                {
                    Label nameLabel = new Label(); 
                    Label resultLabel = new Label();

                    nameLabel.Text = parts[0].Trim(); 
                    nameLabel.Location = new System.Drawing.Point(50, yOffset);
                    nameLabel.Size = new System.Drawing.Size(200, 30);
                    resultLabel.Text = parts[1].Trim(); 
                    resultLabel.Location = new System.Drawing.Point(250, yOffset);
                    resultLabel.Size = new System.Drawing.Size(200, 30);

                    scrollPanel.Controls.Add(nameLabel);
                    scrollPanel.Controls.Add(resultLabel);

                    yOffset += 40; 
                }
                else if (parts.Length == 1 && parts[0].StartsWith("總價:"))
                {
                    Label totalPriceLabel = new Label();
                    totalPriceLabel.Text =parts[0].Trim(); 
                    totalPriceLabel.Location = new System.Drawing.Point(50, yOffset); 
                    totalPriceLabel.Size = new System.Drawing.Size(200, 30); 
                    scrollPanel.Controls.Add(totalPriceLabel); 
                    yOffset += 40;
                   
                }
                else if (parts.Length == 1 && parts[0].StartsWith("註記:"))
                {
                    Label noteTextBox = new Label();
                    noteTextBox.Text =parts[0].Trim(); 
                    noteTextBox.Location = new System.Drawing.Point(50, yOffset); 
                    noteTextBox.Size = new System.Drawing.Size(400, 30);
                    scrollPanel.Controls.Add(noteTextBox); 
                    yOffset += 40;
                   
                }
            }
            //-----------列印按鈕--------------
            Button pinrt = new Button();
            pinrt.Text = "列印";
            pinrt.Location = new System.Drawing.Point(300, yOffset);
            pinrt.Size = new System.Drawing.Size(150, 40);
            pinrt.Font = new System.Drawing.Font(pinrt.Font.FontFamily, 16);
            pinrt.Click += new EventHandler(Print_Click);
            this.Controls.Add(pinrt);
            scrollPanel.Controls.Add(pinrt);

            //-----------儲存到報表按鈕--------------
            Button save_execl = new Button();
            save_execl.Text = "儲存到報表";
            save_execl.Location = new System.Drawing.Point(300, yOffset+50);
            save_execl.Size = new System.Drawing.Size(150, 40);
            save_execl.Font = new System.Drawing.Font(pinrt.Font.FontFamily, 16);
            save_execl.Click += new EventHandler(SaveExecl_Click);
            this.Controls.Add(save_execl);
            scrollPanel.Controls.Add(save_execl);
        }
    }
}
