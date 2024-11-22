using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace finite_state_machine
{

    enum State 
    {
        Q0,
        Q1,
        Q2,
        Q3,
      
    }

    enum ArrowType
    {
        UP,
        DOWN,
        RIGHT,
        LEFT

    }

    struct StateInfo
    {
        public Label valueLabel1;
        public Label valueLabel0;
        public PictureBox arrow1;
        public PictureBox arrow0;
        public string value;
        public PictureBox statePictureBox;


    }

    public partial class Form1 : Form
    {
        State currentState = State.Q3;
         State succesState = State.Q2;
        State startingState = State.Q3;
        System.Windows.Forms.Timer timer;
        Dictionary<State, StateInfo> stateInfos;
        int startCounter = 0;
        int count = 1;





        string currentInput = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label_q0q1.Hide();
            label_q0q2.Hide();
            label_q1q3.Hide();
            label_q3q2.Hide();
            timer = new System.Windows.Forms.Timer() { Interval = 1500 };
            timer.Tick += Tick;


            stateInfos = new Dictionary<State, StateInfo>
            {
                {State.Q0,new StateInfo(){statePictureBox = state0_pictureBox, arrow0 = arrow_02, arrow1 = arrow_01 , valueLabel0 = label_q0q2, valueLabel1 = label_q0q1} },
                {State.Q1,new StateInfo(){statePictureBox = state1_pictureBox, arrow0 = arrow_13, arrow1 = arrow_10 , valueLabel0 = label_q1q3, valueLabel1 = label_q0q1} },
                {State.Q2,new StateInfo(){statePictureBox = state2_pictureBox, arrow0 = arrow_20, arrow1 = arrow_23 , valueLabel0 = label_q0q2, valueLabel1 = label_q3q2} },
                {State.Q3,new StateInfo(){statePictureBox = state3_pictureBox, arrow0 = arrow_31, arrow1 = arrow_32 , valueLabel0 = label_q1q3, valueLabel1 = label_q3q2} },

            };
        }

        

        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            CheckText();
        }



        void CheckText()
        {
            string text = InputBox.Text;
            foreach(char letter in text)
            {
            
                if(letter.ToString() != "0" && letter.ToString() != "1")
                {

                    InputBox.Text = "";
                    break;
                }
               
            }
            currentInput = InputBox.Text;


           
        }

       

        private void RunButton_Click(object sender, EventArgs e)
        {
            Run();
        }


        void Run()
        {
            if (InputBox.Text.Length == 0) return;

            ResetStatePictureBox();
            InputBox.ReadOnly = true;
            InputBox.BackColor = Color.LightGray;

            RunButton.Enabled = false;
            RunButton.BackColor = Color.LightGray;

            label_q0q1.Hide();
            label_q0q2.Hide();
            label_q1q3.Hide();
            label_q3q2.Hide();
            declined_label.Hide();
            accepted_label.Hide();

            timer.Start();
     

        }

      
        void Tick(object sender, EventArgs e)
        {
            
            if (startCounter == 0)
            {
                SetCurrentValue(label_start, currentInput[0].ToString());
            }
            else if (startCounter == 1) SetCurrentArrow(start_arrow,ArrowType.LEFT);

            else if(startCounter == 2) SetCurrentStatePictureBox();

            if (startCounter < 3) {
                startCounter++;
                    return;
            };



            SetCurrentState();
            Debug.WriteLine(currentState.ToString());
            SetCurrentStatePictureBox();

           

            if (count == currentInput.Length)
            {
                timer.Stop();
                ResetArrows();
                ResetValues();

                if (currentState == succesState)
                {
                    accepted_label.Text = $"Łańcuch zaakceptowany \n Stan końcowy: {currentState.ToString()}";
                    accepted_label.Show();
                   
                }
                else
                {
                    declined_label.Text = $"Łańcuch odrzucony \n Stan końcowy: {currentState.ToString()}";
                    declined_label.Show();
                }

                UnlockControls();
            }
            else
            {
                count++;
            }

            
        }

        void UnlockControls()
        {

            InputBox.BackColor = Color.White;
            RunButton.BackColor = Color.White;
            currentState = startingState;
            RunButton.Enabled = true;
            InputBox.Text = "";
            currentInput = "";
            InputBox.ReadOnly = false;
            InputBox.Enabled = true;
            startCounter = 0;
            count = 1;


        }
        

        void ResetArrows()
        {
            arrow_23.Image = Properties.Resources.arrow_right;
            arrow_01.Image = Properties.Resources.arrow_right;

            arrow_32.Image = Properties.Resources.arrow_left;
            arrow_10.Image = Properties.Resources.arrow_left;
            start_arrow.Image = Properties.Resources.arrow_left;

            arrow_02.Image = Properties.Resources.arrow_up;
            arrow_13.Image = Properties.Resources.arrow_up;

            arrow_20.Image = Properties.Resources.arrow_down;
            arrow_31.Image = Properties.Resources.arrow_down;
        }

         void SetCurrentArrow(PictureBox pictureBox, ArrowType arrowType)
        {

            ResetArrows();

            switch (arrowType)
            {
                case ArrowType.UP:
                    using(var ms = new MemoryStream(Properties.Resources.arrow_up_active))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                    break;
                case ArrowType.DOWN:
                    using (var ms = new MemoryStream(Properties.Resources.arrow_down_active))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                    break;
                case ArrowType.RIGHT:
                    using (var ms = new MemoryStream(Properties.Resources.arrow_right_active))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                    break;
                case ArrowType.LEFT:
                    using (var ms = new MemoryStream(Properties.Resources.arrow_left_active))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                    break;
                default:
                    break;
            }


        }
         void SetCurrentValue(Label label, string value)
        {
            ResetValues();
            label.Text = value;
            label.Show();
        }


        void ResetValues()
        {
            label_q0q1.Hide();
            label_q0q2.Hide();
            label_q1q3.Hide();
            label_q3q2.Hide();
        }


        void ResetStatePictureBox()
        {
            state0_pictureBox.Image = Properties.Resources.state;
            state1_pictureBox.Image = Properties.Resources.state;
            state2_pictureBox.Image = Properties.Resources.state;
            state3_pictureBox.Image = Properties.Resources.state;
        }
         void SetCurrentStatePictureBox()
        {
            ResetStatePictureBox();



            switch (currentState)
            {
                case State.Q0:
                    
                    state0_pictureBox.Image = Properties.Resources.state_active;
                    break;
                case State.Q1:
                    state1_pictureBox.Image = Properties.Resources.state_active;
                    break;
                case State.Q2:
                    state2_pictureBox.Image = Properties.Resources.state_active;
                    break;
                case State.Q3:
                    state3_pictureBox.Image = Properties.Resources.state_active;
                    break;
                default:
                    break;
            }
           
           

        }



        void SetCurrentState()
        {if (count >= currentInput.Length) return;

            if (currentInput[count].ToString() == "0")
            {
                switch (currentState)
                {
                    case State.Q0:

                        SetCurrentArrow(stateInfos[currentState].arrow0, ArrowType.UP);
                        SetCurrentValue(stateInfos[currentState].valueLabel0, "0");
                        currentState = State.Q2;

                        break;
                    case State.Q1:

                        SetCurrentArrow(stateInfos[currentState].arrow0, ArrowType.UP);
                        SetCurrentValue(stateInfos[currentState].valueLabel0, "0");
                        currentState = State.Q3;
                        break;
                    case State.Q2:

                        SetCurrentArrow(stateInfos[currentState].arrow0, ArrowType.DOWN);
                        SetCurrentValue(stateInfos[currentState].valueLabel0, "0");
                        currentState = State.Q0;
                        break;
                    case State.Q3:
                        SetCurrentArrow(stateInfos[currentState].arrow0, ArrowType.DOWN);
                        SetCurrentValue(stateInfos[currentState].valueLabel0, "0");
                        currentState = State.Q1;
                
                        break;
                    default:
                        break;
                }
            }
            else if (currentInput[count].ToString() == "1")
            {
                switch (currentState)
                {
                    case State.Q0:
                        SetCurrentArrow(stateInfos[currentState].arrow1, ArrowType.RIGHT);
                        SetCurrentValue(stateInfos[currentState].valueLabel1, "1");
                        currentState = State.Q1;
                        
                        break;
                    case State.Q1:
                        SetCurrentArrow(stateInfos[currentState].arrow1, ArrowType.LEFT);
                        SetCurrentValue(stateInfos[currentState].valueLabel1, "1");
                        currentState = State.Q0;
            
                        break;
                    case State.Q2:
                        SetCurrentArrow(stateInfos[currentState].arrow1, ArrowType.RIGHT);
                        SetCurrentValue(stateInfos[currentState].valueLabel1, "1");
                        currentState = State.Q3;
                        
                        break;
                    case State.Q3:
                        SetCurrentArrow(stateInfos[currentState].arrow1, ArrowType.LEFT);
                        SetCurrentValue(stateInfos[currentState].valueLabel1, "1");
                        currentState = State.Q2;
                     
                        break;
                    default:
                        break;
                }
            }
        }

        
    }

    
}
