﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BxDRobotExporter.Wizard
{
    public partial class WizardPageControl : UserControl
    {
        public event Action FinishClicked;

        public WizardPageControl()
        {
            InitializeComponent();

            this.WizardNavigator.NextButton.Click += NextButton_Click;
            this.WizardNavigator.BackButton.Click += BackButton_Click;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (ActivePageIndex > 0)
            {
                ActivePageIndex--;
                for (int i = 1; i < Controls.Count; i++)
                {
                    if (i == ActivePageIndex + 1)
                        Controls[i].Visible = true;
                    else
                        Controls[i].Visible = false;
                }
                WizardNavigator.UpdateState(defaultNavigatorStates[ActivePageIndex + 1]);
                if (!((IWizardPage)(this.Controls[ActivePageIndex + 1])).Initialized)
                    ((IWizardPage)(this.Controls[ActivePageIndex + 1])).Initialize();
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            try
            {
                ((IWizardPage)(this.Controls[ActivePageIndex + 1])).OnNext();
                if (ActivePageIndex < Controls.Count - 1 && WizardNavigator.NextButton.Text == "Next >" || WizardNavigator.NextButton.Text == "Start >")
                {
                    ActivePageIndex++;
                    for (int i = 1; i < Controls.Count; i++)
                    {
                        if (i == ActivePageIndex + 1)
                            Controls[i].Visible = true;
                        else
                            Controls[i].Visible = false;
                    }
                    WizardNavigator.UpdateState(defaultNavigatorStates[ActivePageIndex + 1]);
                    if (!((IWizardPage)(this.Controls[ActivePageIndex + 1])).Initialized)
                        ((IWizardPage)(this.Controls[ActivePageIndex + 1])).Initialize();
                }
                else if (WizardNavigator.NextButton.Text == "Finish")
                    FinishClicked?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        int ActivePageIndex = 0;

        private Dictionary<int, WizardNavigator.WizardNavigatorState> defaultNavigatorStates = new Dictionary<int, WizardNavigator.WizardNavigatorState>();

        public void Add(UserControl page, WizardNavigator.WizardNavigatorState defaultState = WizardNavigator.WizardNavigatorState.Clean)
        {
            if (!(page is IWizardPage))
                throw new ArgumentException("ERROR: Given page does not extend IWizardPage", "page");
            page.Size = new Size(460, 653);
            page.Location = new Point(0, 0);
            page.Top = 0;
            page.Left = 0;
            page.Visible = false;
            page.BackColor = Color.Transparent;
            defaultNavigatorStates.Add(Controls.Count, defaultState);
            Controls.Add(page);
        }

        public void AddRange(UserControl[] pages, WizardNavigator.WizardNavigatorState[] defaultStates = null)
        {
            foreach (UserControl page in pages)
            {
                if (!(page is IWizardPage))
                    throw new ArgumentException("ERROR: Given page does not extend IWizardPage", "page");
                page.Size = new Size(460, 653);
                page.Location = new Point(0, 0);
                page.Top = 0;
                page.Left = 0;
                page.Visible = false;
                if (page.BackColor == Control.DefaultBackColor) page.BackColor = Color.Transparent;
                if (defaultStates == null)
                    defaultNavigatorStates.Add(Controls.Count, WizardNavigator.WizardNavigatorState.Clean);
                else
                    defaultNavigatorStates.Add(Controls.Count, defaultStates[pages.ToList().IndexOf(page)]);
                Controls.Add(page);
            }
        }

        public void BeginWizard()
        {
            Controls[1].Visible = true;
            WizardNavigator.UpdateState(defaultNavigatorStates[1]);
        }
    }
}