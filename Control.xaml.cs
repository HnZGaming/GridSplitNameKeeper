﻿using System;
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

namespace GridSplitNameKeeper
{
    /// <summary>
    /// Interaction logic for Control
    /// </summary>
    public partial class Control : UserControl
    {
        private GridSplitNameKeeperCore Plugin { get; }

        public Control()
        {
            InitializeComponent();
        }

        public Control(GridSplitNameKeeperCore plugin) : this()
        {
            Plugin = plugin;
            DataContext = plugin.Config;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Plugin.Save();
        }
    }
}
