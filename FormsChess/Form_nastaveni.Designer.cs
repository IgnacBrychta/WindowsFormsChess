namespace FormsChess
{
    partial class Form_nastaveni
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_nastaveni));
            this.button_OK = new System.Windows.Forms.Button();
            this.textBox_slozkaTextur = new System.Windows.Forms.TextBox();
            this.button_zvolitTextury = new System.Windows.Forms.Button();
            this.checkedListBox_nacteneTextury_figurky = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton_atomic_ne = new System.Windows.Forms.RadioButton();
            this.radioButton_atomic_ano = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_casomira = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.button_AI_plus = new System.Windows.Forms.Button();
            this.button_AI_minus = new System.Windows.Forms.Button();
            this.textBox_inteligenceAI = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox_SerialPortName = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_OK.Location = new System.Drawing.Point(15, 1021);
            this.button_OK.Margin = new System.Windows.Forms.Padding(6);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(1534, 180);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // textBox_slozkaTextur
            // 
            this.textBox_slozkaTextur.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_slozkaTextur.Location = new System.Drawing.Point(46, 96);
            this.textBox_slozkaTextur.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_slozkaTextur.Name = "textBox_slozkaTextur";
            this.textBox_slozkaTextur.ReadOnly = true;
            this.textBox_slozkaTextur.Size = new System.Drawing.Size(852, 53);
            this.textBox_slozkaTextur.TabIndex = 1;
            this.textBox_slozkaTextur.Text = "<defaultní>";
            this.textBox_slozkaTextur.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_zvolitTextury
            // 
            this.button_zvolitTextury.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_zvolitTextury.Location = new System.Drawing.Point(46, 164);
            this.button_zvolitTextury.Margin = new System.Windows.Forms.Padding(6);
            this.button_zvolitTextury.Name = "button_zvolitTextury";
            this.button_zvolitTextury.Size = new System.Drawing.Size(856, 68);
            this.button_zvolitTextury.TabIndex = 2;
            this.button_zvolitTextury.Text = "Zvolit a načíst";
            this.button_zvolitTextury.UseVisualStyleBackColor = true;
            this.button_zvolitTextury.Click += new System.EventHandler(this.button_zvolitTextury_Click);
            // 
            // checkedListBox_nacteneTextury_figurky
            // 
            this.checkedListBox_nacteneTextury_figurky.Enabled = false;
            this.checkedListBox_nacteneTextury_figurky.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox_nacteneTextury_figurky.FormattingEnabled = true;
            this.checkedListBox_nacteneTextury_figurky.Items.AddRange(new object[] {
            "Pěšec",
            "Věž",
            "Kůň",
            "Střelec",
            "Dáma",
            "Král"});
            this.checkedListBox_nacteneTextury_figurky.Location = new System.Drawing.Point(112, 330);
            this.checkedListBox_nacteneTextury_figurky.Margin = new System.Windows.Forms.Padding(6);
            this.checkedListBox_nacteneTextury_figurky.Name = "checkedListBox_nacteneTextury_figurky";
            this.checkedListBox_nacteneTextury_figurky.Size = new System.Drawing.Size(228, 304);
            this.checkedListBox_nacteneTextury_figurky.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(68, 684);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 46);
            this.label1.TabIndex = 4;
            this.label1.Text = "ATOMIC MODE";
            // 
            // radioButton_atomic_ne
            // 
            this.radioButton_atomic_ne.AutoSize = true;
            this.radioButton_atomic_ne.Checked = true;
            this.radioButton_atomic_ne.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_atomic_ne.Location = new System.Drawing.Point(234, 748);
            this.radioButton_atomic_ne.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_atomic_ne.Name = "radioButton_atomic_ne";
            this.radioButton_atomic_ne.Size = new System.Drawing.Size(107, 50);
            this.radioButton_atomic_ne.TabIndex = 5;
            this.radioButton_atomic_ne.TabStop = true;
            this.radioButton_atomic_ne.Text = "NE";
            this.radioButton_atomic_ne.UseVisualStyleBackColor = true;
            this.radioButton_atomic_ne.Click += new System.EventHandler(this.radioButton_atomic_ne_Click);
            // 
            // radioButton_atomic_ano
            // 
            this.radioButton_atomic_ano.AutoSize = true;
            this.radioButton_atomic_ano.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_atomic_ano.Location = new System.Drawing.Point(74, 748);
            this.radioButton_atomic_ano.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_atomic_ano.Name = "radioButton_atomic_ano";
            this.radioButton_atomic_ano.Size = new System.Drawing.Size(138, 50);
            this.radioButton_atomic_ano.TabIndex = 6;
            this.radioButton_atomic_ano.Text = "ANO";
            this.radioButton_atomic_ano.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(354, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 46);
            this.label2.TabIndex = 8;
            this.label2.Text = "Složka textur:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(130, 278);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 46);
            this.label3.TabIndex = 9;
            this.label3.Text = "Načteno:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(548, 514);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(191, 46);
            this.label4.TabIndex = 10;
            this.label4.Text = "Časomíra";
            // 
            // comboBox_casomira
            // 
            this.comboBox_casomira.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_casomira.FormattingEnabled = true;
            this.comboBox_casomira.Items.AddRange(new object[] {
            "Fixní čas na tah",
            "Vzájemná časomíra"});
            this.comboBox_casomira.Location = new System.Drawing.Point(452, 570);
            this.comboBox_casomira.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox_casomira.Name = "comboBox_casomira";
            this.comboBox_casomira.Size = new System.Drawing.Size(420, 54);
            this.comboBox_casomira.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(526, 684);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(277, 46);
            this.label5.TabIndex = 12;
            this.label5.Text = "Prohodit barvy";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton1.Location = new System.Drawing.Point(12, 20);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(138, 50);
            this.radioButton1.TabIndex = 13;
            this.radioButton1.Text = "ANO";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.radioButton_prohoditBarvy_click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton2.Location = new System.Drawing.Point(172, 20);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(107, 50);
            this.radioButton2.TabIndex = 14;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "NE";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.radioButton_prohoditBarvy_click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(532, 744);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(284, 76);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Location = new System.Drawing.Point(470, 306);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(330, 110);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Checked = true;
            this.radioButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton4.Location = new System.Drawing.Point(174, 40);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(107, 50);
            this.radioButton4.TabIndex = 1;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "NE";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Click += new System.EventHandler(this.radioButton4_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton3.Location = new System.Drawing.Point(12, 40);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(138, 50);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.Text = "ANO";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Click += new System.EventHandler(this.radioButton4_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(456, 278);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(373, 46);
            this.label6.TabIndex = 17;
            this.label6.Text = "Odlišné barvy textur";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(958, 42);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(582, 46);
            this.label7.TabIndex = 20;
            this.label7.Text = "Soubor pro načtení šachovnice:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(968, 164);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(536, 68);
            this.button1.TabIndex = 19;
            this.button1.Text = "Zvolit a načíst";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_NacistFENnotaci);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(968, 96);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(532, 53);
            this.textBox1.TabIndex = 18;
            this.textBox1.Text = "<žádný, výchozí rozložení>";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1054, 684);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(359, 46);
            this.label8.TabIndex = 22;
            this.label8.Text = "Ignorovat časomíru";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton5);
            this.groupBox3.Controls.Add(this.radioButton6);
            this.groupBox3.Location = new System.Drawing.Point(1064, 712);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox3.Size = new System.Drawing.Size(330, 110);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Checked = true;
            this.radioButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton5.Location = new System.Drawing.Point(174, 40);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(107, 50);
            this.radioButton5.TabIndex = 1;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "NE";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.Click += new System.EventHandler(this.radioButton6_Click);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton6.Location = new System.Drawing.Point(12, 40);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(138, 50);
            this.radioButton6.TabIndex = 0;
            this.radioButton6.Text = "ANO";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.Click += new System.EventHandler(this.radioButton6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(988, 278);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(539, 46);
            this.label9.TabIndex = 23;
            this.label9.Text = "Inteligence přítele na telefonu";
            // 
            // button_AI_plus
            // 
            this.button_AI_plus.Location = new System.Drawing.Point(1390, 334);
            this.button_AI_plus.Margin = new System.Windows.Forms.Padding(6);
            this.button_AI_plus.Name = "button_AI_plus";
            this.button_AI_plus.Size = new System.Drawing.Size(60, 60);
            this.button_AI_plus.TabIndex = 24;
            this.button_AI_plus.Text = "+";
            this.button_AI_plus.UseVisualStyleBackColor = true;
            this.button_AI_plus.Click += new System.EventHandler(this.button_AI_plus_Click);
            // 
            // button_AI_minus
            // 
            this.button_AI_minus.Location = new System.Drawing.Point(1390, 406);
            this.button_AI_minus.Margin = new System.Windows.Forms.Padding(6);
            this.button_AI_minus.Name = "button_AI_minus";
            this.button_AI_minus.Size = new System.Drawing.Size(60, 60);
            this.button_AI_minus.TabIndex = 25;
            this.button_AI_minus.Text = "-";
            this.button_AI_minus.UseVisualStyleBackColor = true;
            this.button_AI_minus.Click += new System.EventHandler(this.button_AI_minus_Click);
            // 
            // textBox_inteligenceAI
            // 
            this.textBox_inteligenceAI.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox_inteligenceAI.Location = new System.Drawing.Point(1052, 366);
            this.textBox_inteligenceAI.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_inteligenceAI.Name = "textBox_inteligenceAI";
            this.textBox_inteligenceAI.ReadOnly = true;
            this.textBox_inteligenceAI.Size = new System.Drawing.Size(322, 68);
            this.textBox_inteligenceAI.TabIndex = 26;
            this.textBox_inteligenceAI.Text = "20/20";
            this.textBox_inteligenceAI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(1054, 514);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(334, 46);
            this.label10.TabIndex = 28;
            this.label10.Text = "Stockfish konzole";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton7);
            this.groupBox4.Controls.Add(this.radioButton8);
            this.groupBox4.Location = new System.Drawing.Point(1064, 542);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox4.Size = new System.Drawing.Size(330, 110);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Checked = true;
            this.radioButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton7.Location = new System.Drawing.Point(174, 40);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(107, 50);
            this.radioButton7.TabIndex = 1;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "NE";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.Click += new System.EventHandler(this.radioButton7_Click);
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton8.Location = new System.Drawing.Point(12, 40);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(138, 50);
            this.radioButton8.TabIndex = 0;
            this.radioButton8.Text = "ANO";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.Click += new System.EventHandler(this.radioButton7_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioButton9);
            this.groupBox5.Controls.Add(this.radioButton10);
            this.groupBox5.Location = new System.Drawing.Point(527, 902);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(284, 76);
            this.groupBox5.TabIndex = 30;
            this.groupBox5.TabStop = false;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Checked = true;
            this.radioButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton9.Location = new System.Drawing.Point(172, 20);
            this.radioButton9.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(107, 50);
            this.radioButton9.TabIndex = 14;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "NE";
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.Click += new System.EventHandler(this.radioButton9_Click);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton10.Location = new System.Drawing.Point(12, 20);
            this.radioButton10.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(138, 50);
            this.radioButton10.TabIndex = 13;
            this.radioButton10.Text = "ANO";
            this.radioButton10.UseVisualStyleBackColor = true;
            this.radioButton10.Click += new System.EventHandler(this.radioButton9_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(407, 852);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(576, 46);
            this.label11.TabIndex = 29;
            this.label11.Text = "Propojení s reálnou šachovnicí:";
            // 
            // comboBox_SerialPortName
            // 
            this.comboBox_SerialPortName.Enabled = false;
            this.comboBox_SerialPortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox_SerialPortName.FormattingEnabled = true;
            this.comboBox_SerialPortName.Location = new System.Drawing.Point(1039, 852);
            this.comboBox_SerialPortName.Name = "comboBox_SerialPortName";
            this.comboBox_SerialPortName.Size = new System.Drawing.Size(411, 54);
            this.comboBox_SerialPortName.TabIndex = 31;
            // 
            // Form_nastaveni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1582, 1354);
            this.Controls.Add(this.comboBox_SerialPortName);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.textBox_inteligenceAI);
            this.Controls.Add(this.button_AI_minus);
            this.Controls.Add(this.button_AI_plus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox_casomira);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioButton_atomic_ano);
            this.Controls.Add(this.radioButton_atomic_ne);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBox_nacteneTextury_figurky);
            this.Controls.Add(this.button_zvolitTextury);
            this.Controls.Add(this.textBox_slozkaTextur);
            this.Controls.Add(this.button_OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form_nastaveni";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "  Nastavení";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_nastaveni_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.TextBox textBox_slozkaTextur;
        private System.Windows.Forms.Button button_zvolitTextury;
        private System.Windows.Forms.CheckedListBox checkedListBox_nacteneTextury_figurky;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton_atomic_ne;
        private System.Windows.Forms.RadioButton radioButton_atomic_ano;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox comboBox_casomira;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_AI_plus;
        private System.Windows.Forms.Button button_AI_minus;
        private System.Windows.Forms.TextBox textBox_inteligenceAI;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox comboBox_SerialPortName;
    }
}