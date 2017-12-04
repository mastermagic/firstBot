using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Threading;

namespace FirstBot
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;
        private static int humanDelay = 2000;
        private static bool stopProcessing = false;
        private static int actionNumber = 1;

        public static bool IsExists(IWebDriver driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static string FirstUpper(string str)
        {
            string[] s = str.Split(' ');

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Length > 1)
                    s[i] = s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
                else s[i] = s[i].ToUpper();
            }
            return string.Join(" ", s);
        }

        public static void HumanDelay(bool longDelay = false)
        {
            Random rnd = new Random();
            if (longDelay)
            {
                Thread.Sleep(humanDelay + rnd.Next(1200, 3200));
            }
            Thread.Sleep(humanDelay + rnd.Next(-200, 1200));
        }

        public void UserRandomAction()
        {
            if (actionNumber > 4)
            {
                Random rnd = new Random();
                int act;
                if ((act = rnd.Next(1, 6)) % 2 == 1)
                {
                    // Пытаемся пойти наверх
                    try
                    {
                        IWebElement upButton = Browser.FindElement(By.XPath("//nobr[@id = 'stl_text']"));
                        upButton.Click();
                    }
                    catch (Exception)
                    {
                    }
                    Browser.Navigate().Refresh();
                    switch (act)
                    {
                        case 1:
                            try
                            {                                
                                IWebElement groupsItem = Browser.FindElement(By.XPath("//a[@href = '/feed']"));
                                groupsItem.Click();
                            }
                            catch (Exception)
                            {
                            }
                            break;
                        case 2:
                            try
                            {
                                IWebElement groupsItem = Browser.FindElement(By.XPath("//a[@href = '/im']"));
                                groupsItem.Click();
                            }
                            catch (Exception)
                            {
                            }
                            break;
                        case 3:
                            try
                            {
                                IWebElement groupsItem = Browser.FindElement(By.XPath("//a[@href = '/friends']"));
                                groupsItem.Click();
                            }
                            catch (Exception)
                            {
                            }
                            break;
                    }
                    HumanDelay(true);
                    actionNumber = 1;
                }                
            }            
        }

        public Form1()
        {
            InitializeComponent();
            string path1 = @"keywords.kwd";
            string path2 = @"pkeywords.kwd";
            string path3 = @"kstopwords.swd";
            string path4 = @"pstopwords.swd";
            
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            try
            {
                using (StreamReader sr = new StreamReader(path1, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        textBox1.AppendText(line + "\r\n");
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        textBox2.AppendText(line + "\r\n");
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                using (StreamReader sr = new StreamReader(path3, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        textBox3.AppendText(line + "\r\n");
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                using (StreamReader sr = new StreamReader(path4, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        textBox4.AppendText(line + "\r\n");
                    }
                }
            }
            catch (Exception)
            {
            }

            OpenQA.Selenium.Chrome.ChromeOptions co = new OpenQA.Selenium.Chrome.ChromeOptions();

            //co.AddArgument(@"user-data-dir=c:\\Users\\husainovdr\\AppData\\Local\\Google\\Chrome\\User Data\");
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver(/*co*/);

            Browser.Navigate().GoToUrl("http://vk.com");
            //Browser.Manage().Window.Maximize();
        }

        // Залогиниться
        private void button1_Click(object sender, EventArgs e)
        {
            string login;
            string pass;
            try
            {
                string accDatapath = @"acc.data";
                using (StreamReader sr = new StreamReader(accDatapath, System.Text.Encoding.Default))
                {                    
                    login = sr.ReadLine();
                    pass = sr.ReadLine();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения файла данных об аккаунте");
                return;
            }
            
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            
            if (IsExists(Browser, By.XPath("//div[@class='top_profile_name']")))
            {
                MessageBox.Show("Уже залогинен", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                IWebElement emailInput = ww.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[id='index_email']")));
                emailInput.SendKeys(login);
                HumanDelay();
                IWebElement passInput = ww.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[id='index_pass']")));
                passInput.SendKeys(pass + OpenQA.Selenium.Keys.Enter);
            }
        }

        // Закрыть
        private void button2_Click(object sender, EventArgs e)
        {
            Browser.Quit();
            Close();
        }        

        /////////////////////
        // ПОСТ ОБЪЯВЛЕНИЙ //
        /////////////////////
        private void button1_Click_1(object sender, EventArgs e)
        {
            bool stop = false;
            HumanDelay();
            // Пытаемся пойти наверх
            try
            {
                IWebElement upButton = Browser.FindElement(By.XPath("//nobr[@id = 'stl_text']"));
            }
            catch (Exception)
            {
            }
            // Заходим в группы
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            try
            {
                IWebElement groupsItem = ww.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a[href = '/groups']")));
                groupsItem.Click();
                if (stopProcessing)
                {
                    return;
                }
                HumanDelay();
                // Находим нужную группу
                IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Пост объявлений Управа")));
                groupRow.Click();
                if (stopProcessing)
                {
                    return;
                }
                HumanDelay();
            }
            catch (Exception)
            {
            }
            // Ищем предлагаемые
            try
            {
                IWebElement offeredTab = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Предлагаемые")));
                offeredTab.Click();
            }
            catch (NoSuchElementException)
            {
                MessageBox.Show("Нет предлагаемых новостей", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                stop = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Нет предлагаемых новостей", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                stop = true;
            }
            if (!stop)
            {
                while (IsExists(Browser, By.PartialLinkText("Предлагаемые")) || !IsExists(Browser, By.PartialLinkText("Разрешается размещать не более")))
                {
                    if (stopProcessing)
                    {
                        return;
                    }
                    // Постим
                    try
                    {
                        actionNumber++;
                        HumanDelay();
                        IWebElement prepareButton = ww.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button[contains(text(),'Подготовить к публикации')])[last()]")));

                        prepareButton.Click();
                        HumanDelay();
                        IWebElement publicButton = ww.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@id='wpe_save'][1]")));

                        publicButton.Click();
                        UserRandomAction();
                        if (actionNumber == 1)
                        {
                            try
                            {
                                IWebElement groupsItem = ww.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a[href = '/groups']")));
                                groupsItem.Click();
                                if (stopProcessing)
                                {
                                    return;
                                }
                                HumanDelay();
                                // Находим нужную группу
                                IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Пост объявлений Управа")));
                                groupRow.Click();
                                if (stopProcessing)
                                {
                                    return;
                                }
                                HumanDelay();
                            }
                            catch (Exception)
                            {
                            }
                            // Ищем предлагаемые
                            try
                            {
                                IWebElement offeredTab = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Предлагаемые")));
                                offeredTab.Click();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    if (!IsExists(Browser, By.PartialLinkText("Предлагаемые")) || IsExists(Browser, By.PartialLinkText("Разрешается размещать не более")))
                    {
                        break;
                    }
                }
            }
            HumanDelay();
        }
        /////////////////////
        /////////////////////

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //////////////
        // КВАРТИРЫ //
        //////////////
        private void button2_Click_1(object sender, EventArgs e)
        {
            string path = @"keywords.kwd";
            string path2 = @"kstopwords.swd";
            // Загружаем ключевые слова в файл
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.Write(textBox1.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки в файл КС", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            string condition = "";
            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    int i = 1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 1)
                        {
                            condition = "contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        else
                        {
                            condition += " or contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения файла КС", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            // Загружаем стоп слова в файл
            try
            {
                using (StreamWriter sw = new StreamWriter(path2, false, System.Text.Encoding.Default))
                {
                    sw.Write(textBox3.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки в файл стоп слов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            string condition2 = "";
            try
            {
                using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                {
                    string line;
                    int i = 1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 1)
                        {
                            condition2 = "contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        else
                        {
                            condition2 += " or contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения файла стоп слов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            bool stop = false;
            // Пытаемся пойти наверх
            try
            {
                IWebElement upButton = Browser.FindElement(By.XPath("//nobr[@id = 'stl_text']"));
                upButton.Click();
            }
            catch (Exception)
            {
            }
            HumanDelay();
            // Заходим в группы
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            try
            {                
                IWebElement groupsItem = Browser.FindElement(By.CssSelector("a[href = '/groups']"));
                groupsItem.Click();
                if (stopProcessing)
                {
                    return;
                }
                HumanDelay();
                // Находим нужную группу
                IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Квартира Управа")));
                //IWebElement groupRow = Browser.FindElement(By.PartialLinkText("Пост объявлений Управа"));

                groupRow.Click();
            }
            catch (Exception)
            {
                try
                {
                    Browser.Navigate().Refresh();
                    IWebElement groupsItem = Browser.FindElement(By.XPath("//a[@href = '/groups']"));
                    groupsItem.Click();
                    if (stopProcessing)
                    {
                        return;
                    }
                    HumanDelay();
                    // Находим нужную группу
                    IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Квартира Управа")));
                    //IWebElement groupRow = Browser.FindElement(By.XPath("//a[@class = 'group_row_title' and contains(text(), 'Пост объявлений Управа')]"));

                    groupRow.Click();
                }
                catch (Exception)
                {
                    stop = true;
                }
            }
            if (stopProcessing)
            {
                return;
            }
            HumanDelay();
            
            // Ищем предлагаемые
            try
            {
                //IWebElement offeredTab = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Предлагаемые")));
                IWebElement offeredTab = Browser.FindElement(By.PartialLinkText("Предлагаемые"));
                offeredTab.Click();
            }
            catch (NoSuchElementException)
            {
                MessageBox.Show("Нет предлагаемых новостей", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                stop = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Нет предлагаемых новостей", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                stop = true;
            }
            if (!stop)
            {

                while (IsExists(Browser, By.PartialLinkText("Предлагаемые")) || !IsExists(Browser, By.PartialLinkText("Разрешается размещать не более")))
                {

                    if (stopProcessing)
                    {
                        return;
                    }
                    // Постим                    
                    try
                    {
                        actionNumber++;
                        HumanDelay();
                    //   List<IWebElement> postsInOffer = new List<IWebElement>(Browser.FindElements(By.XPath("//div[@class='wall_text' and (" + condition + ")]")));
                    //IWebElement postsInOffer = Browser.FindElement(By.XPath("(//div[@class='wall_text' and (" + condition + ")])[last()]"));
                    /*
                        if (postsInOffer.Count > 0)
                        {*/
                    //IWebElement postRow = postsInOffer.FindElement(By.XPath("(//ancestor::div[@class = '_post_content'])"));
                    //IWebElement postRow = Browser.FindElement(By.XPath("(//div[@class='_post_content' and (" + condition + ")])[last()]"));

                        IWebElement prepareButton = Browser.FindElement(By.XPath("(//div[@class='wall_text' and (" + condition + ") and not(" + condition2 + ") ]//ancestor::div[@class = '_post_content']//button[contains(text(),'Подготовить к публикации')])[last()]"));
                        prepareButton.Click();

                        HumanDelay();
                        IWebElement publicButton = ww.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@id='wpe_save']")));
                        publicButton.Click();
                        // }

                        // Выполняем какое-нибудь пользовательское действие, если уже пора
                        UserRandomAction();
                        if (actionNumber == 1)
                        {
                            try
                            {
                                // Заходим в группы
                                IWebElement groupsItem = ww.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a[href = '/groups']")));
                                groupsItem.Click();
                                if (stopProcessing)
                                {
                                    return;
                                }
                                HumanDelay();
                                // Находим нужную группу
                                IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Квартира Управа")));
                                groupRow.Click();
                                if (stopProcessing)
                                {
                                    return;
                                }
                                HumanDelay();
                            }
                            catch (Exception)
                            {
                            }
                            // Ищем предлагаемые
                            try
                            {
                                IWebElement offeredTab = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Предлагаемые")));
                                offeredTab.Click();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    //catch (NoSuchElementException)
                    //{                        
                    //}
                    //catch (ElementNotVisibleException)
                    //{
                    //}
                    catch (Exception)
                    {
                        MessageBox.Show("Нет предлагаемых новостей по ключевым словам", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    }
                    if (!IsExists(Browser, By.PartialLinkText("Предлагаемые")) || IsExists(Browser, By.PartialLinkText("Разрешается размещать не более")))
                    {
                        break;
                    }
                }

            }
            HumanDelay();
        }
        //////////////
        //////////////

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                humanDelay = 1000 * Convert.ToInt32(delayTextBox.Text);
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            stopProcessing = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stopProcessing = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Browser.Quit();
        }

        ////////////////
        // ПОДСЛУШАНО //
        ////////////////
        private void button6_Click(object sender, EventArgs e)
        {
            string path = @"pkeywords.kwd";
            string path2 = @"pstopwords.swd";
            // Загружаем ключевые слова в файл
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.Write(textBox2.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки в файл КС", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            string condition = "";
            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    int i = 1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 1)
                        {
                            condition = "contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        else
                        {
                            condition += " or contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения файла КС", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            // Загружаем ключевые слова в файл
            try
            {
                using (StreamWriter sw = new StreamWriter(path2, false, System.Text.Encoding.Default))
                {
                    sw.Write(textBox4.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки в файл стоп слов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            string condition2 = "";
            try
            {
                using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                {
                    string line;
                    int i = 1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 1)
                        {
                            condition2 = "contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        else
                        {
                            condition2 += " or contains(node(), '" + line + "') or contains(node(),'" + FirstUpper(line) + "')";
                        }
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения файла стоп слов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            bool stop = false;
            // Пытаемся пойти наверх
            try
            {
                IWebElement upButton = Browser.FindElement(By.XPath("//nobr[@id = 'stl_text']"));
                upButton.Click();
            }
            catch (Exception)
            {
            }
            HumanDelay();
            // Заходим в группы
            WebDriverWait ww = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            try
            {
                IWebElement groupsItem = Browser.FindElement(By.CssSelector("a[href = '/groups']"));
                groupsItem.Click();
                if (stopProcessing)
                {
                    return;
                }
                HumanDelay();
                // Находим нужную группу
                IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Подслушано Управа")));

                groupRow.Click();
            }
            catch (Exception)
            {
                try
                {
                    Browser.Navigate().Refresh();
                    IWebElement groupsItem = Browser.FindElement(By.XPath("//a[@href = '/groups']"));
                    groupsItem.Click();
                    if (stopProcessing)
                    {
                        return;
                    }
                    HumanDelay();
                    // Находим нужную группу
                    IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Подслушано Управа")));

                    groupRow.Click();
                }
                catch (Exception)
                {
                    stop = true;
                }
            }
            if (stopProcessing)
            {
                return;
            }
            HumanDelay();

            // Ищем предлагаемые
            try
            {
                IWebElement offeredTab = Browser.FindElement(By.PartialLinkText("Предлагаемые"));
                offeredTab.Click();
            }
            catch (NoSuchElementException)
            {
                MessageBox.Show("Нет предлагаемых новостей", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                stop = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Нет предлагаемых новостей", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                stop = true;
            }
            if (!stop)
            {

                while (IsExists(Browser, By.PartialLinkText("Предлагаемые")))
                {

                    if (stopProcessing)
                    {
                        return;
                    }
                    // Постим                    
                    try
                    {
                        actionNumber++;
                        HumanDelay();

                        IWebElement prepareButton = Browser.FindElement(By.XPath("(//div[@class='wall_text' and (" + condition + ") and not(" + condition2 + ")]//ancestor::div[@class = '_post_content']//button[contains(text(),'Подготовить к публикации')])[last()]"));
                        prepareButton.Click();

                        HumanDelay();
                        IWebElement publicButton = ww.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@id='wpe_save']")));
                        publicButton.Click();

                        // Выполняем какое-нибудь пользовательское действие, если уже пора
                        UserRandomAction();
                        if (actionNumber == 1)
                        {
                            try
                            {
                                IWebElement groupsItem = ww.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a[href = '/groups']")));
                                groupsItem.Click();
                                if (stopProcessing)
                                {
                                    return;
                                }
                                HumanDelay();
                                // Находим нужную группу
                                IWebElement groupRow = ww.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Подслушано Управа")));
                                groupRow.Click();
                                if (stopProcessing)
                                {
                                    return;
                                }
                                HumanDelay();
                            }
                            catch (Exception)
                            {
                            }
                            // Ищем предлагаемые
                            try
                            {
                                IWebElement offeredTab = ww.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Предлагаемые")));
                                offeredTab.Click();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Нет предлагаемых новостей по ключевым словам", "Нет новостей", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    }
                    if (!IsExists(Browser, By.PartialLinkText("Предлагаемые")) || IsExists(Browser, By.PartialLinkText("Разрешается размещать не более")))
                    {
                        break;
                    }
                }
            }
            HumanDelay();
        }
        ////////////////
        ////////////////

        private void button5_Click(object sender, EventArgs e)
        {
            stopProcessing = true;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
