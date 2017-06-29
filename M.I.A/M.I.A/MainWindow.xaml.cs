using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace M.I.A
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();


        SpeechSynthesizer s = new SpeechSynthesizer();
        Choices list = new Choices();
        Boolean mode = true;
        public Boolean search = false;
        public Boolean lookfor = false;


        public MainWindow()
        {
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine();


            list.Add(File.ReadAllLines(@"C:\Users\502707467\Desktop\Secret\Mia\dictionary.txt"));


            Grammar gr = new Grammar(new GrammarBuilder(list));

            InitializeComponent();
            // rec = SpeechRecognitionEngine

            try
            {
                

                rec.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(engine_AudioLevelUpdated);
                rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(rec_SpeachRecognized);

                rec.RequestRecognizerUpdate();

                // loads dictionary
                rec.LoadGrammar(gr);

                //giving it a name

                // Systems Default audio device
                rec.SetInputToDefaultAudioDevice();

                // Listening
                rec.RecognizeAsync(RecognizeMode.Multiple);
                s.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(s_SpeakCompleted);

                if (s.State == SynthesizerState.Speaking)
                    s.SpeakAsyncCancelAll();
            }
            catch { return; }


            s.SelectVoiceByHints(VoiceGender.Female);

            s.Speak("Hi, my name is Mia"); 

            
        }

        private void s_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {

        }

        

        private void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            progress.Value = e.AudioLevel;
            
        }

        public void say(String h)
        {


            //s.Speak(h);
            s.SpeakAsync(h);
            textBox2.AppendText(h + "\n");

        }

        private void rec_SpeachRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String r = e.Result.Text;


            if (r == "wake up")
            {
                mode = true;
                label2.DataContext = "State: Awake";
                say("im up");
            }

            
            if (r == "sleep" || r == "go to sleep")
            {
                mode = false;
                label2.DataContext = "State: Sleep";
                say("bye");
                
                
            }

            if (search)

            {

                Process.Start("https://www.google.com/#q=" + r);

                search = false;
            }

            if (lookfor)

            {

                IWebDriver driver = new ChromeDriver("C:\\Users\\502707467\\Desktop\\driver");

                driver.Url = "https://ge.service-now.com/mytech/";

                IWebElement user = driver.FindElement(By.Id("username"));
                user.SendKeys("502707467");

                IWebElement pass = driver.FindElement(By.Id("password"));
                pass.SendKeys("D2317867y");

                IWebElement enter = driver.FindElement(By.Id("submitFrm"));

                enter.Click();

                IWebElement yourself = driver.FindElement(By.Id("shopnewCartUser"));

                yourself.Click();

                IWebElement request = driver.FindElement(By.Id("requestLink"));

                request.Click();

                IWebElement windowsS = driver.FindElement(By.LinkText("Windows Software"));

                windowsS.Click();

                IWebElement search = driver.FindElement(By.Id("searchTextBox"));

                search.SendKeys(r);

                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                IWebElement go = driver.FindElement(By.ClassName("btn btn-primary"));

                
                go.Click();


                lookfor = false;
            }




            if (mode == true && search == false)
            {

                if (r == "search for")
                {
                    search = true;

                }


                if (r == "look for")
                {
                    lookfor = true;

                }

                if (r == "hello")
                {
                    say("hey");
                }

                if (r == "how are you")
                {

                    say("good. and you");

                }

                if (r == "im good")
                {

                    say("thats good to hear");

                }

                if (r == "what is my tech")
                {
                    say("something wonderful");
                }

                if (r == "what time is it")
                {

                    say(DateTime.Now.ToString("h:mm tt"));

                }

                if (r == "whats todays date")
                {

                    say(DateTime.Now.ToString("M/d/yyyy"));

                }

                if (r == "open my tech")
                {

                    say("sure, opening my tech");
                    Process.Start("https://ge.service-now.com/mytech/");

                }

                if (r == "open the my tech guide" || r == "open my tech guide")
                {

                    say("sure, opening the my tech guide");
                    Process.Start("https://ge.service-now.com/kb_view.do?sysparm_article=KB1467713");

                }

                if (r == "i want to see my technology")
                {

                    say("okay, showing you your technology");
                    Process.Start("https://ge.service-now.com/mytech/mytechnology.do");

                

                }

            }
            textBox1.AppendText(r + "\n");
        }
    }
}
