/*********************************************************************************************
 * Joe's Automotive
 * Grant Andrews
 * 7/9/2022
 * This program allows a user to select the services that were performed and manually
 * enter a parts charge and a labor charge. The program calculates the total service
 * and labor charge, the total parts charge, the total tax on parts, and the overall total
 * charge and displays this to the user.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Joe_s_Automotive
{
    public partial class joesAutomotive : Form
    {
        public joesAutomotive()
        {
            InitializeComponent();
        }

            // Declare and assign constants at the field level
            private const decimal OIL_CHANGE = 26;
            private const decimal LUBE_JOB = 18;
            private const decimal RADIATOR_FLUSH = 30;
            private const decimal TRANSMISSION_FLUSH = 80;
            private const decimal INSPECTION_CHARGE = 15;
            private const decimal MUFFLER_REPLACEMENT = 100;
            private const decimal TIRE_ROTATION = 20;
            const decimal TAX = .06M;


        private void calculateButton_Click(object sender, EventArgs e)

            // This event handler will call on methods to calculate charges and
            // then to display this to the user when the "Calculate" button is clicked
        {
            // Declare and initalize variables
            decimal totalFlushCharges = 0;
            decimal totalMiscCharges = 0;
            decimal totalPartsCharges = 0;
            decimal totalLaborCharges = 0;
            decimal taxOnParts = 0;
            decimal totalOilLubeCharges = 0;
            decimal servicesAndLaborTotal;
            decimal totalCharges;


            // Call on methods only when necessary, to calculate charges. All methods are "out" methods
            if (oilChangeCheckBox.Checked || lubeJobCheckBox.Checked)
                OilLubeCharges(out totalOilLubeCharges);

            if (radiatorFlushCheckBox.Checked || transmissionFlushCheckBox.Checked)
                FlushCharges(out totalFlushCharges);
            
            if (inspectionCheckBox.Checked || replaceMufflerCheckBox.Checked || tireRotationCheckBox.Checked)
                MiscCharges(out totalMiscCharges);

            if (partsTextBox.Text!="" || laborTextBox.Text!="")
                OtherCharges(out totalPartsCharges, out totalLaborCharges);

            if (totalPartsCharges != 0)
                TaxCharges(in totalPartsCharges, out taxOnParts);
            
            TotalCharges(in totalOilLubeCharges, totalFlushCharges, totalMiscCharges,
                            totalPartsCharges, totalLaborCharges, taxOnParts, out totalCharges);

            ServicesAndLabor(in totalCharges, in taxOnParts, in totalPartsCharges,
                                        out servicesAndLaborTotal);


            // Ensure that error handling in the "OtherCharges" method was successful
            // If so, call method to populate text properties to display to user
            // If not, call method to clear labels that are displayed to user
            if (totalPartsCharges != -1 && totalLaborCharges != -1)

                // Ensure that the user has not left the form blank
                if (totalCharges != 0)
                    PopulateLabels(in servicesAndLaborTotal, in totalPartsCharges, in taxOnParts, in totalCharges);

                else
                    MessageBox.Show("The form has been left blank. You must enter at least one " +
                        "charge in order to calculate.");

            else
                ClearFees();
        }


        private void OilLubeCharges(out decimal totalOilLubeCharges)

            // This method calculates the total charges for oil and lube services
        {
            // Initialize variable
            totalOilLubeCharges = 0;

            // Check for oil change, add charge if selected
            if (oilChangeCheckBox.Checked)
                totalOilLubeCharges += OIL_CHANGE;

            // Check for lube job, add charge if selected
            if (lubeJobCheckBox.Checked)
                totalOilLubeCharges += LUBE_JOB;
        }


        private void FlushCharges(out decimal totalFlushCharges)

            // This method calculates the total charges for flushes
        {
            // Initialize variable
            totalFlushCharges = 0;

            // Check for radiator flush, add charge if selected
            if (radiatorFlushCheckBox.Checked)
                totalFlushCharges += RADIATOR_FLUSH;

            // Check for transmission flush, add charge if selected
            if (transmissionFlushCheckBox.Checked)
                totalFlushCharges += TRANSMISSION_FLUSH;
        }


        private void MiscCharges(out decimal totalMiscCharges)

            // This method calculates the total charges for inspections,
            // muffler replacement, and tire rotations
        {
            // Initialize variable
            totalMiscCharges = 0;

            // Check for inspection, add charge if selected
            if (inspectionCheckBox.Checked)
                totalMiscCharges += INSPECTION_CHARGE;

            // Check for muffler replacement, add charge if selected
            if (replaceMufflerCheckBox.Checked)
                totalMiscCharges += MUFFLER_REPLACEMENT;

            //Check for tire rotation, add charge if selected
            if (tireRotationCheckBox.Checked)
                totalMiscCharges += TIRE_ROTATION;
        }


        private void OtherCharges(out decimal partsCharge, out decimal laborCharge)

            // This method calculates the total charges for parts and labor
        {
            // Initialize variables
            partsCharge = 0;
            laborCharge = 0;

            // Check for parts charge, use TryParse & if/else to ensure valid data is entered.
            // If any letters, symbols, or a negative number is entered, show user a MessageBox
            // and assign the variable partsCharge = -1.
            if (partsTextBox.Text != "")

                if (decimal.TryParse(partsTextBox.Text, out decimal parts))

                    if (parts >= 0)
                        partsCharge += parts;

                    else
                    {
                        MessageBox.Show("You have entered a negative number for parts, only " +
                            "positive numbers or zero is allowed. You may also leave this blank.");
                        partsCharge = -1;
                    }

                else
                {
                    MessageBox.Show("You have submitted an invalid value for 'Parts'. " +
                        "Please enter a numerical value only. Only whole numbers and decimals " +
                        "are allowed. You may also leave this blank.");
                    partsCharge = -1;
                }

            // Check for labor charge, use TryParse to ensure valid data is entered.
            // If a non-numeric character, or a negative number is entered, show user a MessageBox
            // and assign laborCharge = -1
            if (laborTextBox.Text != "")

                if (decimal.TryParse(laborTextBox.Text, out decimal labor))

                    if (labor >= 0)
                        laborCharge += labor;

                    else
                    {
                        MessageBox.Show("You have entered a negative number for labor, only " +
                            "positive numbers or zero is allowed. You may also leave this blank.");
                        laborCharge = -1;
                    }

                else
                {
                    MessageBox.Show("You have submitted an invalid value for 'Labor'. " +
                        "Please enter a numerical value only. Only whole numbers and decimals " +
                        "are allowed. You may also leave this blank.");
                    laborCharge = -1;
                }
        }


        private void TaxCharges(in decimal totalPartsCharge, out decimal taxOnParts)

            // This method calculates the tax charge on parts
        {  
            taxOnParts = (totalPartsCharge * TAX);
        }


        private void TotalCharges(in decimal totalOilLubeCharges, in decimal totalFlushCharges,
                        in decimal totalMiscCharges, in decimal totalPartsCharge,
                        in decimal totalLaborCharge, in decimal taxOnLabor, out decimal totalCharges)

            // This method sums all charges and returns the total charges
        {
            totalCharges = totalOilLubeCharges + totalFlushCharges + totalMiscCharges + totalPartsCharge + totalLaborCharge + taxOnLabor;
        }


        private void ServicesAndLabor(in decimal totalCharges, in decimal taxOnParts,
                        in decimal totalPartsCharges, out decimal servicesAndLaborTotal)

            // This method calculates the total charges for services and labor
        {
            servicesAndLaborTotal = totalCharges - taxOnParts - totalPartsCharges;
        }


        private void PopulateLabels(in decimal servicesAndLaborTotal, in decimal totalPartsCharges,
                        in decimal taxOnParts, in decimal totalCharges)

            // This method populates the correct labels to display to the user
        {
            servicesAndLaborDisplayLabel.Text = servicesAndLaborTotal.ToString("C");
            partsSummaryDisplayLabel.Text = totalPartsCharges.ToString("C");
            taxDisplayLabel.Text = taxOnParts.ToString("C");
            totalFeesDisplayLabel.Text = totalCharges.ToString("C");
        }


        private void clearButton_Click(object sender, EventArgs e)

            // This event handler calls on methods to clear all checkboxes, textboxes,
            // and labels on the form when the "Clear" button is clicked
        {
            ClearOilLube();
            ClearFlushes();
            ClearMisc();
            ClearOther();
            ClearFees();
        }


        private void ClearOilLube()

            // This method clears the oil and lube checkboxes
        {
            oilChangeCheckBox.Checked = false;
            lubeJobCheckBox.Checked = false;
        }


        private void ClearFlushes()

            // This method clears the flush checkboxes
        {
            radiatorFlushCheckBox.Checked = false;
            transmissionFlushCheckBox.Checked = false;
        }


        private void ClearMisc()

            // This method clears the inspection, muffler, and tire rotation checkboxes
        {
            inspectionCheckBox.Checked = false;
            replaceMufflerCheckBox.Checked = false;
            tireRotationCheckBox.Checked = false;
        }


        private void ClearOther()

            // This method clears the parts, and labor text boxes
        {
            partsTextBox.Text = "";
            laborTextBox.Text = "";
        }


        private void ClearFees()

            // This method clears the display labels at the bottom of the form
        {
            servicesAndLaborDisplayLabel.Text = "";
            partsSummaryDisplayLabel.Text = "";
            taxDisplayLabel.Text = "";
            totalFeesDisplayLabel.Text = "";
        }


        private void exitButton_Click(object sender, EventArgs e)

            // This event handler closes the form when the "Exit" button is clicked
        {
            this.Close();
        }
    }
}
