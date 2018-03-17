using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MegaChallengeCasino
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Set viewState
                double money=100;
                ViewState.Add("Money", money);

                //display initial money
                moneyLabel.Text = String.Format("{0:C}", money);

                //Get 3 random image names
                string imageName1 = getImageName();
                string imageName2 = getImageName();
                string imageName3 = getImageName();

                //get 3 images
                getImage1(imageName1);
                getImage2(imageName2);
                getImage3(imageName3);
            }
        }

        protected void pullLeverButton_Click(object sender, EventArgs e)
        {
            int reward = 0;
            double bet = 0;

            //Get money data from ViewState
            double money = (double)ViewState["Money"];

            //Get Bet
            bet = getBet();
            if (bet == 0 || money < bet) return;

            //Get 3 random image names
            string imageName1 = getImageName();
            string imageName2 = getImageName();
            string imageName3 = getImageName();
            
            //get 3 images
            getImage1(imageName1);
            getImage2(imageName2);
            getImage3(imageName3);
            

            // Check the reward
            //Cherry
            reward = checkRewardCherry(countCherry(imageName1, imageName2, imageName3))
                +checkRewardJackpot(imageName1, imageName2, imageName3);

            // Calculate how much won
            double win = winCalculation(bet, checkRewardBar(imageName1, imageName2, imageName3), reward);
            
            // display result label
            displayResult(bet, win);

            // Calculate money afte game
            money = moneyCalculation(money, bet, win);

            // Display money after game
            moneyLabel.Text = String.Format("{0:C}", money);

            // Store amount of money after game
            ViewState["Money"] = money; 
            
        }

        private double getBet()
        {
            double bet = 0;

            if (!double.TryParse(betTextBox.Text.Trim(), out bet))
                return 0;
            else
                return bet;
        }

        Random random = new Random();
        private string getImageName()
        {
            string[] images = new string[] 
            {"Strawberry","Bar","Lemon","Bell","Clover","Cherry",
                "Diamond","Orange","Seven","HorseShoe","Plum","Watermelon"};

            return images[random.Next(11)];
        }

        private void getImage1(string imageName)
        {
            Image1.ImageUrl = String.Format("/Images/{0}.png", imageName);
        }

        private void getImage2(string imageName)
        {
            Image2.ImageUrl = String.Format("/Images/{0}.png", imageName);
        }
        private void getImage3(string imageName)
        {
            Image3.ImageUrl = String.Format("/Images/{0}.png", imageName);
        }

        private int countCherry(string image1, string image2, string image3)
        {
            int cherryCount = 0;
            if (image1 == "Cherry") cherryCount += 1;
            if (image2 == "Cherry") cherryCount += 1;
            if (image3 == "Cherry") cherryCount += 1;

            return cherryCount;
        }

        private int checkRewardCherry(int cherryCount)
        {
            int reward = 0;
            if (cherryCount == 1) reward = 2;
            if (cherryCount == 2) reward = 3;
            if (cherryCount == 3) reward = 4;

            return reward;
        }

        private int checkRewardJackpot(string image1, string image2, string image3)
        {
            int reward = 0;
            if (image1 == "Seven" && image2 == "Seven" && image3 == "Seven") reward = 100;

            return reward;
        }

        //Return "true" if there are ever one bar 
        private bool checkRewardBar(string image1, string image2, string image3)
        {
            if (image1 == "Bar" || image2 == "Bar" || image3 == "Bar") return true;
            else return false;
        }

        // Money calculation
        private double moneyCalculation(double money, double bet, double win)
        {
            double cash = money;
            cash = cash - bet + win;
            
            return cash;
        }

        //Win calculation
        private double winCalculation(double bet, bool checkRewardBar, int reward)
        {
            double win = 0;
            if (checkRewardBar == true) return win;  // If there is a bar, win nothing -> win=0
            else win = bet * reward;

            return win;
        }

        private void displayResult(double bet, double win)
        {
            if (win == 0)
                resultLabel.Text = String.Format("Sorry, you lost {0:C}. Better luck next time.", bet);
            else
                resultLabel.Text = String.Format("You Bet {0:C} and won {1:C}!", bet, win);
        }

    }


    
}